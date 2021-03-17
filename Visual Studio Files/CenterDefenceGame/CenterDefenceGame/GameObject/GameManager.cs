using CenterDefenceGame.GameObject.DrawObject;
using CenterDefenceGame.GameObject.EnemyObject;
using CenterDefenceGame.GameObject.LevelObject;
using CenterDefenceGame.GameObject.Page;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterDefenceGame.GameObject
{
	public delegate void ButtonDelegate();

	#region Enum

	public enum EMessageType
	{
		System,
		Debug,
		Warring,
		Error,
		Normal,
	}

	public enum EBlockType
	{
		EmptySpace,
		None,
		Wall,
		Core,
		ResourceGenerator,
		AmmunitionGenerator,
		Obstacle,
		EnemySpawner,
	}

	public enum EDirection
	{
		Destination,
		Right,
		UpRight,
		Up,
		UpLeft,
		Left,
		DownLeft,
		Down,
		DownRight,
		None,
	}

	public enum ESystemSituation
	{
		Start, // 게임 시작
		End,
		Shop,
		Standby,
		Build,
		Wave,
		Pause,
		WaveClear,
		WeaponShop,
		CoreShop,
	}

	public enum EDock
	{
		Center,
		Left,
		Right,
		Top,
		Bottom
	}

	public enum EGoDirection
	{
		Core,              // 벽을 제외한 코어를 향한다.
		AllExceptWall,     // 벽을 제외한 모든 구조물을 향한다.
		All,               // 모든 구조물을 향한다.
		Player,            // 벽과 구조물을 제외한 공간의 플레이어를 향한다.
		PlayerIgnoreBlock, // 벽을 제외한 구조물을 모두 무시하고 플레이어를 향한다.
		Center,            // 다 무시하고 중앙으로 간다. (코어 위치)
	}

	#endregion

	public class GameManager
	{

		public readonly Image SmallMark = Image.FromFile("ExplosionMark_0.png");
		public readonly Image BigMark = Image.FromFile("ExplosionMark_1.png");

		#region  메인 폼

		public Main MainForm;
		public Color BackgroundColor;

		#endregion

		#region  System

		private ESystemSituation GameSituation;
		public bool IsDebugInfoOn = false;

		#endregion

		#region  게임 오브젝트

		public Bitmap GameMapBitmap;
		public Bitmap GameConstructureBitmap;
		public VirtualMouse GameMouse;
		public LevelManager GameLevelManager;
		public GUI GameGUI;
		public Map GameMap;
		public Camera GameCamera;
		public Player GamePlayer;
		public PlayerBulletList GamePlayerBulletList;
		public EnemyList GameEnemyList;
		public DeadEffect[] DeadEffects;
		private int DeadEffectCounter = 0;
		private int MaxDeadEffect = 50;

		#endregion

		#region Game Pages
		
		private ShopPage GameShopPage;
		private StartPage GameStartPage;
		private PausePage GamePausePage;
		private EndPage GameEndPage;

		#endregion

		#region  키 스위치

		// 0 아무런 입력 없음
		// 1 띄어진 상태
		// 2 눌린 상태
		public Dictionary<Keys, int> IsKeyReleasedSwitch;
		public Dictionary<MouseButtons, int> IsMouseReleasedSwitch;
		public MouseButtons[] MouseArray;
		public Keys[] KeyArray;

		#endregion

		#region  게임속 마우스 좌표

		private int MouseX = 0;
		private int MouseY = 0;

		#endregion

		#region  Level and Wave System
		
		private int WhiteScreenAlpha = 0;
		private int WaveTimer;
		public int WaveTimerMax;
		public int EndWave;
		private int Wave;
		private int AmmunitionCount;
		private int ResourceCount;

		#endregion

		public GameManager(Main main)
		{
			this.MainForm = main;
			this.BackgroundColor = Color.FromArgb(15, 15, 15);
			this.GameSituation = ESystemSituation.Start;

			#region Data Set

			this.WaveTimerMax				= DataTable.WaveTimerMax;
			this.EndWave					= DataTable.EndWave;

			#endregion

			#region Initialize Key Released Switches

			this.IsKeyReleasedSwitch = new Dictionary<Keys, int>();
			this.IsKeyReleasedSwitch.Add(Keys.W, 0);
			this.IsKeyReleasedSwitch.Add(Keys.A, 0);
			this.IsKeyReleasedSwitch.Add(Keys.S, 0);
			this.IsKeyReleasedSwitch.Add(Keys.D, 0);
			this.IsKeyReleasedSwitch.Add(Keys.Escape, 0);
			this.IsKeyReleasedSwitch.Add(Keys.F1, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F2, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F3, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F4, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F5, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F6, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F7, 0); // Debugging Key
			this.IsKeyReleasedSwitch.Add(Keys.F8, 0); // Debugging Key
			this.KeyArray = new Keys[this.IsKeyReleasedSwitch.Count()];
			
			int keyCount = 0;
			foreach(var item in this.IsKeyReleasedSwitch)
			{
				this.KeyArray[keyCount] = item.Key;
				keyCount ++;
			}

			this.IsMouseReleasedSwitch = new Dictionary<MouseButtons, int>();
			this.IsMouseReleasedSwitch.Add(MouseButtons.Left, 0);
			this.IsMouseReleasedSwitch.Add(MouseButtons.Right, 0);
			this.IsMouseReleasedSwitch.Add(MouseButtons.Middle, 0);
			this.MouseArray = new MouseButtons[this.IsMouseReleasedSwitch.Count()];
			
			int mouseCount = 0;
			foreach(var item in this.IsMouseReleasedSwitch)
			{
				this.MouseArray[mouseCount] = item.Key;
				mouseCount ++;
			}

			#endregion
			
			#region Initialize Game Map and sprites

			this.GameMap = new Map(this);
			this.GameMapBitmap = this.GameMap.GetMapBitmap();
			this.GameConstructureBitmap = this.GameMap.GetConstructureBitmap();

			#endregion

			#region Initialize Game Objects

			int cellSize = GameMap.CellSize;
			int mapWidth  = GameMap.Width;
			int mapHeight = GameMap.Height;
			int centerX = cellSize * (mapWidth / 2);
			int centerY = cellSize * (mapHeight / 2);

			int formWidth  = this.MainForm.ClientSize.Width;
			int formHeight = this.MainForm.ClientSize.Height;

			this.GameGUI = new GUI(this);
			this.GameMouse = new VirtualMouse(this);
			this.GameLevelManager = new LevelManager(this);
			this.GameCamera = new Camera(this, centerX - formWidth / 2, centerY - formHeight / 2, formWidth, formHeight);
			this.GamePlayer = new Player(this);
			this.GamePlayerBulletList = new PlayerBulletList(this);
			this.GameEnemyList = new EnemyList(this);

			this.DeadEffects = new DeadEffect[this.MaxDeadEffect];

			#endregion

			#region Initialize Game Pages

			this.GameShopPage = new ShopPage(this);
			this.GameStartPage = new StartPage(this);
			this.GamePausePage = new PausePage(this);
			this.GameEndPage = new EndPage(this);

			#endregion

			this.Reset();
		}

		public void Reset()
		{
			#region Reset Key Released Switches

			foreach(var item in this.KeyArray)
			{
				this.IsKeyReleasedSwitch[item] = 0;
			}
			
			foreach(var item in this.MouseArray)
			{
				this.IsMouseReleasedSwitch[item] = 0;
			}

			#endregion

			#region Reset Game Map and sprites

			if (this.GameMapBitmap != null)
			{
				this.GameMapBitmap.Dispose();
			}
			
			if (this.GameConstructureBitmap != null)
			{
				this.GameConstructureBitmap.Dispose();
			}

			this.GameMapBitmap = this.GameMap.GetMapBitmap();
			this.GameConstructureBitmap = this.GameMap.GetConstructureBitmap();

			#endregion

			#region Reset Game Objects

			int cellSize = GameMap.CellSize;
			int mapWidth  = GameMap.Width;
			int mapHeight = GameMap.Height;

			int formWidth  = this.MainForm.ClientSize.Width;
			int formHeight = this.MainForm.ClientSize.Height;

			this.GameGUI.Reset();
			this.GameMouse.Reset();
			this.GameLevelManager.Reset();
			this.GameMap.Reset();
			this.GameEnemyList.Reset();
			this.GamePlayer.Reset(this.GameMap.GetCenterX(), this.GameMap.GetCenterY());
			this.GamePlayerBulletList.Reset();

			this.DeadEffectCounter = 0;
			for (int i = 0; i < this.MaxDeadEffect; i ++)
			{
				this.DeadEffects[i] = new DeadEffect(this);
			}

			#endregion

			#region Reset Game Pages

			this.GameShopPage.Reset();
			this.GameStartPage.Reset();
			this.GamePausePage.Reset();
			this.GameEndPage.Reset();

			#endregion

			#region Reset Level and Wave System

			this.GameSituation = ESystemSituation.Start;
			this.WaveTimer = this.WaveTimerMax;
			this.Wave = 1;
			this.AmmunitionCount = this.GameMap.GetMaxAmmunition();
			this.ResourceCount = DataTable.InitializeResource;

			#endregion
		}

		/*
		 * 게임 비트맵 재설정시 반드시 기존 비트맵을 해제해줘야한다.
		 * 메모리 릭 생김
		 */

		public void Update(float deltaRatio, Graphics graphics, Point mouseMovedAmount, Point guiMousePosition)
		{
			#region Reset all GameBitmaps

			if (this.GameMap.IsNeedFrontRefresh)
			{
				this.GameMap.IsNeedFrontRefresh = false;
				if (this.GameConstructureBitmap != null)
				{
					this.GameConstructureBitmap.Dispose();
					this.GameConstructureBitmap = this.GameMap.GetConstructureBitmap();
				}
			}
			
			if (this.GameMap.IsNeedBackRefresh)
			{
				this.GameMap.IsNeedBackRefresh = false;
				if (this.GameMapBitmap != null)
				{
					this.GameMapBitmap.Dispose();
					this.GameMapBitmap = this.GameMap.GetMapBitmap();
				}
			}

			#endregion

			#region Game Logic

			// Ammunition Count
			
			int maxAmmo = this.GameMap.GetMaxAmmunition();

			if (this.AmmunitionCount > maxAmmo)
			{
				this.AmmunitionCount = maxAmmo;
			}
			
			int bonus = 0;

			// Wave Timer Count Down
			if (this.GetGameSituation() == ESystemSituation.Wave)
			{
				if (this.GameLevelManager.IsSpawnComplete() && this.GameEnemyList.IsEmpty())
				{
					bonus = (this.WaveTimer / 60) * this.Wave;

					// 마지막 웨이브이면
					if (this.Wave == this.EndWave)
					{
						this.SetGameSituation(ESystemSituation.End);
					}
					else
					{
						this.WaveClear();
					}
				}
				
				if (this.GameMap.IsCoreDestroyed())
				{
					// 코어가 파괴되어 게임 오버됨
					this.SetGameSituation(ESystemSituation.End);
				}
				else
				{
					if (this.WaveTimer > 0)
					{
						this.WaveTimer --;
					}
					else
					{
						// 마지막 웨이브이면
						if (this.Wave == this.EndWave)
						{
							this.SetGameSituation(ESystemSituation.End);
						}
						else
						{
							this.WaveClear();
						}
					}
				}

			}

			if (this.GetGameSituation() == ESystemSituation.WaveClear)
			{
				int clearDelay = (int)(5 * deltaRatio);

				if (this.WhiteScreenAlpha > clearDelay)
				{
					this.WhiteScreenAlpha -= clearDelay;
				}
				else
				{
					this.WhiteScreenAlpha = 0;
					
					// Wave Level Up
					StringBuilder sb = new StringBuilder();
					sb.Append("Wave ");
					sb.Append(this.Wave);
					sb.Append(" Clear !");
					this.GameGUI.AddNotification(EMessageType.System, sb.ToString());
					
					float coreResourceRatio = (DataTable.CoreGenerateResourceRatio[0, GameMap.LevelCoreResourceRatio] / 100f);

					bonus = (int)(((WaveTimer / 100) + (Wave * 10)) * (coreResourceRatio + 1));

					if (bonus > 0)
					{
						this.GameGUI.AddNotification(EMessageType.Normal, "클리어 보너스 !");
						this.AddRecourceCount(bonus);
					}

					this.Wave ++;
					this.SetGameSituation(ESystemSituation.Standby);
				}
			}

			#endregion

			#region Updating
			
			// Game Camera and Mouse
			this.GameCamera.Update(deltaRatio, (int)this.GamePlayer.GetX(), (int)this.GamePlayer.GetY());
			this.MouseX = GameMouse.Position.X + this.GameCamera.GetX();
			this.MouseY = GameMouse.Position.Y + this.GameCamera.GetY();
			this.GameMouse.Update(deltaRatio, mouseMovedAmount);
			this.GameLevelManager.Update();
			this.GameMap.Update(this.GameMouse.GetPosition());

			ESystemSituation situation = this.GetGameSituation();

			// Game Player Bullets Enemies

			if (situation == ESystemSituation.Wave ||
				situation == ESystemSituation.Standby ||
				situation == ESystemSituation.Build ||
				situation == ESystemSituation.WaveClear ||
				situation == ESystemSituation.End)
			{
				if (situation != ESystemSituation.WaveClear)
				{
					this.GamePlayer.Update(deltaRatio);
				}

				this.GamePlayerBulletList.Update(deltaRatio);
				this.GameEnemyList.Update(deltaRatio);
			}

			// Game GUI
			this.GameGUI.Update();

			// Game Pages
			this.GameShopPage.Update(this.GameMouse.GetPosition(), guiMousePosition);
			this.GameStartPage.Update(guiMousePosition);
			this.GamePausePage.Update(guiMousePosition);
			this.GameEndPage.Update(guiMousePosition);

			#endregion

			#region Drawing

			// Draw Background Color
			graphics.Clear(this.BackgroundColor);

			// Draw Background Objects
			graphics.DrawImage(this.GameMapBitmap		  , -this.GameCamera.GetX(), -this.GameCamera.GetY());
			graphics.DrawImage(this.GameConstructureBitmap, -this.GameCamera.GetX(), -this.GameCamera.GetY());
			
			// Draw
			this.GamePlayer.Draw(graphics);
			this.GameEnemyList.Draw(graphics);
			this.GameMap.DrawAndUpdateBuildMode(graphics);
			this.GameMap.Draw(graphics);
			this.GamePlayerBulletList.Draw(graphics);

			if (this.WhiteScreenAlpha > 0)
			{
				using (SolidBrush sb = new SolidBrush(Color.FromArgb(this.WhiteScreenAlpha, 255, 255, 255)))
				{
					graphics.FillRectangle(sb, -1000, -1000, 3000, 3000);
				}
			}
			
			// 상점은 디스플레이보다 앞에 출력
			this.GameShopPage.Draw(graphics);

			// Draw HMD
			this.GameGUI.DrawHMD(graphics);
			
			// Draw Game Pages
			this.GameStartPage.Draw(graphics);
			this.GamePausePage.Draw(graphics);
			this.GameEndPage.Draw(graphics);

			// Draw Effects

			for (int index = 0; index < this.MaxDeadEffect; index ++)
			{
				this.DeadEffects[index].Draw(graphics);
			}

			// Draw Debug Infomation
			if (this.MainForm.IsDebugging && this.IsDebugInfoOn)
			{
				this.GameMap.DrawDebugInfo(graphics);
			}

			this.GameGUI.DrawNotification(graphics);

			// Draw Mouse
			this.GameMouse.Draw(graphics, guiMousePosition);

			#endregion
			
			#region KeyBoard and Mouse Inputs

			this.RunKeyboardButtonAction();
			this.RunMouseButtonAction();

			#endregion

			#region Debugger

			// 디버그 모드 툴팁 출력
			if(this.MainForm.IsDebugging)
			{
				graphics.FillRectangle(Brushes.Red, 0, 0, 100, 33);
				graphics.DrawString("Debug Mode", this.MainForm.Font, Brushes.White, 10, 10);
			}

			#endregion
		}

		#region Getter Setter

		public void WaveClear()
		{
			if (this.GetGameSituation() == ESystemSituation.Wave)
			{
				this.WhiteScreenAlpha = 255;
				this.WaveTimer = this.WaveTimerMax;
				this.SetGameSituation(ESystemSituation.WaveClear);
				this.GameMap.IsNeedBackRefresh = true;
				this.GameMap.IsNeedFrontRefresh = true;
			}
		}

		public int GetWaveTime()
		{
			return WaveTimer;
		}

		public int GetTimer()
		{
			return this.WaveTimer / 60;
		}

		public ESystemSituation GetGameSituation()
		{
			return this.GameSituation;
		}

		public void SetGameSituation(ESystemSituation situation)
		{
			this.GameSituation = situation;
		}

		public int GetWave()
		{
			if (this.Wave > 0)
			{
				return this.Wave;
			}
			else
			{
				return 1;
			}
		}

		public int GetAmmunitionCount()
		{
			return this.AmmunitionCount;
		}

		public void AddAmmunitionCount(int amount)
		{
			if (amount == 0)
			{
				return;
			}

			int maxAmmo = this.GameMap.GetMaxAmmunition();

			if (this.AmmunitionCount <= maxAmmo - amount)
			{
				this.AmmunitionCount += amount;
			}
			else
			{
				this.AmmunitionCount = maxAmmo;
			}
		}

		public bool ConsumAmmunition(int ammunitionAmount)
		{
			if (this.AmmunitionCount >= ammunitionAmount)
			{
				this.AmmunitionCount -= ammunitionAmount;
				return true;
			}
			else
			{
				this.GameGUI.AddNotification(EMessageType.Normal, "탄약이 부족합니다.");
				return false;
			}
		}

		public int GetResourceCount()
		{
			return this.ResourceCount;
		}

		public void AddRecourceCount(int amount)
		{
			if (amount == 0)
			{
				return;
			}

			this.ResourceCount += amount;
			StringBuilder sb = new StringBuilder();
			sb.Append("자원 +");
			sb.Append(amount);
			this.GameGUI.AddNotification(EMessageType.Normal, sb.ToString());
		}

		public bool ConsumResource(int resourceAmount)
		{
			if (this.ResourceCount >= resourceAmount)
			{
				this.ResourceCount -= resourceAmount;
				return true;
			}
			else
			{
				this.GameGUI.AddNotification(EMessageType.Normal, "자원이 부족합니다.");
				return false;
			}
		}

		public Point GetIngameMousePosition()
		{
			return new Point(this.MouseX, this.MouseY);
		}

		#endregion

		#region Level Functions

		public void GoNextWave()
		{
			if ((this.GameSituation == ESystemSituation.Standby) ||
				(this.GameSituation == ESystemSituation.Shop) ||
				(this.GameSituation == ESystemSituation.Build))
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Wave ");
				sb.Append(this.GetWave());
				sb.Append(" Start !");

				this.GameGUI.AddNotification(EMessageType.System, sb.ToString());
				this.GameSituation = ESystemSituation.Wave;
			}
		}

		public void CreateDeadEffect(int x, int y, int size, bool isBig)
		{
			if (this.GetGameSituation() == ESystemSituation.WaveClear)
			{
				return;
			}

			if (this.DeadEffectCounter < this.MaxDeadEffect - 1)
			{
				this.DeadEffectCounter ++;
			}
			else
			{
				this.DeadEffectCounter = 0;
			}

			this.DeadEffects[this.DeadEffectCounter].Active(x, y, size, isBig);
		}

		#endregion

		#region General Collision Detection

		public bool CheckCollisionRapidly(float x, float y, int width, int height, Vector2D point1, Vector2D point2)
		{
			float baseHalfWidth  = width  / 2;
			float baseHalfHeight = height / 2;

			float lineX = (point1.X + point2.X) / 2;
			float lineY = (point1.Y + point2.Y) / 2;

			float lineHalfWidth  = Math.Abs(point1.X - lineX);
			float lineHalfHeight = Math.Abs(point1.Y - lineY);

			if ((Math.Abs(x - lineX) < (baseHalfWidth  + lineHalfWidth )) &&
				(Math.Abs(y - lineY) < (baseHalfHeight + lineHalfHeight)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public Vector2D CheckCollisionPrecise(float x, float y, int width, int height, Vector2D point1, Vector2D point2)
		{
			float baseHalfWidth  = width  / 2;
			float baseHalfHeight = height / 2;

			float baseLeft		= x - baseHalfWidth;
			float baseRight		= x + baseHalfWidth;
			float baseTop		= y - baseHalfHeight;
			float baseBottom	= y + baseHalfHeight;

			float lineWidth		= point2.X - point1.X;
			float lineHeight	= point2.Y - point1.Y;

			float ratioY1 = (baseTop	 - point1.Y) / lineHeight;
			float ratioY2 = (baseBottom  - point1.Y) / lineHeight;
			float ratioX1 = (baseLeft	 - point1.X) / lineWidth;
			float ratioX2 = (baseRight	 - point1.X) / lineWidth;

			float spotX1 = point1.X + lineWidth * ratioY1;
			float spotX2 = point1.X + lineWidth * ratioY2;
			float spotY1 = point1.Y + lineHeight * ratioX1;
			float spotY2 = point1.Y + lineHeight * ratioX2;

			Vector2D minVector = null;

			// 방향이름의 뜻은 의미가 없고, 기준선이 좌상단에서 우하단으로 진행할 때
			// 사각형의 우상단에 접촉한 경우를 기준으로 작성했기에 참고용으로 적는다.
			//  && (0 <= ratioY1) && (ratioY1 <= 1) 는 본래 빠른 검사가 없을 때 
			// 접촉 검사가 범위 안에서만 이루어지도록 하는 역할을 했으나
			// 빠른검사가 동일한 역할을 대신 함으로써
			// 빠른검사로 고속으로 확인한 후 접촉할 수 있는 경우에만 이 검사를 진행한다.

			// 상단
			if ((baseLeft < spotX1) && (spotX1 < baseRight))// && (0 <= ratioY1) && (ratioY1 <= 1))
			{
				if (minVector == null)
				{
					minVector = new Vector2D(spotX1, baseTop);
				}
				else
				{
					// 의미 없음
				}
			}
			
			// 하단
			if ((baseLeft < spotX2) && (spotX2 < baseRight))// && (0 <= ratioY2) && (ratioY2 <= 1))
			{
				if (minVector == null)
				{
					minVector = new Vector2D(spotX2, baseBottom);
				}
				else
				{
					float minW = Math.Abs(minVector.X - point1.X);
					float minH = Math.Abs(minVector.Y - point1.Y);
					
					float detectW = Math.Abs(spotX2 - point1.X);
					float detectH = Math.Abs(baseBottom - point1.Y);

					if (detectW < minW && detectH < minH)
					{
						minVector.X = spotX2;
						minVector.Y = baseBottom;
					}
				}
			}
			
			// 좌측
			if ((baseTop < spotY1) && (spotY1 < baseBottom))// && (0 <= ratioX1) && (ratioX1 <= 1))
			{
				if (minVector == null)
				{
					minVector = new Vector2D(baseLeft, spotY1);
				}
				else
				{
					float minW = Math.Abs(minVector.X - point1.X);
					float minH = Math.Abs(minVector.Y - point1.Y);
					
					float detectW = Math.Abs(baseLeft - point1.X);
					float detectH = Math.Abs(spotY1 - point1.Y);

					if (detectW < minW && detectH < minH)
					{
						minVector.X = baseLeft;
						minVector.Y = spotY1;
					}
				}
			}
			
			// 우측
			if ((baseTop < spotY2) && (spotY2 < baseBottom))// && (0 <= ratioX2) && (ratioX2 <= 1))
			{
				if (minVector == null)
				{
					minVector = new Vector2D(baseRight, spotY2);
				}
				else
				{
					float minW = Math.Abs(minVector.X - point1.X);
					float minH = Math.Abs(minVector.Y - point1.Y);
					
					float detectW = Math.Abs(baseRight - point1.X);
					float detectH = Math.Abs(spotY2 - point1.Y);

					if (detectW < minW && detectH < minH)
					{
						minVector.X = baseRight;
						minVector.Y = spotY2;
					}
				}
			}
			
			return minVector;
		}

		#endregion

		public void RunKeyboardButtonAction()
		{
			// Key Input Detect
			foreach(var key in this.KeyArray)
			{
				if (this.MainForm.IsKeyPressed[key])
				{
					if (this.IsKeyReleasedSwitch[key] == 0)
					{
						this.IsKeyReleasedSwitch[key] = 1;
					}
					else if (this.IsKeyReleasedSwitch[key] == 1)
					{
						this.IsKeyReleasedSwitch[key] = 2;
					}
				}
				else
				{
					this.IsKeyReleasedSwitch[key] = 0;
				}
			}

			#region General Key Inputs
			
			if(this.IsKeyReleasedSwitch[Keys.Escape] == 1)
			{
				switch(this.GetGameSituation())
				{
					case ESystemSituation.End:
					case ESystemSituation.Start:
						return;

					case ESystemSituation.Pause:
						this.GamePausePage.ResumeGame();
						return;
						
					case ESystemSituation.Standby:
					case ESystemSituation.Wave:
						this.GameGUI.AddNotification(EMessageType.System, "일시 정지");
						this.SetGameSituation(ESystemSituation.Pause);
						return;

					case ESystemSituation.Shop:
					case ESystemSituation.Build:
						this.SetGameSituation(ESystemSituation.Standby);
						return;
						
					case ESystemSituation.CoreShop:
					case ESystemSituation.WeaponShop:
						this.SetGameSituation(ESystemSituation.Shop);
						return;
				}
			}

			#endregion

			#region Debug Key Inputs

			if (this.IsKeyReleasedSwitch[Keys.F1] == 1)
			{
				if (!this.MainForm.IsDebugging)
				{
					this.GameGUI.SetNotificationMaxCount(99);
				}

				this.GameGUI.AddNotification(EMessageType.System, "########################################");
				this.GameGUI.AddNotification(EMessageType.System, "[F1] 도움말.");
				this.GameGUI.AddNotification(EMessageType.System, "[F2] 디버거 모드를 설정");
				this.GameGUI.AddNotification(EMessageType.System, "[F3] 디버그 정보 출력");
				this.GameGUI.AddNotification(EMessageType.System, "[F4] 경로 그리드 정보 전환");
				this.GameGUI.AddNotification(EMessageType.System, "[F5] 현재 웨이브 종료");
				this.GameGUI.AddNotification(EMessageType.System, "[F6] 자원 10000 추가, 총알 최대로");
				this.GameGUI.AddNotification(EMessageType.System, "[F7] 기능 없음");
				this.GameGUI.AddNotification(EMessageType.System, "[F8] 웨이브 1 추가");
				this.GameGUI.AddNotification(EMessageType.System, "########################################");

				if (!this.MainForm.IsDebugging)
				{
					this.GameGUI.SetNotificationMaxCount(1);
				}
			}
			
			if(this.IsKeyReleasedSwitch[Keys.F2] == 1)
			{
				this.MainForm.IsDebugging = !this.MainForm.IsDebugging;

				this.GameMapBitmap.Dispose();
				this.GameMapBitmap = this.GameMap.GetMapBitmap();

				if (this.MainForm.IsDebugging)
				{
					this.GameGUI.AddNotification(EMessageType.System, "Debug Mode Enabled");
					this.GameGUI.AddNotification(EMessageType.Warring, "디버거 모드에서는 프레임 드랍이 있을 수 있습니다.");
					this.GameGUI.SetNotificationMaxCount(99);
				}
				else
				{
					this.GameGUI.AddNotification(EMessageType.System, "Debug Mode Disabled");
					this.GameGUI.SetNotificationMaxCount(1);
				}
			}

			if(this.IsKeyReleasedSwitch[Keys.F3] == 1)
			{
				if (this.MainForm.IsDebugging)
				{
					this.IsDebugInfoOn = !this.IsDebugInfoOn;

					StringBuilder sb = new StringBuilder();
					sb.Append("IsDebugInfoOn = ");
					sb.Append(this.IsDebugInfoOn.ToString());

					this.GameGUI.AddNotification(EMessageType.Debug, sb.ToString());
				}
				else
				{
					this.GameGUI.AddNotification(EMessageType.System, "디버그 모드를 활성화하십시오.");
				}
			}

			if(this.IsKeyReleasedSwitch[Keys.F4] == 1)
			{
				if (this.IsDebugInfoOn)
				{
					if (this.GameMap.DebugShowGrid < EGoDirection.PlayerIgnoreBlock)
					{
						this.GameMap.DebugShowGrid ++;
					}
					else
					{
						this.GameMap.DebugShowGrid = EGoDirection.Core;
					}

					StringBuilder sb = new StringBuilder();
					sb.Append("Path Grid = ");
					sb.Append(this.GameMap.DebugShowGrid.ToString());

					this.GameGUI.AddNotification(EMessageType.Debug, sb.ToString());
				}
			}
			
			if(this.IsKeyReleasedSwitch[Keys.F5] == 1)
			{
				if (this.MainForm.IsDebugging)
				{
					if (this.GameSituation == ESystemSituation.Wave)
					{
						this.GameGUI.AddNotification(EMessageType.Debug, "End Current Wave");
						this.WaveTimer = 0;
					}
					else
					{
						this.GameGUI.AddNotification(EMessageType.Debug, "웨이브중이 아닐 때는 웨이브를 끝낼 수 없습니다.");
					}
				}
				else
				{
					this.GameGUI.AddNotification(EMessageType.System, "디버그 모드를 활성화하십시오.");
				}
			}
			
			
			if(this.IsKeyReleasedSwitch[Keys.F6] == 1)
			{
				if (this.MainForm.IsDebugging)
				{
					AddRecourceCount(10000);
					AddAmmunitionCount(10000);
				}
				else
				{
					this.GameGUI.AddNotification(EMessageType.System, "디버그 모드를 활성화하십시오.");
				}
			}
			
			if(this.IsKeyReleasedSwitch[Keys.F7] == 1)
			{

			}

			if(this.IsKeyReleasedSwitch[Keys.F8] == 1)
			{
				if (this.MainForm.IsDebugging)
				{
					if (this.Wave < this.EndWave)
					{
						this.Wave ++;
						this.GameGUI.AddNotification(EMessageType.Debug, "GameManager.Wave ++");

						StringBuilder sb = new StringBuilder();
						sb.Append("GameManager.Wave = ");
						sb.Append(this.Wave);

						this.GameGUI.AddNotification(EMessageType.Debug, sb.ToString());
					}
					else
					{
						this.GameGUI.AddNotification(EMessageType.Debug, "최대 웨이브 입니다.");
					}
				}
				else
				{
					this.GameGUI.AddNotification(EMessageType.System, "디버그 모드를 활성화하십시오.");
				}

			}

			#endregion
		}

		public void RunMouseButtonAction()
		{
			// Mouse Input Detect
			foreach(var button in this.MouseArray)
			{
				if (this.MainForm.IsMousePressed[button])
				{
					this.IsMouseReleasedSwitch[button] = 2;
				}
				else
				{
					// Released Moment
					if (this.IsMouseReleasedSwitch[button] == 2)
					{
						this.IsMouseReleasedSwitch[button] = 1;
					}
					else if (this.IsMouseReleasedSwitch[button] == 1)
					{
						this.IsMouseReleasedSwitch[button] = 0;
					}
				}
			}

			#region General Button Inputs

			if (this.IsMouseReleasedSwitch[MouseButtons.Left] == 1)
			{

			}
			
			if (this.IsMouseReleasedSwitch[MouseButtons.Right] == 1)
			{
				if (this.MainForm.IsDebugging && this.GetGameSituation() == ESystemSituation.Wave)
				{
					// 적군 생성
				}
			}
			
			if (this.IsMouseReleasedSwitch[MouseButtons.Middle] == 1)
			{
				// 아마 쓸일이 없을듯
			}

			#endregion
		}

		#region Draw Objects

		public void DrawDarkScreen(Graphics graphics, int alpha)
		{
			using(SolidBrush sb = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0)))
			{
				graphics.FillRectangle(sb, -1000, -1000, 3000, 3000);
			}
		}

		#endregion

		/// <summary>
		/// -1 좌측 0 중앙 1 우측
		/// </summary>
		/// <param name="text"></param>
		/// <param name="penWidth"></param>
		/// <param name="textColor"></param>
		/// <param name="penColor"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fontName"></param>
		/// <param name="fontStyle"></param>
		/// <param name="fontSize"></param>
		/// <param name="alignCenter"></param>
		/// <returns></returns>
		public Bitmap GetTextBitmap(string text, int penWidth, Color textColor, Color penColor, int width, int height,
									string fontName, int fontStyle, int fontSize, int alignCenter)
		{
			Bitmap bitmap = new Bitmap(width, height);
			Graphics g = Graphics.FromImage(bitmap);

			// 비트맵에 문자열을 출력한다.
			using(GraphicsPath textPath = new GraphicsPath())
			{
				g.SmoothingMode = SmoothingMode.HighQuality;

				Point point = new Point(width / 2, height / 2);
				
				if (alignCenter == -1)
				{
					point.X = 5;
				}
				else if (alignCenter == 1)
				{
					point.X = width - 5;
				}

				using(StringFormat sf = new StringFormat())
				using(FontFamily ff = new FontFamily(fontName))
				{
					if (alignCenter == 0)
					{
						sf.Alignment = StringAlignment.Center;
					}
					else if (alignCenter == 1)
					{
						sf.Alignment = StringAlignment.Far;
					}

					sf.LineAlignment = StringAlignment.Center;

					textPath.AddString(text, ff, fontStyle, fontSize, point, sf);
				}

				using (Pen pen = new Pen(penColor, penWidth))
				{
					pen.LineJoin = LineJoin.Round;
					g.DrawPath(pen, textPath);
				}

				using (SolidBrush sb = new SolidBrush(textColor))
				{
					g.FillPath(sb, textPath);
				}

				g.SmoothingMode = SmoothingMode.None;
			}

			g.Dispose();
			return bitmap;
		}
		
		public Bitmap GetImageButtonBitmap(Image image, int width, int height, float imageScale)
		{
			Bitmap bitmap = new Bitmap(width, height);
			Graphics g = Graphics.FromImage(bitmap);

			int imageWidth  = (int)(image.Width * imageScale);
			int imageHeight = (int)(image.Height * imageScale);

			// 비트맵에 이미지를 출력한다.
			using(GraphicsPath textPath = new GraphicsPath())
			{
				Point point = new Point((width - imageWidth) / 2, (height - imageHeight) / 2);
				int offset = 1;
				
				using(SolidBrush sb = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
				{
					g.FillRectangle(sb, offset, offset, width - offset * 2, height - offset * 2);
				}

				g.DrawImage(image, point.X + offset, point.Y + offset, imageWidth, imageHeight);

				using(Pen pen = new Pen(Color.White, offset))
				{
					g.DrawRectangle(pen, offset, offset, width - offset * 2, height - offset * 2);
				}

				g.SmoothingMode = SmoothingMode.None;
			}

			g.Dispose();
			return bitmap;
		}
	}
}
