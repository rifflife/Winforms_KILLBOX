using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterDefenceGame.GameObject.LevelObject
{
	public class Map
	{
		#region External Data

		private Bitmap[] BlockImages;

		#endregion

		#region Map Grid

		private GameManager Manager;
		private EBlockType[,] MapGrid;

		public readonly int CellSize = 70;
		public readonly int Width = 20;
		public readonly int Height = 20;

		private int[,] BlockHealthPoint;

		private int[,] HitDelay;
		private int HitDelayIni = 3;
		private bool IsHit = false;
		private SolidBrush HitBrush = new SolidBrush(Color.FromArgb(100, 255, 0, 0));

		#endregion

		#region Component

		private List<Point> EnemySpawnerPoints;
		private int EnemySpawnerIndex;

		private int CoreAmmoCount;
		private int AmmunitionGeneratorAmmoCount;
		private int CoreAmmoGeneratorAmount;

		private int ResourceGeneratorMakeAmount;
		private int CoreResourceMakeAmount;

		private int ResourceGeneratorHealthPoint;
		private int AmmunitionGeneratorHealthPoint;
		private int WallHealthPoint;
		private int ObstacleHealthPoint;

		private int ResourceGeneratorCount;

		private int AmmunitionDelayIniLong;
		private int AmmunitionDelayIniShort;
		private bool IsFirstAmmunitionDelay;

		private int AmmunitionDelay;
		private int AmmunitionGeneratorCount;
		private int WaveCheck;
		
		private int CoreHealthPointIni;
		private int CoreHealthPoint;

		private int CoreRegen;
		private int CoreRegenDelay;
		private readonly int CoreRegenDelayIni = 60;

		private float CoreAmmoHaveRatio;
		private float CoreAmmunitionGenerateDelay;
		private float CoreResourceGenerateRatio;

		public int LevelCoreHealth;
		public int LevelCoreRegen;
		public int LevelCoreAmmunitionMakeTime;
		public int LevelCoreResourceRatio;
		public int LevelCoreAmmoHaveRatio;


		#endregion
		
		#region Pricese

		private int PriceWall;
		private int PriceObstacle;
		private int PriceAmmunitionGenerator;
		private int PriceResourceGenerator;

		#endregion

		#region Path Finding

		// 벽을 제외한 코어를 향한다.
		private EDirection[,] PathToCore;
		private List<EBlockType> StartFromCore;
		private List<EBlockType> PathToCoreException;
		
		// 벽을 제외한 모든 구조물을 향한다.
		private EDirection[,] PathToAllExceptWall;
		private List<EBlockType> StartFromAllExceptWall;
		private List<EBlockType> PathToAllExceptWallException;
		
		// 모든 구조물을 향한다.
		private EDirection[,] PathToAll;
		private List<EBlockType> StartFromAll;
		private List<EBlockType> PathToAllException;
		
		// 벽과 구조물을 제외한 공간의 플레이어를 향한다.
		private EDirection[,] PathToPlayer;
		private List<EBlockType> PathToPlayerException;
		
		// 벽을 제외한 구조물을 모두 무시하고 플레이어를 향한다.
		private EDirection[,] PathToPlayerIgnoreBlock;
		private List<EBlockType> PathToPlayerIgnoreBlockException;

		#endregion

		#region Map Section Positions

		private int CenterX = 0;
		private int CenterY = 0;

		private int GridX1 = 0;
		private int GridX2 = 0;
		private int GridX3 = 0;
		private int GridX4 = 0;
		private int GridY1 = 0;
		private int GridY2 = 0;
		private int GridY3 = 0;
		private int GridY4 = 0;

		private int SpawnerX1 = 0;
		private int SpawnerX2 = 0;
		private int SpawnerX3 = 0;
		private int SpawnerX4 = 0;
		private int SpawnerY1 = 0;
		private int SpawnerY2 = 0;
		private int SpawnerY3 = 0;
		private int SpawnerY4 = 0;

		private int[] ExpandTurn;

		#endregion

		#region Build Mode

		private SolidBrush BuildPossibleBrush = new SolidBrush(Color.FromArgb(150, 0, 255, 0));
		private SolidBrush BuildImpossibleBrush = new SolidBrush(Color.FromArgb(150, 255, 0, 0));
		private SolidBrush BuildRemoveBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 0));

		private EBlockType SelectedBlockType = EBlockType.Obstacle;
		private TextButton CancelBuildModeButton;
		private ImageButton SelectWallButton;
		private ImageButton SelectObstacleButton;
		private ImageButton SelectAmmunitionGeneratorButton;
		private ImageButton SelectResourceGeneratorButton;

		private int BuildModeInitialDelay = 0;

		#endregion

		#region System

		private bool IsMapGridChanged = true;
		public bool IsNeedBackRefresh = true; // 뒷 배경 재설정
		public bool IsNeedFrontRefresh = true; // 블럭 재설정

		#endregion

		#region

		public EGoDirection DebugShowGrid = EGoDirection.AllExceptWall;

		#endregion

		// 생성자
		public Map(GameManager gameManager)
		{
			this.Manager = gameManager;

			#region Load Map Sprites

			this.BlockImages = new Bitmap[8];

			for (int imageIndex = 0; imageIndex < BlockImages.Count(); imageIndex ++)
			{
				this.BlockImages[imageIndex] = new Bitmap(Image.FromFile("BlockType_" + imageIndex + ".png"));
			}

			#endregion

			#region Map Section Divide

			this.Width = 16;
			this.Height = 16;

			this.CenterX = (this.Width  / 2) * this.CellSize;
			this.CenterY = (this.Height / 2) * this.CellSize;

			int boundary = 1;
			int centerWidth  = 8;
			int centerHeight = 8;

			int distanceX = (Width  - centerWidth)  / 2 - boundary;
			int distanceY = (Height - centerHeight) / 2 - boundary;

			this.GridX1 = boundary;
			this.GridX2 = this.GridX1 + distanceX;
			this.GridX3 = this.GridX2 + centerWidth;
			this.GridX4 = Width - boundary;
			this.GridY1 = boundary;
			this.GridY2 = this.GridY1 + distanceY;
			this.GridY3 = this.GridY2 + centerHeight;
			this.GridY4 = Height - boundary;

			this.SpawnerX1 = this.GridX1;
			this.SpawnerX2 = this.GridX2;
			this.SpawnerX3 = this.GridX3 - 1;
			this.SpawnerX4 = this.GridX4 - 1;
			this.SpawnerY1 = this.GridY1;
			this.SpawnerY2 = this.GridY2;
			this.SpawnerY3 = this.GridY3 - 1;
			this.SpawnerY4 = this.GridY4 - 1;

			#endregion
			
			#region Initialize Map

			this.MapGrid  = new EBlockType[Height, Width];
			this.BlockHealthPoint = new int[Height, Width];
			this.HitDelay = new int[Height, Width];
			this.EnemySpawnerPoints = new List<Point>();
			this.ExpandTurn = new int[5];
			this.ExpandTurn[0] = 0;
			this.ExpandTurn[1] = 1;
			this.ExpandTurn[2] = 2;
			this.ExpandTurn[3] = 3;
			this.ExpandTurn[4] = 4;

			#endregion

			#region Initialize Path Finding

			// 벽을 제외한 코어를 향한다.
			this.PathToCore                   = new EDirection[this.Height, this.Width];
			this.StartFromCore                = new List<EBlockType>();
			this.PathToCoreException          = new List<EBlockType>();
			this.StartFromCore.Add(EBlockType.Core);
			this.PathToCoreException.Add(EBlockType.EmptySpace);
			this.PathToCoreException.Add(EBlockType.Wall);
			
			// 벽을 제외한 모든 구조물을 향한다.
			this.PathToAllExceptWall          = new EDirection[this.Height, this.Width];
			this.StartFromAllExceptWall       = new List<EBlockType>();
			this.PathToAllExceptWallException = new List<EBlockType>();
			this.StartFromAllExceptWall.Add(EBlockType.Core);
			this.StartFromAllExceptWall.Add(EBlockType.Obstacle);
			this.StartFromAllExceptWall.Add(EBlockType.AmmunitionGenerator);
			this.StartFromAllExceptWall.Add(EBlockType.ResourceGenerator);
			this.PathToAllExceptWallException.Add(EBlockType.EmptySpace);
			this.PathToAllExceptWallException.Add(EBlockType.Wall);
			
			// 모든 구조물을 향한다.
			this.PathToAll                    = new EDirection[this.Height, this.Width];
			this.StartFromAll                 = new List<EBlockType>();
			this.PathToAllException           = new List<EBlockType>();
			this.StartFromAll.Add(EBlockType.Core);
			this.StartFromAll.Add(EBlockType.Wall);
			this.StartFromAll.Add(EBlockType.Obstacle);
			this.StartFromAll.Add(EBlockType.AmmunitionGenerator);
			this.StartFromAll.Add(EBlockType.ResourceGenerator);
			this.PathToAllException.Add(EBlockType.EmptySpace);
			
			// 벽과 구조물을 제외한 공간의 플레이어를 향한다.
			this.PathToPlayer                 = new EDirection[this.Height, this.Width];
			this.PathToPlayerException        = new List<EBlockType>();
			this.PathToPlayerException.Add(EBlockType.EmptySpace);
			this.PathToPlayerException.Add(EBlockType.Wall);
			this.PathToPlayerException.Add(EBlockType.Core);
			this.PathToPlayerException.Add(EBlockType.Obstacle);
			this.PathToPlayerException.Add(EBlockType.AmmunitionGenerator);
			this.PathToPlayerException.Add(EBlockType.ResourceGenerator);
			
			// 벽을 제외한 구조물을 모두 무시하고 플레이어를 향한다.
			this.PathToPlayerIgnoreBlock      = new EDirection[this.Height, this.Width];
			this.PathToPlayerIgnoreBlockException = new List<EBlockType>();
			this.PathToPlayerIgnoreBlockException.Add(EBlockType.EmptySpace);
			this.PathToPlayerIgnoreBlockException.Add(EBlockType.Wall);
			this.PathToPlayerIgnoreBlockException.Add(EBlockType.Core);

			#endregion

			#region Initialize Build Mode GUI

			this.CancelBuildModeButton = new TextButton(gameManager, this.CancelBuildMode, true, 75, 75, 100, 100, "건설\n취소", GameFont.GAME_FONT, 30, EDock.Right, EDock.Bottom);

			int bWidth = 60;
			this.SelectWallButton = 
				new ImageButton(gameManager, this.SelectWall, false, 75, -150, bWidth, bWidth, this.BlockImages[(int)EBlockType.Wall], 0.5f, EDock.Right, EDock.Center);
			this.SelectObstacleButton = 
				new ImageButton(gameManager, this.SelectObstacle, false, 75, -50, bWidth, bWidth, this.BlockImages[(int)EBlockType.Obstacle], 0.5f, EDock.Right, EDock.Center);
			this.SelectAmmunitionGeneratorButton = 
				new ImageButton(gameManager, this.SelectAmmunitionGenerator, false, 75, 50, bWidth, bWidth, this.BlockImages[(int)EBlockType.AmmunitionGenerator], 0.5f, EDock.Right, EDock.Center);
			this.SelectResourceGeneratorButton = 
				new ImageButton(gameManager, this.SelectResourceGenerator, false, 75, 150, bWidth, bWidth, this.BlockImages[(int)EBlockType.ResourceGenerator], 0.5f, EDock.Right, EDock.Center);

			#endregion

			this.Reset();
		}

		public void Reset()
		{
			this.IsMapGridChanged = true;
			this.IsNeedBackRefresh = true; 
			this.IsNeedFrontRefresh = true;

			this.EnemySpawnerIndex = 0;
			this.WaveCheck = 0;
			this.IsFirstAmmunitionDelay = true;

			this.AmmunitionGeneratorCount = 0;
			this.ResourceGeneratorCount = 0;

			#region Scramble Expand Turn

			Random rand = new Random();
			
			for (int count = 0; count < 10; count ++)
			{
				int selection = rand.Next(3);

				int temp = this.ExpandTurn[selection];
				this.ExpandTurn[selection] = this.ExpandTurn[3];
				this.ExpandTurn[3] = temp;
			}

			#endregion

			#region Reset Map tiles

			// Initialize every map tiles to EmptySpace
			for (int y = 0; y < Height; y ++)
			{
				for (int x = 0; x < Width; x ++)
				{
					this.MapGrid[y, x] = EBlockType.EmptySpace;
					this.BlockHealthPoint[y, x] = -1;
					this.HitDelay[y, x] = 0;
				}
			}
			
			this.InitializePathGrid(this.PathToCore         , this.Width, this.Height);
			this.InitializePathGrid(this.PathToAll	        , this.Width, this.Height);
			this.InitializePathGrid(this.PathToAllExceptWall, this.Width, this.Height);
			this.InitializePathGrid(this.PathToPlayer       , this.Width, this.Height);
			this.InitializePathGrid(this.PathToPlayerIgnoreBlock, this.Width, this.Height);
			
			// Initialize initial play area, In the first level the size of area will be 8 X 8
			for (int y = this.GridY2; y < this.GridY3; y ++)
			{
				for (int x = this.GridX2; x < this.GridX3; x ++)
				{
					this.MapGrid[y, x] = EBlockType.None;
				}
			}
			
			// Set Core

			int centerPositionX = this.Width / 2;
			int centerPositionY = this.Height / 2;

			this.MapGrid[centerPositionY    , centerPositionX    ] = EBlockType.Core;
			this.MapGrid[centerPositionY    , centerPositionX - 1] = EBlockType.Core;
			this.MapGrid[centerPositionY - 1, centerPositionX    ] = EBlockType.Core;
			this.MapGrid[centerPositionY - 1, centerPositionX - 1] = EBlockType.Core;

			// Set EnemySpawner
			this.MapGrid[this.SpawnerY2, this.SpawnerX2] = EBlockType.EnemySpawner;
			this.MapGrid[this.SpawnerY2, this.SpawnerX3] = EBlockType.EnemySpawner;
			this.MapGrid[this.SpawnerY3, this.SpawnerX2] = EBlockType.EnemySpawner;
			this.MapGrid[this.SpawnerY3, this.SpawnerX3] = EBlockType.EnemySpawner;

			this.EnemySpawnerPoints.Clear();
			this.EnemySpawnerPoints.Add(new Point(this.SpawnerY2, this.SpawnerX2));
			this.EnemySpawnerPoints.Add(new Point(this.SpawnerY2, this.SpawnerX3));
			this.EnemySpawnerPoints.Add(new Point(this.SpawnerY3, this.SpawnerX2));
			this.EnemySpawnerPoints.Add(new Point(this.SpawnerY3, this.SpawnerX3));

			#endregion
			
			#region Data Set

			this.CoreAmmoCount					= DataTable.CoreAmmoCount;
			this.AmmunitionGeneratorAmmoCount	= DataTable.AmmunitionGeneratorAmmoCount;
			this.CoreAmmoGeneratorAmount		= DataTable.CoreAmmoGeneratorAmount;

			this.ResourceGeneratorMakeAmount	= DataTable.ResourceGeneratorMakeAmount;
			this.CoreResourceMakeAmount			= DataTable.CoreResourceMakeAmount;

			this.ResourceGeneratorHealthPoint	= DataTable.ResourceGeneratorHealthPoint;
			this.AmmunitionGeneratorHealthPoint	= DataTable.AmmunitionGeneratorHealthPoint;
			this.WallHealthPoint				= DataTable.WallHealthPoint;
			this.ObstacleHealthPoint			= DataTable.ObstacleHealthPoint;

			this.CoreHealthPointIni				= DataTable.CoreHealth[0, 0];
			this.CoreHealthPoint				= DataTable.CoreHealth[0, 0];

			#endregion

			LevelCoreHealth = 0;
			LevelCoreRegen = 0;
			LevelCoreAmmunitionMakeTime = 0;
			LevelCoreResourceRatio = 0;
			LevelCoreAmmoHaveRatio = 0;
			CoreRegenDelay = 0;

			this.SetCoreData();
		}

		public void SetCoreData()
		{
			CoreHealthPointIni          = DataTable.CoreHealth[0, LevelCoreHealth];
			CoreRegen                   = DataTable.CoreRegen[0, LevelCoreRegen];
			CoreResourceGenerateRatio   = DataTable.CoreGenerateResourceRatio[0, LevelCoreResourceRatio] / 10 + 1;
			CoreAmmoHaveRatio           = (DataTable.CoreAmmoHasRatio[0, LevelCoreAmmoHaveRatio] + 100) / 100f;

			CoreAmmunitionGenerateDelay = DataTable.CoreGenerateAmmunitionMakeTime[0, LevelCoreAmmunitionMakeTime] / 10;
			AmmunitionDelayIniLong		= (int)((CoreAmmunitionGenerateDelay * 60) / 2);
			AmmunitionDelayIniShort		= (int)((CoreAmmunitionGenerateDelay * 60) / 2);
		}

		public void Update(Point virtualMousePoint)
		{
			#region Add Ammunition

			ESystemSituation situation = this.Manager.GetGameSituation();

			if (situation == ESystemSituation.Wave)
			{
				if (this.AmmunitionDelay > 0)
				{
					this.AmmunitionDelay --;
				}
				else
				{
					// 총알 생성시간을 반으로 나눈다.
					// 생성할 총알이 짝수라면 총알 생성시간 동안 반으로 나눈 양을 두번 생성
					// 홀수라면 첫번째 생성에는 반으로 나눈 양을 생성, 두번째 생성에는 1을 더해서 생성한다.

					int ammoGenerateAmount = this.AmmunitionGeneratorCount + this.CoreAmmoGeneratorAmount;

					if (this.IsFirstAmmunitionDelay)
					{
						this.IsFirstAmmunitionDelay = false;
						ammoGenerateAmount /= 2;
					}
					else
					{
						this.IsFirstAmmunitionDelay = true;
						if (ammoGenerateAmount % 2 == 1)
						{
							ammoGenerateAmount /= 2;
							ammoGenerateAmount ++;
						}
						else
						{
							ammoGenerateAmount /= 2;
						}
					}

					this.Manager.AddAmmunitionCount(ammoGenerateAmount);
					this.AmmunitionDelay = this.AmmunitionDelayIniShort;
				}
			}
			else
			{
				if (this.Manager.GetGameSituation() != ESystemSituation.End)
				{
					this.Manager.AddAmmunitionCount(10);
				}
			}

			#endregion

			#region Regen

			if (situation == ESystemSituation.Wave)
			{
				if (CoreHealthPoint < CoreHealthPointIni - CoreRegen)
				{
					if (CoreRegenDelay <  CoreRegenDelayIni)
					{
						CoreRegenDelay ++;
					}
					else
					{
						CoreRegenDelay = 0;
						if (CoreHealthPoint < CoreHealthPointIni - CoreRegen)
						{
							CoreHealthPoint += CoreRegen;
						}
						else
						{
							CoreHealthPoint = CoreHealthPointIni;
						}
					}
				}
			}

			#endregion

			#region Update Path Grids

			if (this.IsMapGridChanged)
			{
				this.UpdatePath(this.PathToCore         , Width, Height, StartFromCore         , PathToCoreException);
				this.UpdatePath(this.PathToAll          , Width, Height, StartFromAll          , PathToAllException);
				this.UpdatePath(this.PathToAllExceptWall, Width, Height, StartFromAllExceptWall, PathToAllExceptWallException);
				this.IsMapGridChanged = false;
			}

			float playerX = this.Manager.GamePlayer.GetX();
			float playerY = this.Manager.GamePlayer.GetY();

			this.UpdatePath(this.PathToPlayer, Width, Height, playerX, playerY, PathToPlayerException);
			this.UpdatePath(this.PathToPlayerIgnoreBlock, Width, Height, playerX, playerY, PathToPlayerIgnoreBlockException);

			#endregion

			#region Wave detection to expand the map

			int wave = this.Manager.GetWave();

			// 웨이브가 바뀌면
			if (this.WaveCheck != wave)
			{
				this.WaveCheck = wave;

				this.AmmunitionDelay = this.AmmunitionDelayIniShort;

				this.Manager.AddRecourceCount((int)(((ResourceGeneratorCount * ResourceGeneratorMakeAmount) + CoreResourceMakeAmount) * CoreResourceGenerateRatio));

				this.MapExpand(this.Manager.GetWave());

				// 건물 체력 모두 초기화

				for (int y = 0; y < this.Height; y ++)
				{
					for (int x = 0; x < this.Width; x ++)
					{
						int health = -1;

						switch(this.MapGrid[y, x])
						{
							case EBlockType.Wall:
								health = this.WallHealthPoint;
								break;
								
							case EBlockType.ResourceGenerator:
								health = this.ResourceGeneratorHealthPoint;
								break;

							case EBlockType.AmmunitionGenerator:
								health = this.AmmunitionGeneratorHealthPoint;
								break;
								
							case EBlockType.Obstacle:
								health = this.ObstacleHealthPoint;
								break;
						}

						this.BlockHealthPoint[y, x] = health;
					}
				}
			}

			#endregion

			#region Build Mode

			if (this.Manager.GetGameSituation() == ESystemSituation.Build)
			{
				this.CancelBuildModeButton.Update(virtualMousePoint);
				this.SelectWallButton.Update(virtualMousePoint);
				this.SelectObstacleButton.Update(virtualMousePoint);
				this.SelectAmmunitionGeneratorButton.Update(virtualMousePoint);
				this.SelectResourceGeneratorButton.Update(virtualMousePoint);
			}

			#endregion
		}

		public void Draw(Graphics graphics)
		{
			if (this.IsHit)
			{
				this.IsHit = false;

				int cameraX = this.Manager.GameCamera.GetX();
				int cameraY = this.Manager.GameCamera.GetY();

				for (int y = 0; y < this.Height; y ++)
				{
					for (int x = 0; x < this.Width; x ++)
					{
						if (this.HitDelay[y, x] > 0)
						{
							this.HitDelay[y, x] --;
							this.IsHit = true;

							graphics.FillRectangle(this.HitBrush, this.CellSize * x - cameraX, this.CellSize * y - cameraY, CellSize, CellSize);
						}
					}
				}
			}
		}
		
		public void DrawAndUpdateBuildMode(Graphics graphics)
		{
			if (this.BuildModeInitialDelay > 0)
			{
				this.BuildModeInitialDelay --;
				return;
			}

			if (this.Manager.GetGameSituation() == ESystemSituation.Build)
			{
				// 건설 가격 초기화
				int waveCount = (this.Manager.GetWave() - 1);

				PriceWall					= DataTable.PriceWall + waveCount * 3;
				PriceObstacle				= DataTable.PriceObstacle + waveCount * 2;
				PriceAmmunitionGenerator	= DataTable.PriceAmmunitionGenerator + waveCount * 3;;
				PriceResourceGenerator		= DataTable.PriceResourceGenerator + waveCount * 3;;


				bool isMouseOnButton = (SelectWallButton.IsMouseOnButton() ||
					                    SelectObstacleButton.IsMouseOnButton() ||
										SelectAmmunitionGeneratorButton.IsMouseOnButton() ||
										SelectResourceGeneratorButton.IsMouseOnButton());

				#region Draw Block Build Position

				if (!isMouseOnButton)
				{
					int cameraX = this.Manager.GameCamera.GetX();
					int cameraY = this.Manager.GameCamera.GetY();
					Point mousePosition = this.Manager.GetIngameMousePosition();
				
					int cellX = (mousePosition.X) / this.CellSize;
					int cellY = (mousePosition.Y) / this.CellSize;

					EBlockType blockType = this.GetPointBlockType(cellX, cellY);

					Rectangle drawRectangle = new Rectangle(cellX * this.CellSize - cameraX, cellY * this.CellSize - cameraY, this.CellSize, this.CellSize);
					
					ColorMatrix cm = new ColorMatrix();
					cm.Matrix33 = 0.7f;
					ImageAttributes imageTransparency = new ImageAttributes();
					imageTransparency.SetColorMatrix(cm);

					graphics.DrawImage(BlockImages[(int)this.SelectedBlockType], drawRectangle, 0, 0, this.CellSize, this.CellSize, GraphicsUnit.Pixel, imageTransparency);

					bool canRemove = false;
					bool canPlace = false;

					switch(blockType)
					{
						case EBlockType.EmptySpace:
						case EBlockType.Core:
						case EBlockType.EnemySpawner:
							graphics.FillRectangle(BuildImpossibleBrush, drawRectangle);
							break;
							
						case EBlockType.None:
							graphics.FillRectangle(BuildPossibleBrush, drawRectangle);
							canPlace = true;
							break;
							
						case EBlockType.Wall:
						case EBlockType.ResourceGenerator:
						case EBlockType.AmmunitionGenerator:
						case EBlockType.Obstacle:
							graphics.FillRectangle(BuildRemoveBrush, drawRectangle);
							canRemove = true;
							break;
					}

					bool isNeedCheckPlayerPosition = false;
					
					if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Left] > 0)
					{
						if (canRemove)
						{
							if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Left] == 1)
							{
								//this.Manager.GameGUI.AddNotification(EMessageType.Normal, "블럭을 먼저 제거하세요.");
							}
						}

						if (!canPlace && !canRemove)
						{
							if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Left] == 1)
							{
								this.Manager.GameGUI.AddNotification(EMessageType.Normal, "여기에 블럭을 설치할 수 없습니다.");
							}
						}

						if (canPlace)
						{
							int resourceNeed = 0;
							
							switch(this.SelectedBlockType)
							{
								case EBlockType.Wall:
									resourceNeed = PriceWall;
									isNeedCheckPlayerPosition = true;
									break;

								case EBlockType.AmmunitionGenerator:
									resourceNeed = PriceAmmunitionGenerator;
									break;
									
								case EBlockType.ResourceGenerator:
									resourceNeed = PriceResourceGenerator;
									break;
									
								case EBlockType.Obstacle:
									isNeedCheckPlayerPosition = true;
									resourceNeed = PriceObstacle;
									break;
							}

							if (this.Manager.ConsumResource(resourceNeed))
							{
								if (this.SetPosition(cellX, cellY, this.SelectedBlockType))
								{
									this.Manager.GameGUI.AddNotification(EMessageType.Normal, "블럭 설치 완료");
								}
								else
								{
									this.Manager.GameGUI.AddNotification(EMessageType.Error, "Map.SetPosition() Error!");
								}
							}

						}
					}

					// If Player Detected Block Place Position, then Reset Player Position To The Center Of Map.
					if (isNeedCheckPlayerPosition)
					{
						int playerX = (int)this.Manager.GamePlayer.GetX();
						int playerY = (int)this.Manager.GamePlayer.GetY();

						int playerHalfHeight = this.Manager.GamePlayer.GetHeight() / 2;
						int playerHalfWidth  = this.Manager.GamePlayer.GetWidth()  / 2;

						bool isCollideLeft   = (((playerX - playerHalfWidth ) / this.CellSize) == cellX);
						bool isCollideRight  = (((playerX + playerHalfWidth ) / this.CellSize) == cellX);
						bool isCollideTop    = (((playerY - playerHalfHeight) / this.CellSize) == cellY);
						bool isCollideBottom = (((playerY + playerHalfHeight) / this.CellSize) == cellY);

						if ((isCollideRight && isCollideTop   ) || (isCollideLeft && isCollideTop   ) ||
							(isCollideRight && isCollideBottom) || (isCollideLeft && isCollideBottom))
						{
							this.Manager.GamePlayer.SetPlayerPosition(this.CenterX, this.CenterY);
						}
					}
					
					if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Right] > 0)
					{
						if (canRemove)
						{
							EBlockType deletedBlockType = this.MapGrid[cellY, cellX];

							switch(deletedBlockType)
							{
								case EBlockType.Wall:
									this.Manager.AddRecourceCount(PriceWall);
									break;

								case EBlockType.AmmunitionGenerator:
									this.Manager.AddRecourceCount(PriceAmmunitionGenerator);
									break;
									
								case EBlockType.ResourceGenerator:
									this.Manager.AddRecourceCount(PriceResourceGenerator);
									break;
									
								case EBlockType.Obstacle:
									this.Manager.AddRecourceCount(PriceObstacle);
									break;
							}
							
							if (this.SetPosition(cellX, cellY, EBlockType.None))
							{
								this.Manager.GameGUI.AddNotification(EMessageType.Normal, "블럭 제거 성공");
							}
							else
							{
								this.Manager.GameGUI.AddNotification(EMessageType.Error, "Map.SetPosition() Error!");
							}
						}
						
						if (!canPlace && !canRemove)
						{
							if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Right] == 1)
							{
								this.Manager.GameGUI.AddNotification(EMessageType.Normal, "이 블럭은 제거할 수 없습니다.");
							}
						}
					}
				}

				#endregion

				this.CancelBuildModeButton.Draw(graphics);
				this.SelectWallButton.Draw(graphics);
				this.SelectObstacleButton.Draw(graphics);
				this.SelectAmmunitionGeneratorButton.Draw(graphics);
				this.SelectResourceGeneratorButton.Draw(graphics);
			}
		}

		public void Attacked(float attackedPositionX, float attackedPositionY, int damage)
		{
			Point cellPoint = this.GetCellPosition(attackedPositionX, attackedPositionY);

			this.HitDelay[cellPoint.Y, cellPoint.X] = this.HitDelayIni;
			this.IsHit = true;

			if (this.MapGrid[cellPoint.Y, cellPoint.X] == EBlockType.Core)
			{
				if (this.CoreHealthPoint > damage)
				{
					this.CoreHealthPoint -= damage;
				}
				else
				{
					// 코어 터짐
					this.CoreHealthPoint = 0;

					for (int y = 0; y < this.Height; y ++)
					{
						for (int x = 0; x < this.Width; x ++)
						{
							if (this.MapGrid[y, x] == EBlockType.Core)
							{
								int halfCellSize = this.CellSize / 2;
								int effectX = (cellPoint.X * this.CellSize) + halfCellSize;
								int effectY = (cellPoint.Y * this.CellSize) + halfCellSize;
								this.Manager.CreateDeadEffect(effectX, effectY, 12, true);
								this.SetPosition(cellPoint.X, cellPoint.Y, EBlockType.None);
							}
						}
					}
				}
			}
			else
			{
				if (this.BlockHealthPoint[cellPoint.Y, cellPoint.X] > damage)
				{
					this.BlockHealthPoint[cellPoint.Y, cellPoint.X] -= damage;
				}
				else if (this.BlockHealthPoint[cellPoint.Y, cellPoint.X] > 0)
				{
					int halfCellSize = this.CellSize / 2;
					int effectX = (cellPoint.X * this.CellSize) + halfCellSize;
					int effectY = (cellPoint.Y * this.CellSize) + halfCellSize;
					this.Manager.CreateDeadEffect(effectX, effectY, 12, true);
					this.SetPosition(cellPoint.X, cellPoint.Y, EBlockType.None);
				}
			}
		}

		#region Getter Setter

		public bool IsCoreDestroyed()
		{
			if (this.CoreHealthPoint <= 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsAttackable(float x, float y, EGoDirection goDirection)
		{
			Point point = GetCellPosition(x, y);
			
			EBlockType type = this.MapGrid[point.Y, point.X];

			if (type == EBlockType.Core)
			{
				return true;
			}

			if (this.BlockHealthPoint[point.Y, point.X] > 0)
			{

				switch (goDirection)
				{
					case EGoDirection.All:
						return !this.PathToAllException.Contains(type);

					case EGoDirection.AllExceptWall:
						return !this.PathToAllExceptWallException.Contains(type);

					case EGoDirection.Core:
						return !this.PathToCoreException.Contains(type);

					case EGoDirection.Player:
					case EGoDirection.PlayerIgnoreBlock:
						return (type != EBlockType.Wall);

					default:
						return true;
				}
			}
			else
			{
				return false;
			}
		}

		public int GetMaxAmmunition()
		{
			return (int)((CoreAmmoCount + (AmmunitionGeneratorCount * AmmunitionGeneratorAmmoCount)) * CoreAmmoHaveRatio);;
		}

		public bool IsOverPoint(float x, float y)
		{
			if (x < 0 || x > this.CellSize * this.Width || y < 0 || y > this.CellSize * this.Height)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public Point GetEnemySpawnPoint()
		{
			if (this.EnemySpawnerIndex < this.EnemySpawnerPoints.Count - 1)
			{
				this.EnemySpawnerIndex ++;
			}
			else
			{
				this.EnemySpawnerIndex = 0;
			}

			Point cellPoint = this.EnemySpawnerPoints[this.EnemySpawnerIndex];

			return new Point(cellPoint.X * this.CellSize + this.CellSize / 2, cellPoint.Y * this.CellSize + this.CellSize / 2);
		}

		public EDirection GetPathDirection(int cellX, int cellY, EGoDirection goDirection)
		{
			switch(goDirection)
			{
				case EGoDirection.Core:
					return this.PathToCore[cellY, cellX];

				case EGoDirection.All:
					return this.PathToAll[cellY, cellX];

				case EGoDirection.AllExceptWall:
					return this.PathToAllExceptWall[cellY, cellX];

				case EGoDirection.Player:
					return this.PathToPlayer[cellY, cellX];
					
				case EGoDirection.PlayerIgnoreBlock:
					return this.PathToPlayerIgnoreBlock[cellY, cellX];

				default:
					return EDirection.None;
			}
		}

		public Point GetPathDestination(float x, float y, EGoDirection goDirection)
		{
			int cellX = (int)(x / this.CellSize);
			int cellY = (int)(y / this.CellSize);

			if (cellX < 0 || cellX >= this.Width || cellY < 0 || cellY >= this.Height)
			{
				return new Point(this.CenterX, this.CenterY);
			}

			Point destination;

			switch(this.GetPathDirection(cellX, cellY, goDirection))
			{
				case EDirection.Right:
					destination = new Point(cellX + 1, cellY);
					break;
					
				case EDirection.Left:
					destination = new Point(cellX - 1, cellY);
					break;
					
				case EDirection.Up:
					destination = new Point(cellX, cellY - 1);
					break;
					
				case EDirection.Down:
					destination = new Point(cellX, cellY + 1);
					break;

				case EDirection.UpRight:
					destination = new Point(cellX + 1, cellY - 1);
					break;

				case EDirection.UpLeft:
					destination = new Point(cellX - 1, cellY - 1);
					break;

				case EDirection.DownRight:
					destination = new Point(cellX + 1, cellY + 1);
					break;

				case EDirection.DownLeft:
					destination = new Point(cellX - 1, cellY + 1);
					break;
				
				// Lost the path
				case EDirection.None:
					return new Point(this.CenterX, this.CenterY);

				case EDirection.Destination:
					return new Point(this.CenterX, this.CenterY);

				default:
					if (this.Manager.MainForm.IsDebugging)
					{
						this.Manager.GameGUI.AddNotification(EMessageType.Error, "Enemy.GetPathDestination Error !");
					}
					return new Point(this.CenterX, this.CenterY);
			}

			destination.X *= this.CellSize;
			destination.Y *= this.CellSize;
			destination.X += this.CellSize / 2;
			destination.Y += this.CellSize / 2;

			return destination;
		}

		public bool SetPosition(int cellX, int cellY, EBlockType blockType)
		{
			if (cellX < 0 || cellX > this.Width || cellY < 0 || cellY > this.Height)
			{
				return false;
			}

			if (this.Manager.MainForm.IsDebugging)
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "MapGrid Changed");
			}

			// 블럭이 삭제될때
			if (blockType == EBlockType.None)
			{
				EBlockType deleteType = this.MapGrid[cellY, cellX];

				switch(deleteType)
				{
					case EBlockType.AmmunitionGenerator:
						this.AmmunitionGeneratorCount --;
						break;

					case EBlockType.ResourceGenerator:
						this.ResourceGeneratorCount --;
						break;
				}

				this.BlockHealthPoint[cellY, cellX] = -1;
			}
			else
			{
				// 블럭이 추가될 때
				switch(blockType)
				{
					case EBlockType.AmmunitionGenerator:
						this.AmmunitionGeneratorCount ++;
						this.BlockHealthPoint[cellY, cellX] = this.AmmunitionGeneratorHealthPoint;
						break;

					case EBlockType.ResourceGenerator:
						this.ResourceGeneratorCount ++;
						this.BlockHealthPoint[cellY, cellX] = this.ResourceGeneratorHealthPoint;
						break;
						
					case EBlockType.Obstacle:
						this.BlockHealthPoint[cellY, cellX] = this.ObstacleHealthPoint;
						break;
						
					case EBlockType.Wall:
						this.BlockHealthPoint[cellY, cellX] = this.WallHealthPoint;
						break;
				}
			}

			this.MapGrid[cellY, cellX] = blockType;
			this.IsNeedFrontRefresh = true;
			this.IsMapGridChanged = true;
			return true;
		}

		public int GetCenterX()
		{
			return this.CenterX;
		}

		public int GetCenterY()
		{
			return this.CenterY;
		}

		public void SetBuildModeInitaiDelay()
		{
			this.BuildModeInitialDelay = 1;
		}

		public float GetCoreHealthRatio()
		{
			if (CoreHealthPointIni != 0)
			{
				return (float)CoreHealthPoint / CoreHealthPointIni;
			}

			return 0f;
		}

		#endregion

		#region Path Finding

		public void InitializePathGrid(EDirection[,] pathGrid, int width, int height)
		{
			for (int y = 0; y < height; y ++)
			{
				for (int x = 0; x < width; x ++)
				{
					pathGrid[y, x] = EDirection.None;
				}
			}
		}

		/// <summary>
		/// 경로 업데이트
		/// </summary>
		/// <param name="pathGrid">대입할 Direction 배열</param>
		/// <param name="width">맵 너비</param>
		/// <param name="height">맵 높이</param>
		/// <param name="startTypes">순회 시작 블럭 / 경로 도착 블럭</param>
		/// <param name="exceptTypes">장애물 블럭</param>
		public void UpdatePath(EDirection[,] pathGrid, int width, int height, List<EBlockType> startTypes, List<EBlockType> exceptTypes)
		{
			// Initialize every map Direction to None
			this.InitializePathGrid(pathGrid, width, height);

			Queue<Point> traversalQueue = new Queue<Point>();
			List<Point> startPositions = this.GetComponentPositions(startTypes);

			foreach(Point p in startPositions)
			{
				pathGrid[p.Y, p.X] = EDirection.Destination;
				traversalQueue.Enqueue(p);
			}
			
			this.SetPath(pathGrid, exceptTypes, traversalQueue);
		}

		/// <summary>
		/// 좌표 기준 경로 업데이트
		/// </summary>
		/// <param name="pathGrid">대입할 Direction 배열</param>
		/// <param name="width">맵 너비</param>
		/// <param name="height">맵 높이</param>
		/// <param name="destinationX">순회 시작 지점 X</param>
		/// <param name="destinationY">순회 시작 지점 Y</param>
		/// <param name="exceptTypes">장애물 블럭</param>
		public void UpdatePath(EDirection[,] pathGrid, int width, int height, float destinationX, float destinationY, List<EBlockType> exceptTypes)
		{
			// Initialize every map Direction to None
			this.InitializePathGrid(pathGrid, width, height);

			// 맵 밖을 벗어나면 종료
			if (destinationX < 0 || destinationX > this.Width * this.CellSize ||
				destinationY < 0 || destinationY > this.Height * this.CellSize)
			{
				return;
			}

			// 플레이어가 예외 건물에 올라가 있다면 종료
			Point playerPosition = this.GetCellPosition(destinationX, destinationY);

			EBlockType playerCurrentPositionBlock = this.GetPointBlockType(playerPosition.X, playerPosition.Y);
			if (exceptTypes.Contains(playerCurrentPositionBlock))
			{
				return;
			}

			pathGrid[playerPosition.Y, playerPosition.X] = EDirection.Destination;

			Queue<Point> traversalQueue = new Queue<Point>();

			traversalQueue.Enqueue(playerPosition);

			this.SetPath(pathGrid, exceptTypes, traversalQueue);
		}

		/// <summary>
		/// 경로 찾기
		/// </summary>
		/// <param name="pathGrid">대입할 Direction 배열</param>
		/// <param name="exceptTypes">장애물 블럭</param>
		/// <param name="traversalQueue">순회 Queue</param>
		public void SetPath(EDirection[,] pathGrid, List<EBlockType> exceptTypes, Queue<Point> traversalQueue)
		{
			/*
			 * 한 사이클에서 동일한 지점을 수직수평 방향과 대각선 방향을 분리해 경로를 설정한다.
			 * 수직 수평 방향을 먼저 계산한 후 동일한 위치를 기준으로 대각선 방향 경로를 계산한다.
			 */

			Queue<Point> traversalBufferQueue1 = new Queue<Point>();
			Queue<Point> traversalBufferQueue2 = new Queue<Point>();

			while(traversalQueue.Count != 0)
			{
				int copyCount = traversalQueue.Count();

				for (int copy = 0; copy < copyCount; copy ++)
				{
					Point copyPoint = traversalQueue.Dequeue();
					traversalBufferQueue1.Enqueue(copyPoint);
					traversalBufferQueue2.Enqueue(copyPoint);
				}

				while(traversalBufferQueue1.Count != 0)
				{
					Point basePoint = traversalBufferQueue1.Dequeue();
					Point pointR = new Point(basePoint.X + 1, basePoint.Y);
					Point pointL = new Point(basePoint.X - 1, basePoint.Y);
					Point pointD = new Point(basePoint.X, basePoint.Y + 1);
					Point pointU = new Point(basePoint.X, basePoint.Y - 1);
				
					// Right to Left
					if ((pathGrid[pointR.Y, pointR.X] == EDirection.None) && (!exceptTypes.Contains(this.MapGrid[pointR.Y, pointR.X])))
					{
						pathGrid[pointR.Y, pointR.X] = EDirection.Left;
						traversalQueue.Enqueue(pointR);
					}
				
					// Left to Right
					if ((pathGrid[pointL.Y, pointL.X] == EDirection.None) && (!exceptTypes.Contains(this.MapGrid[pointL.Y, pointL.X])))
					{
						pathGrid[pointL.Y, pointL.X] = EDirection.Right;
						traversalQueue.Enqueue(pointL);
					}
				
					// Down to Up
					if ((pathGrid[pointD.Y, pointD.X] == EDirection.None) && (!exceptTypes.Contains(this.MapGrid[pointD.Y, pointD.X])))
					{
						pathGrid[pointD.Y, pointD.X] = EDirection.Up;
						traversalQueue.Enqueue(pointD);
					}
				
					// Up to Down
					if ((pathGrid[pointU.Y, pointU.X] == EDirection.None) && (!exceptTypes.Contains(this.MapGrid[pointU.Y, pointU.X])))
					{
						pathGrid[pointU.Y, pointU.X] = EDirection.Down;
						traversalQueue.Enqueue(pointU);
					}
				}

				while(traversalBufferQueue2.Count != 0)
				{
					Point basePoint = traversalBufferQueue2.Dequeue();
					  
					Point pointR = new Point(basePoint.X + 1, basePoint.Y);
					Point pointL = new Point(basePoint.X - 1, basePoint.Y);
					Point pointD = new Point(basePoint.X, basePoint.Y + 1);
					Point pointU = new Point(basePoint.X, basePoint.Y - 1);
					Point pointUR = new Point(basePoint.X + 1, basePoint.Y - 1);
					Point pointUL = new Point(basePoint.X - 1, basePoint.Y - 1);
					Point pointDR = new Point(basePoint.X + 1, basePoint.Y + 1);
					Point pointDL = new Point(basePoint.X - 1, basePoint.Y + 1);

					// UR to DL
					if ((pathGrid[pointUR.Y, pointUR.X] == EDirection.None) && 
						(!exceptTypes.Contains(this.MapGrid[pointUR.Y, pointUR.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointR.Y, pointR.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointU.Y, pointU.X])))
					{
						pathGrid[pointUR.Y, pointUR.X] = EDirection.DownLeft;
						traversalQueue.Enqueue(pointUR);
					}
				
					// UL to DR
					if ((pathGrid[pointUL.Y, pointUL.X] == EDirection.None) && 
						(!exceptTypes.Contains(this.MapGrid[pointUL.Y, pointUL.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointL.Y, pointL.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointU.Y, pointU.X])))
					{
						pathGrid[pointUL.Y, pointUL.X] = EDirection.DownRight;
						traversalQueue.Enqueue(pointUL);
					}
				
					// DR to UL
					if ((pathGrid[pointDR.Y, pointDR.X] == EDirection.None) && 
						(!exceptTypes.Contains(this.MapGrid[pointDR.Y, pointDR.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointR.Y, pointR.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointD.Y, pointD.X])))
					{
						pathGrid[pointDR.Y, pointDR.X] = EDirection.UpLeft;
						traversalQueue.Enqueue(pointDR);
					}
				
					// DL to UR
					if ((pathGrid[pointDL.Y, pointDL.X] == EDirection.None) && 
						(!exceptTypes.Contains(this.MapGrid[pointDL.Y, pointDL.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointL.Y, pointL.X])) && 
						(!exceptTypes.Contains(this.MapGrid[pointD.Y, pointD.X])))
					{
						pathGrid[pointDL.Y, pointDL.X] = EDirection.UpRight;
						traversalQueue.Enqueue(pointDL);
					}
				}
			}
		}

		private List<Point> GetComponentPositions(List<EBlockType> blockTypes)
		{
			List<Point> pointList = new List<Point>();

			for (int x = 0; x < this.Width; x ++)
			{
				for (int y = 0; y < this.Height; y ++)
				{
					if (blockTypes.Contains(this.MapGrid[y, x]))
					{
						pointList.Add(new Point(x, y));
					}
				}
			}

			return pointList;
		}

		#endregion

		#region Button Actions

		private void CancelBuildMode()
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Build)
			{
				this.Manager.SetGameSituation(ESystemSituation.Standby);
			}
		}
		
		private void SelectWall()
		{
			if (this.SelectedBlockType != EBlockType.Wall)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("[가격 : ");
				sb.Append(PriceWall);
				sb.Append("] 벽 블럭");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, sb.ToString());
			}
			
			this.SelectedBlockType = EBlockType.Wall;
		}

		private void SelectObstacle()
		{
			if (this.SelectedBlockType != EBlockType.Obstacle)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("[가격 : ");
				sb.Append(PriceObstacle);
				sb.Append("] 장애물 블럭");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, sb.ToString());
			}

			this.SelectedBlockType = EBlockType.Obstacle;
		}

		private void SelectAmmunitionGenerator()
		{
			if (this.SelectedBlockType != EBlockType.AmmunitionGenerator)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("[가격 : ");
				sb.Append(PriceAmmunitionGenerator);
				sb.Append("] 탄약생성기 블럭");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, sb.ToString());
			}

			this.SelectedBlockType = EBlockType.AmmunitionGenerator;
		}

		private void SelectResourceGenerator()
		{
			if (this.SelectedBlockType != EBlockType.ResourceGenerator)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("[가격 : ");
				sb.Append(PriceResourceGenerator);
				sb.Append("] 자원생성기 블럭");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, sb.ToString());
			}

			this.SelectedBlockType = EBlockType.ResourceGenerator;
		}

		#endregion

		#region Map Expand

		public void RemoveEnemySpawner(int x, int y)
		{
			Point spawner = new Point(x, y);

			if (this.EnemySpawnerPoints.Remove(spawner))
			{
				this.MapGrid[spawner.Y, spawner.X] = EBlockType.None;
			}
		}

		public void AddEnemySpawner(int x, int y)
		{
			Point spawner = new Point(x, y);

			if (!this.EnemySpawnerPoints.Contains(spawner))
			{
				this.EnemySpawnerPoints.Add(spawner);
				this.MapGrid[spawner.Y, spawner.X] = EBlockType.EnemySpawner;
			}
		}

		/// <summary>
		/// 웨이브를 순서대로 적용하지 않으면 제대로 적용되지 않는다.
		/// </summary>
		/// <param name="wave"></param>
		public void MapExpand(int wave)
		{
			if (wave > 25)
			{
				return;
			}

			bool isNeedExpand = ((wave % 5) == 0);

			if (!isNeedExpand)
			{
				return;
			}
			
			this.IsMapGridChanged = true;
			this.IsNeedBackRefresh = true;
			this.IsNeedFrontRefresh = true;
			int expandedCount = wave / 5;

			StringBuilder sb = new StringBuilder();
			sb.Append("Map expand [Direction = ");
			sb.Append(expandedCount);
			sb.Append("]");
			this.Manager.GameGUI.AddNotification(EMessageType.System, sb.ToString());

			switch(ExpandTurn[expandedCount - 1])
			{
				case 0:
					// Create a new area on the right
					for(int y = this.GridY2; y < this.GridY3; y ++)
					{
						for(int x = this.GridX3; x < this.GridX4; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					}

					this.RemoveEnemySpawner(this.SpawnerX3, this.SpawnerY2);
					this.RemoveEnemySpawner(this.SpawnerX3, this.SpawnerY3);
					this.AddEnemySpawner(this.SpawnerX4, this.SpawnerY2);
					this.AddEnemySpawner(this.SpawnerX4, this.SpawnerY3);
					break;

				case 1:
					// Create a new area on top
					for(int y = this.GridY1; y < this.GridY2; y ++)
					{
						for(int x = this.GridX2; x < this.GridX3; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					}

					this.RemoveEnemySpawner(this.SpawnerX2, this.SpawnerY2);
					this.RemoveEnemySpawner(this.SpawnerX3, this.SpawnerY2);
					this.AddEnemySpawner(this.SpawnerX2, this.SpawnerY1);
					this.AddEnemySpawner(this.SpawnerX3, this.SpawnerY1);
					break;
					
				case 2:
					// Create a new area on the left
					for(int y = this.GridY2; y < this.GridY3; y ++)
					{
						for(int x = this.GridX1; x < this.GridX2; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					}

					this.RemoveEnemySpawner(this.SpawnerX2, this.SpawnerY2);
					this.RemoveEnemySpawner(this.SpawnerX2, this.SpawnerY3);
					this.AddEnemySpawner(this.SpawnerX1, this.SpawnerY2);
					this.AddEnemySpawner(this.SpawnerX1, this.SpawnerY3);
					break;

				case 3:
					// Create a new area at the bottom
					for(int y = this.GridY3; y < this.GridY4; y ++)
					{
						for(int x = this.GridX2; x < this.GridX3; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					}

					this.RemoveEnemySpawner(this.SpawnerX2, this.SpawnerY3);
					this.RemoveEnemySpawner(this.SpawnerX3, this.SpawnerY3);
					this.AddEnemySpawner(this.SpawnerX2, this.SpawnerY4);
					this.AddEnemySpawner(this.SpawnerX3, this.SpawnerY4);
					break;

				case 4:
					// Create a new area on the left top and right top corners
					for(int y = this.GridY1; y < this.GridY2; y ++)
					{
						for(int x = this.GridX1; x < this.GridX2; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					
						for(int x = this.GridX3; x < this.GridX4; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					}

					// Create a new area on the left bottom and right bottom corners
					for(int y = this.GridY3; y < this.GridY4; y ++)
					{
						for(int x = this.GridX1; x < this.GridX2; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					
						for(int x = this.GridX3; x < this.GridX4; x ++)
						{
							this.MapGrid[y, x] = EBlockType.None;
						}
					}

					this.AddEnemySpawner(this.SpawnerX1, this.SpawnerY1);
					this.AddEnemySpawner(this.SpawnerX4, this.SpawnerY1);
					this.AddEnemySpawner(this.SpawnerX1, this.SpawnerY4);
					this.AddEnemySpawner(this.SpawnerX4, this.SpawnerY4);
					break;
			}

		}

		#endregion

		#region Collision Detection

		public Point GetCellPosition(float x, float y)
		{
			return new Point((int)x / this.CellSize, (int)y / this.CellSize);
		}
		
		public Point GetCellPosition(int x, int y)
		{
			return new Point(x / this.CellSize, y / this.CellSize);
		}

		public bool GetCellPointCollision(int cellX, int cellY, bool isPlayer)
		{
			if (cellX < 0 || cellX >= Width || cellY < 0 || cellY >= Height)
			{
				return true;
			}
			else
			{
				EBlockType type = MapGrid[cellY, cellX];

				if (isPlayer)
				{
					if (type == EBlockType.EmptySpace || type == EBlockType.Obstacle || type == EBlockType.Wall)
					{
						return true;
					}
				}
				else
				{
					if (type == EBlockType.EmptySpace || type == EBlockType.Obstacle || type == EBlockType.Wall ||
						type == EBlockType.AmmunitionGenerator || type == EBlockType.ResourceGenerator || type == EBlockType.Core)
					{
						return true;
					}
				}

				return false;
			}
		}
		
		public EBlockType GetPointBlockType(int cellX, int cellY)
		{
			if (cellX < 0 || cellX >= Width || cellY < 0 || cellY >= Height)
			{
				return EBlockType.EmptySpace;
			}
			else
			{
				return MapGrid[cellY, cellX];
			}
		}

		#endregion

		#region Debug

		public void DrawDebugInfo(Graphics graphics)
		{
			int cameraX = this.Manager.GameCamera.GetX();
			int cameraY = this.Manager.GameCamera.GetY();
			Font textFont = new Font(GameFont.FONT_DEBUG, 10);
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;

			Pen outline = new Pen(Color.Blue, 1);

			EDirection[,] pathGrid;

			switch(this.DebugShowGrid)
			{
				case EGoDirection.All:
					pathGrid = this.PathToAll;
					break;
					
				case EGoDirection.AllExceptWall:
					pathGrid = this.PathToAllExceptWall;
					break;
					
				case EGoDirection.Core:
					pathGrid = this.PathToCore;
					break;
					
				case EGoDirection.Player:
					pathGrid = this.PathToPlayer;
					break;
					
				case EGoDirection.PlayerIgnoreBlock:
					pathGrid = this.PathToPlayerIgnoreBlock;
					break;

				default:
					pathGrid = this.PathToAll;
					break;
			}

			for (int y = 0; y < Height; y ++)
			{
				for (int x = 0; x < Width; x ++)
				{
					graphics.DrawRectangle(outline, x * CellSize - cameraX, y * CellSize - cameraY, CellSize, CellSize);

					graphics.DrawString(pathGrid[y, x].ToString(), textFont, Brushes.Blue, x * CellSize + (CellSize / 2) - cameraX, y * CellSize + (CellSize / 2) - cameraY, stringFormat);
					graphics.DrawString(this.BlockHealthPoint[y, x].ToString(), textFont, Brushes.Blue, x * CellSize + (CellSize / 2) - cameraX, y * CellSize + (CellSize / 2) - cameraY + 15, stringFormat);
				}
			}

			textFont.Dispose();
			stringFormat.Dispose();
			outline.Dispose();
		}

		#endregion

		#region Get Map Bitmaps

		public Bitmap GetMapBitmap()
		{
			Bitmap mapBitmap = new Bitmap(Width * CellSize + 1, Height * CellSize + 1);
			Graphics mapGraphics = Graphics.FromImage(mapBitmap);

			for (int y = 0; y < Height; y ++)
			{
				for (int x = 0; x < Width; x ++)
				{
					EBlockType blockType = this.MapGrid[y, x];

					if (blockType == EBlockType.EnemySpawner)
					{
						mapGraphics.DrawImage(this.BlockImages[(int)EBlockType.EnemySpawner], x * CellSize, y * CellSize, CellSize, CellSize);
					}
					else if (blockType != EBlockType.EmptySpace)
					{
						mapGraphics.DrawImage(this.BlockImages[(int)EBlockType.None], x * CellSize, y * CellSize, CellSize, CellSize);
					}
				}
			}
			
			mapGraphics.Dispose();
			return mapBitmap;
		}

		public Bitmap GetConstructureBitmap()
		{
			Bitmap mapBitmap = new Bitmap(Width * CellSize + 1, Height * CellSize + 1);
			Graphics mapGraphics = Graphics.FromImage(mapBitmap);

			bool isCoreDraw = false;

			for (int y = 0; y < Height; y ++)
			{
				for (int x = 0; x < Width; x ++)
				{
					EBlockType blockType = this.MapGrid[y, x];

					if ((blockType == EBlockType.EmptySpace) || (blockType == EBlockType.None))
					{
						continue;
					}

					if (blockType == EBlockType.Core)
					{
						if (this.CoreHealthPoint <= 0)
						{
							break;
						}

						if (isCoreDraw)
						{
							continue;
						}

						mapGraphics.DrawImage(this.BlockImages[(int)blockType], x * CellSize, y * CellSize, CellSize * 2, CellSize * 2);
						isCoreDraw = true;
						continue;
					}

					mapGraphics.DrawImage(this.BlockImages[(int)blockType], x * CellSize, y * CellSize, CellSize, CellSize);
				}
			}
			
			mapGraphics.Dispose();
			return mapBitmap;
		}

		#endregion
	}
}
