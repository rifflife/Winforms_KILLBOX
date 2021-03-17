using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace CenterDefenceGame.GameObject
{
	public class Player
	{
		private GameManager Manager;
		
		private Polygon2D[] BodyPolygons;
		private Color[] BodyColors;
		private int Width;
		private int Height;
		private float X;
		private float Y;
		private float Rotation;
		private float MoveSpeed;
		private float MoveAxisSpeed;
		private float MoveAngularSpeed;
		private bool IsDead;

		private int HitDelay;
		private int HitDelayIni = 4;
		
		#region Player Data

		private int HealthPoint;
		private int HealthPointMax;

		private Color LeftBulletColor = Color.FromArgb(0, 70, 220);
		private Color RightBulletColor = Color.FromArgb(255, 220, 0);

		private float MaxRecoil;
		private float Recoil;
		private float DecreaseRecoil;

		// Left Weapon Variable
		private float LeftWeaponFireRate;
		private float LeftWeaponFireRateIni;
		private int LeftWeaponDamage;
		private int LeftWeaponSpeed;
		private int LeftWeaponExtension;
		private float LeftWeaponRecoil;
		private int LeftWeaponAmmoNeed;

		// Left Upgrade Level
		public int LevelLeftWeaponFireRate;
		public int LevelLeftWeaponDamage;
		public int LevelLeftWeaponExtension;
		public int LevelLeftWeaponRecoil;
		
		// Right Weapon Variable
		private float RightWeaponMinFireAngle;
		
		private float RightWeaponFireRate;
		private float RightWeaponFireRateIni;
		private int RightWeaponDamage;
		private int RightWeaponSpeed;
		private int RightWeaponExtension;
		private float RightWeaponRecoil;
		private int RightWeaponAmmoNeed;
		
		// Right Upgrade Level
		public int LevelRightWeaponFireRate;
		public int LevelRightWeaponDamage;
		public int LevelRightWeaponExtension;
		public int LevelRightWeaponRecoil;

		// Player General Level
		public int LevelPlayerHealth;
		public int LevelPlayerRegen;

		private int RegenInWaveDelayIni = 60;
		private int RegenInWaveDelay;
		private int RegenInWave;

		public int LevelPlayerReborn;
		private int PlayerRebornTime;
		private int PlayerRebornDelay;

		#endregion

		public Player(GameManager gameManager)
		{
			this.Manager = gameManager;

			// 초기 플레이어 시선은 아래
			this.MoveAxisSpeed = 5f;
			this.MoveAngularSpeed = 3.5f;

			#region Player Body Polygons Setting

			// Player Bodies Color Set
			this.BodyColors = new Color[5];
			// Body
			this.BodyColors[0] = Color.FromArgb(150, 150, 150);
			// Arm 1
			this.BodyColors[1] = Color.FromArgb(100, 100, 100);
			// Arm 2
			this.BodyColors[2] = Color.FromArgb(100, 100, 100);
			// Head
			this.BodyColors[3] = Color.FromArgb(70, 70, 70);
			// If Player takes damage
			this.BodyColors[4] = Color.FromArgb(200, 0, 0);

			this.BodyPolygons = new Polygon2D[4];

			// Body Size
			int bWidth = 30;
			int bHeight = 23;
			int bOffset = 2;
			this.BodyPolygons[0] = new Polygon2D(this.X, this.Y, -bWidth / 2, -bHeight / 2 + bOffset, bWidth, bHeight, this.BodyColors[0]);

			// Arms Size
			int aWidth = 15;
			int aHeight = 15;
			int aOffset = 17;
			this.BodyPolygons[1] = new Polygon2D(this.X, this.Y, - aWidth - aOffset, -aHeight / 2, aHeight, aWidth, this.BodyColors[1]);
			this.BodyPolygons[2] = new Polygon2D(this.X, this.Y,			aOffset, -aHeight / 2, aHeight, aWidth, this.BodyColors[2]);

			// Head Size
			int hWidth = 14;
			int hHeight = 14;
			this.BodyPolygons[3] = new Polygon2D(this.X, this.Y, -hHeight / 2, -hWidth / 2, hWidth, hHeight, this.BodyColors[3]);

			#endregion
		}

		public void Reset(int x, int y)
		{
			// Initialize Player Position
			this.X = x;
			this.Y = y;
			this.Width = 40;
			this.Height = 40;
			this.Rotation = (float)(270 * Math.PI / 180);
			this.MoveSpeed = 0;

			// Weapon Initialize
			#region Player Data

			Recoil = 0;
			MaxRecoil = DataTable.MaxRecoil;
			DecreaseRecoil = DataTable.DecreaseRecoil;

			LevelLeftWeaponFireRate	 = 0;
			LevelLeftWeaponDamage	 = 0;
			LevelLeftWeaponExtension = 0;
			LevelLeftWeaponRecoil    = 0;
			
			RightWeaponMinFireAngle = DataTable.RightWeaponMinFireAngle;

			LevelRightWeaponFireRate  = 0;
			LevelRightWeaponDamage	  = 0;
			LevelRightWeaponExtension = 0;
			LevelRightWeaponRecoil    = 0;

			LevelPlayerHealth = 0;
			LevelPlayerRegen  = 0;
			LevelPlayerReborn = 0;

			#endregion

			SetPlayerData();
		}

		public void SetPlayerPosition(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public void SetPlayerData()
		{
			LeftWeaponFireRate = 0;
			LeftWeaponFireRateIni  = DataTable.LeftWeaponFireRate[0, LevelLeftWeaponFireRate];
			LeftWeaponDamage    = DataTable.LeftWeaponDamage[0, LevelLeftWeaponDamage];
			LeftWeaponSpeed     = DataTable.LeftWeaponDamage[2, LevelLeftWeaponDamage];
			LeftWeaponExtension = DataTable.LeftWeaponExtension[0, LevelLeftWeaponExtension];

			LeftWeaponRecoil = (float)((Math.PI / 1800) * DataTable.LeftWeaponRecoil[0, LevelLeftWeaponRecoil]);

			LeftWeaponAmmoNeed = LeftWeaponExtension;

			RightWeaponFireRate = 0;
			RightWeaponFireRateIni  = DataTable.RightWeaponFireRate[0, LevelRightWeaponFireRate];
			RightWeaponDamage    = DataTable.RightWeaponDamage[0, LevelRightWeaponDamage];
			RightWeaponSpeed     = DataTable.LeftWeaponDamage[2, LevelRightWeaponDamage];
			RightWeaponExtension = DataTable.RightWeaponExtension[0, LevelRightWeaponExtension];
			
			RightWeaponRecoil = (float)((Math.PI / 1800) * DataTable.RightWeaponRecoil[0, LevelRightWeaponRecoil]);

			RightWeaponAmmoNeed = RightWeaponExtension;

			// 체력
			HealthPointMax = DataTable.PlayerHealth[0, LevelPlayerHealth];
			HealthPoint = HealthPointMax;

			// 재생성 시간
			PlayerRebornTime = DataTable.PlayerRebornTime[0, LevelPlayerReborn] * 60;
			PlayerRebornDelay = 0;

			// 체력 재생
			RegenInWave = DataTable.PlayerRegen[0, LevelPlayerRegen];
		}

		public void Update(float deltaRatio)
		{
			if (this.Manager.GameMap.IsCoreDestroyed())
			{
				this.Hit(9999999);
				return;
			}

			#region Player Check Condition

			if (this.Manager.GetGameSituation() != ESystemSituation.Wave)
			{
				this.Reborn();
			}
			else
			{
				// 웨이브중에 죽었다면 플레이어 재생성
				if (this.IsDead)
				{
					this.Reborn();

					return;
				}
				// 웨이브 중에는 채력 재생
				else
				{
					if (RegenInWaveDelay > 0)
					{
						RegenInWaveDelay --;
					}
					else
					{
						RegenInWaveDelay = RegenInWaveDelayIni;
						if (HealthPoint < HealthPointMax - RegenInWave)
						{
							HealthPoint += RegenInWave;
						}
						else
						{
							HealthPoint = HealthPointMax;
						}
					}
				}
			}

			#endregion

			#region Player Aim Direction Set

			Point gameMouse = this.Manager.GetIngameMousePosition();
			float vectorX = gameMouse.X - this.X;
			float vectorY = gameMouse.Y - this.Y;
			float direction = (float)Math.Atan2(vectorX, vectorY);
			this.Rotation = direction;

			#endregion

			#region Player Fire Bullet

			if (this.LeftWeaponFireRate > 0)
			{
				this.LeftWeaponFireRate -= deltaRatio;
			}

			if (this.RightWeaponFireRate > 0)
			{
				this.RightWeaponFireRate -= deltaRatio;
			}

			// Decrease Recoil

			if (Recoil > DecreaseRecoil)
			{
				Recoil -= DecreaseRecoil;
			}
			else
			{
				Recoil = 0;
			}

			// Fire Left Weapon
			if ((this.LeftWeaponFireRate <= 0) && (this.Manager.MainForm.IsMousePressed[MouseButtons.Left]) &&
				((this.Manager.GetGameSituation() == ESystemSituation.Wave) || (this.Manager.GetGameSituation() == ESystemSituation.Standby)))
			{
				if (this.Manager.ConsumAmmunition(LeftWeaponAmmoNeed))
				{
					this.LeftWeaponFireRate = this.LeftWeaponFireRateIni;

					// 난수 생성
					Random rand = new Random();
					float randRecoilRatio = (float)(rand.NextDouble());

					float addRecoil = Recoil * randRecoilRatio;
					
					if (rand.Next(2) == 1)
					{
						addRecoil *= -1;
					}

					float diagonal_90 = (float)(Math.PI / 2);

					float fireWidth = DataTable.LeftWeaponFireWidth;

					Vector2D bulletPoint1 = new Vector2D ((float)Math.Sin(this.Rotation - diagonal_90), (float)Math.Cos(this.Rotation - diagonal_90));
					Vector2D bulletPoint2 = new Vector2D ((float)Math.Sin(this.Rotation + diagonal_90), (float)Math.Cos(this.Rotation + diagonal_90));

					if (LeftWeaponExtension % 2 == 0)
					{
						float fireHalfWidth = fireWidth / 2;
						int fireCount = LeftWeaponExtension / 2;
						
						Vector2D createP1 = bulletPoint1 * fireHalfWidth;
						Vector2D createP2 = bulletPoint2 * fireHalfWidth;
						bulletPoint1 *= fireWidth;
						bulletPoint2 *= fireWidth;

						for (int fire = 0; fire < fireCount; fire ++)
						{
							float addX1 = createP1.X + (bulletPoint1.X * fire);
							float addY1 = createP1.Y + (bulletPoint1.Y * fire);
							
							float addX2 = createP2.X + (bulletPoint2.X * fire);
							float addY2 = createP2.Y + (bulletPoint2.Y * fire);

							var pb1 = new PlayerBullet(this.Manager, X + addX1, Y + addY1, gameMouse.X + addX1, gameMouse.Y + addY1,
								LeftWeaponSpeed, LeftWeaponDamage, LeftBulletColor, addRecoil);
							var pb2 = new PlayerBullet(this.Manager, X + addX2, Y + addY2, gameMouse.X + addX2, gameMouse.Y + addY2,
								LeftWeaponSpeed, LeftWeaponDamage, LeftBulletColor, addRecoil);

							this.Manager.GamePlayerBulletList.CreateBullet(pb1);
							this.Manager.GamePlayerBulletList.CreateBullet(pb2);
						}
					}
					else
					{
						bulletPoint1 *= fireWidth;
						bulletPoint2 *= fireWidth;
						
						var pb = new PlayerBullet(this.Manager, X, Y, gameMouse.X, gameMouse.Y,
							LeftWeaponSpeed, LeftWeaponDamage, LeftBulletColor, addRecoil);
						this.Manager.GamePlayerBulletList.CreateBullet(pb);

						if (LeftWeaponExtension >= 3)
						{
							for (int fire = 1; fire < LeftWeaponExtension / 2 + 1; fire++)
							{
								float addX1 = bulletPoint1.X * fire;
								float addY1 = bulletPoint1.Y * fire;
							
								float addX2 = bulletPoint2.X * fire;
								float addY2 = bulletPoint2.Y * fire;

								var pb1 = new PlayerBullet(this.Manager, X + addX1, Y + addY1, gameMouse.X + addX1, gameMouse.Y + addY1,
									LeftWeaponSpeed, LeftWeaponDamage, LeftBulletColor, addRecoil);
								var pb2 = new PlayerBullet(this.Manager, X + addX2, Y + addY2, gameMouse.X + addX2, gameMouse.Y + addY2,
									LeftWeaponSpeed, LeftWeaponDamage, LeftBulletColor, addRecoil);

								this.Manager.GamePlayerBulletList.CreateBullet(pb1);
								this.Manager.GamePlayerBulletList.CreateBullet(pb2);
							}
						}
					}

					// 반동 추가
					IncreaseRecoil(LeftWeaponRecoil);
				}
			}
			
			// Fire Right Weapon
			if ((this.RightWeaponFireRate <= 0) && (this.Manager.MainForm.IsMousePressed[MouseButtons.Right]) &&
				((this.Manager.GetGameSituation() == ESystemSituation.Wave) || (this.Manager.GetGameSituation() == ESystemSituation.Standby)))
			{
				this.RightWeaponFireRate = this.RightWeaponFireRateIni;

				if (this.Manager.ConsumAmmunition(RightWeaponAmmoNeed))
				{
					float fireAngleStart = 0;

					if (Recoil > RightWeaponMinFireAngle)
					{
						fireAngleStart = Recoil;
					}
					else
					{
						fireAngleStart = RightWeaponMinFireAngle;
					}

					float fireAngleIndex = (fireAngleStart * 2) / (RightWeaponExtension - 1);

					for (int fire = 0; fire < RightWeaponExtension; fire++)
					{
						var pb = new PlayerBullet(this.Manager, X, Y, gameMouse.X, gameMouse.Y,
							RightWeaponSpeed, RightWeaponDamage, RightBulletColor, fireAngleStart - fireAngleIndex * fire);
						this.Manager.GamePlayerBulletList.CreateBullet(pb);
					}
				}
				
				// 반동 추가
				IncreaseRecoil(RightWeaponRecoil);
			}

			#endregion

			#region Player Move and Collision Detection

			int moveDirectionX = 0;
			int moveDirectionY = 0;

			if (this.Manager.MainForm.IsKeyPressed[Keys.W])
			{
				moveDirectionY --;
			}
			if (this.Manager.MainForm.IsKeyPressed[Keys.S])
			{
				moveDirectionY ++;
			}
			if (this.Manager.MainForm.IsKeyPressed[Keys.A])
			{
				moveDirectionX --;
			}
			if (this.Manager.MainForm.IsKeyPressed[Keys.D])
			{
				moveDirectionX ++;
			}

			if (moveDirectionX != 0 && moveDirectionY != 0)
			{
				this.MoveSpeed = this.MoveAngularSpeed;
			}
			else
			{
				this.MoveSpeed = this.MoveAxisSpeed;
			}
			
			// Collision Detected
			int cellSize = this.Manager.GameMap.CellSize;

			float hWidth  = this.Width  / 2;
			float hHeight = this.Height / 2;

			this.X += moveDirectionX * this.MoveSpeed * deltaRatio;
			
			int cRight	= (int)((this.X + hWidth) / cellSize);
			int cLeft	= (int)((this.X - hWidth) / cellSize);
			int cBottom	= (int)((this.Y + hHeight) / cellSize);
			int cTop	= (int)((this.Y - hHeight) / cellSize);
			bool detectRightTop		= this.Manager.GameMap.GetCellPointCollision(cRight, cTop   ,true);
			bool detectRightBottom	= this.Manager.GameMap.GetCellPointCollision(cRight, cBottom,true);
			bool detectLeftTop		= this.Manager.GameMap.GetCellPointCollision(cLeft , cTop   ,true);
			bool detectLeftBottom	= this.Manager.GameMap.GetCellPointCollision(cLeft , cBottom,true);

			if (moveDirectionX > 0)
			{
				if (detectRightTop || detectRightBottom)
				{
					this.X = cRight * cellSize - this.Width / 2 - 1;
				}
			}
			else if (moveDirectionX < 0)
			{
				if (detectLeftTop || detectLeftBottom)
				{
					this.X = (cLeft + 1) * cellSize + this.Width / 2;
				}
			}

			this.Y += moveDirectionY * this.MoveSpeed * deltaRatio;
			
			cRight	= (int)((this.X + hWidth) / cellSize);
			cLeft	= (int)((this.X - hWidth) / cellSize);
			cBottom	= (int)((this.Y + hHeight) / cellSize);
			cTop	= (int)((this.Y - hHeight) / cellSize);

			detectRightTop		= this.Manager.GameMap.GetCellPointCollision(cRight, cTop   ,true);
			detectRightBottom	= this.Manager.GameMap.GetCellPointCollision(cRight, cBottom,true);
			detectLeftTop		= this.Manager.GameMap.GetCellPointCollision(cLeft , cTop   ,true);
			detectLeftBottom	= this.Manager.GameMap.GetCellPointCollision(cLeft , cBottom,true);

			if (moveDirectionY > 0)
			{
				if (detectRightBottom || detectLeftBottom)
				{
					this.Y = cBottom * cellSize - this.Height / 2 - 1;
				}
			}
			else if (moveDirectionY < 0)
			{
				if (detectRightTop || detectLeftTop)
				{
					this.Y = (cTop + 1) * cellSize + this.Height / 2;
				}
			}

			// 플레이어가 맵 밖으로 나가졌을 때
			if ((this.X < 0) || (this.X > this.Manager.GameMap.Width * cellSize) ||
				(this.Y < 0) || (this.Y > this.Manager.GameMap.Height * cellSize))
			{
				this.X = this.Manager.GameMap.Width  * cellSize / 2;
				this.Y = this.Manager.GameMap.Height * cellSize / 2;
				this.Manager.GameGUI.AddNotification(EMessageType.Error, "Player Position ERROR!");
			}
			
			#endregion
		}

		public int GetAimRecoil()
		{
			return (int)(Recoil * 100);
		}

		public void IncreaseRecoil(float amount)
		{
			if (Recoil < MaxRecoil - amount)
			{
				Recoil += amount;
			}
			else
			{
				Recoil = MaxRecoil;
			}
		}

		public void Draw(Graphics graphics)
		{
			if (this.IsDead)
			{
				return;
			}

			if (this.HitDelay > 1)
			{
				this.HitDelay --;
				for (int index = 0; index < this.BodyPolygons.Count(); index++)
				{
					this.BodyPolygons[index].SetColor(this.BodyColors[4]);
				}
			}
			else if (this.HitDelay == 1)
			{
				this.HitDelay = 0;
				for (int index = 0; index < this.BodyPolygons.Count(); index++)
				{
					this.BodyPolygons[index].SetColor(this.BodyColors[index]);
				}
			}

			// Draw Player Body
			for (int index = 0; index < BodyPolygons.Count(); index ++)
			{
				this.BodyPolygons[index].SetPosition(this.X, this.Y);
				this.BodyPolygons[index].Rotate(this.Rotation);
				this.BodyPolygons[index].Draw(graphics, this.Manager.GameCamera);
			}
		}
		
		private void Reborn()
		{
			ESystemSituation situation = this.Manager.GetGameSituation();

			if (situation == ESystemSituation.End)
			{
				return;
			}

			int endWaveRegenSpeed = 5;

			if (situation == ESystemSituation.Wave)
			{
				this.HealthPoint = (int)((PlayerRebornDelay / (float)PlayerRebornTime) * HealthPointMax);
				
				if (PlayerRebornDelay < PlayerRebornTime)
				{
					PlayerRebornDelay ++; 
				}
			}
			else
			{
				if (HealthPoint < HealthPointMax - endWaveRegenSpeed)
				{
					HealthPoint += endWaveRegenSpeed;
				}
				else
				{
					HealthPoint = HealthPointMax;
				}
			}

			if (this.IsDead && (HealthPoint == HealthPointMax))
			{
				PlayerRebornDelay = 0;
				// 부활
				this.HealthPoint = this.HealthPointMax;
				// 부활 이펙트
				this.Manager.CreateDeadEffect((int)this.X, (int)this.Y, 20, false);
				this.IsDead = false;
			}
		}

		public void Hit(int damage)
		{
			if (this.IsDead)
			{
				return;
			}

			if (this.HealthPoint > damage)
			{
				this.HealthPoint -= damage;
				this.HitDelay = this.HitDelayIni;
			}
			else
			{
				this.HealthPoint = 0;
				this.IsDead = true;
				this.Manager.CreateDeadEffect((int)this.X, (int)this.Y, 14, true);
				this.X = this.Manager.GameMap.GetCenterX();
				this.Y = this.Manager.GameMap.GetCenterY();
			}
		}

		#region Getter and Setter

		public float GetX()
		{
			return this.X;
		}

		public float GetY()
		{
			return this.Y;
		}

		public int GetWidth()
		{
			return this.Width;
		}

		public int GetHeight()
		{
			return this.Height;
		}

		public float GetHealthPointRatio()
		{
			return (float)HealthPoint / (float)HealthPointMax;
		}

		public float GetDistance(float x, float y)
		{
			float lengthX = this.X - x;
			float lengthY = this.Y - y;

			return (float)Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
		}

		#endregion
	}
}
