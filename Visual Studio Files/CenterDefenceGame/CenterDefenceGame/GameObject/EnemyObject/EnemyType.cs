using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.EnemyObject
{
	public class EnemyType
	{
		protected GameManager Manager;
		protected Polygon2D[] BodyPolygons;
		protected Color[] BodyColors;

		protected Vector2D Position;
		protected Vector2D MoveAmount;
		protected Vector2D BounceAmount;
		protected Point Destination;

		// 맵 센터로 설정된 도착 좌표는 경로 찾기 실패 좌표이다.
		protected Point MapCenter;

		protected int Width;
		protected int Height;
		protected float MoveSpeed;
		protected float MoveDirectionChangeDelay = 0;
		protected float MoveDirectionChangeDelayIni = 10;

		protected int HealthPoint;
		protected int HealthPointMax;
		protected bool IsDead;
		private int HitDelay;
		private int HitDelayIni = 5;

		protected float AttackDelay;
		protected float AttackDelayIni;

		protected EGoDirection PathToGo;

		protected bool IsStuck;
		private int StuckDelay = 0;

		protected bool IsWantAttackPlayer;
		protected int Damage;
		protected int DropResource;
		protected int AttackReach;
		private int PlayerDetectReach;

		protected bool IsBig = false;

		public Vector2D GetPosition()
		{
			return this.Position;
		}

		public EnemyType(GameManager gameManager, int x, int y, float moveSpeed, int healthPointMax,
			float attackDelayIni, int attackReach, int damage, int dropResource, EGoDirection pathToGo)
		{
			this.Manager = gameManager;
			this.MapCenter = new Point(this.Manager.GameMap.GetCenterX(), this.Manager.GameMap.GetCenterY());
			this.PlayerDetectReach = this.Manager.GameMap.CellSize;

			this.AttackReach = attackReach;
			this.Damage = damage;
			this.PathToGo = pathToGo;

			if (this.PathToGo == EGoDirection.Player || this.PathToGo == EGoDirection.PlayerIgnoreBlock)
			{
				this.IsWantAttackPlayer = true;
			}

			this.DropResource = dropResource;

			// Initialize Eenemy Position
			this.Position = new Vector2D(x, y);
			this.MoveAmount = new Vector2D(0, 0);
			this.BounceAmount = new Vector2D(0, 0);

			this.MoveSpeed = moveSpeed;
			
			this.HealthPointMax	= healthPointMax;
			this.HealthPoint	= this.HealthPointMax;
			this.AttackDelayIni = attackDelayIni;
			this.AttackDelay	= 0;

			this.IsDead			= false;
		}

		public void Update(float deltaRatio)
		{
			if (this.IsStuck)
			{
				return;
			}

			#region Refresh

			if (this.IsDead)
			{
				return;
			}

			if (!(this.Manager.GetGameSituation() == ESystemSituation.Wave) ||
				(this.Manager.GameMap.IsOverPoint(this.Position.X, this.Position.Y)))
			{
				if (this.Manager.GetGameSituation() == ESystemSituation.End)
				{
					if (!this.Manager.GameMap.IsCoreDestroyed())
					{
						this.Kill();
						return;
					}
				}
				else
				{
					this.Kill();
					return;
				}
			}

			#endregion
			
			#region Set Move Destination and Direction, Detect Stuck Situation

			bool isDirectlyGoToPlayer = false;

			if (this.MoveDirectionChangeDelay > 0)
			{
				this.MoveDirectionChangeDelay -= deltaRatio;
			}
			else
			{
				Point cellPoint = this.Manager.GameMap.GetCellPosition(this.Position.X, this.Position.Y);

				// 만약에 적이 어딘가 끼어서 못움직이면
				if (this.Manager.GameMap.GetCellPointCollision(cellPoint.X, cellPoint.Y, false))
				{
					if (this.StuckDelay < 10)
					{
						this.StuckDelay ++;
					}
					else
					{
						this.IsStuck = true;
					}
				}
				else
				{
					this.StuckDelay = 0;
				}

				// 움직일 목표지점 설정
				if (this.PathToGo == EGoDirection.Center)
				{
					// 그냥 중앙으로 간다.
					this.Destination.X = this.Manager.GameMap.GetCenterX();
					this.Destination.Y = this.Manager.GameMap.GetCenterY();
				}
				else
				{
					// 플레이어를 향한 경로에서 플레이어에 근접했을 때.
					if (((this.PathToGo == EGoDirection.Player)|| (this.PathToGo == EGoDirection.PlayerIgnoreBlock)) &&
						(this.Manager.GamePlayer.GetDistance(this.Position.X, this.Position.Y) < this.PlayerDetectReach))
					{
						// 플레이어를 향해 직진
						this.Destination.X = (int)this.Manager.GamePlayer.GetX();
						this.Destination.Y = (int)this.Manager.GamePlayer.GetY();
						isDirectlyGoToPlayer = true;
					}
					// 플레이어를 향한 경로가 아닐 때
					else
					{
						if (this.IsWantAttackPlayer)
						{
							Point position = this.Manager.GameMap.GetCellPosition(this.Position.X, this.Position.Y);

							// 플레이어를 다시 쫒을 수 있다면 쫒아간다.
							if (this.Manager.GameMap.GetPathDirection(position.X, position.Y, EGoDirection.Player) != EDirection.None)
							{
								// 단 한번만 다시 쫒는다.
								this.IsWantAttackPlayer = false;
								this.PathToGo = EGoDirection.Player;
							}
						}

						// 경로 찾기
						this.Destination = this.Manager.GameMap.GetPathDestination(this.Position.X, this.Position.Y, this.PathToGo);
					}
					
					// 방향 잃음
					if (this.Destination == this.MapCenter)
					{
						switch(this.PathToGo)
						{
							// 벽을 제외한 코어로 향하는 길이 없다.
							case EGoDirection.Core:
								this.PathToGo = EGoDirection.All;
								break;
							
							// 벽을 제외한 모든 구조물로 향하는 길이 없다.
							case EGoDirection.AllExceptWall:
								this.PathToGo = EGoDirection.All;
								break;

							// 벽과 구조물을 제외한 공간의 플레이어를 향하는 길이 없다.
							case EGoDirection.Player:
								this.PathToGo = EGoDirection.PlayerIgnoreBlock;
								break;

							// 벽을 제외한 구조물을 모두 무시하고 플레이어를 향하는 길이 없다.
							case EGoDirection.PlayerIgnoreBlock:
								this.PathToGo = EGoDirection.Core;
								break;

							// 모든게 파괴되면 그냥 중앙으로 가서 비빈다.
							case EGoDirection.All:
								this.PathToGo = EGoDirection.Center;
								break;
						}
					}
				}

				this.MoveAmount = new Vector2D(this.Destination.X - this.Position.X, this.Destination.Y - this.Position.Y).GetNormalize();

				float direction = (float)Math.Atan2(MoveAmount.X, MoveAmount.Y);
				this.Position.Rotation = direction;

				this.MoveDirectionChangeDelay = this.MoveDirectionChangeDelayIni;

				if (isDirectlyGoToPlayer)
				{
					// 플레이어 발견시 지연없이 바로이동
					this.MoveDirectionChangeDelay = 0;
				}
			}

			#endregion

			#region Attack

			if (this.AttackDelay > 0)
			{
				this.AttackDelay --;
			}
			else
			{
				// 3방향을 검사해야함, 공격 방향, 공격방향으로부터 일정 각도인 두 방향
				// 45도 방향으로 향할때 대각선방향의 블럭만 공격하고 수직수평 방향의 블럭은 공격하지 않는 오류가 있음.
				float diagonal = (float)(Math.PI / 4);

				Vector2D attackPoint = new Vector2D ((float)Math.Sin(this.Position.Rotation), (float)Math.Cos(this.Position.Rotation));
				attackPoint *= this.AttackReach;
				attackPoint += this.Position;
					
				bool isAttackedPlayer = false;
				bool isFindBlock = this.Manager.GameMap.IsAttackable(attackPoint.X, attackPoint.Y, this.PathToGo);

				// 공격방향 검사
				if (!isFindBlock)
				{
					// 대각선 방향 1 검사
					attackPoint.X = (float)((Math.Sin(this.Position.Rotation - diagonal) * this.AttackReach) + this.Position.X);
					attackPoint.Y = (float)((Math.Cos(this.Position.Rotation - diagonal) * this.AttackReach) + this.Position.Y);
					isFindBlock = this.Manager.GameMap.IsAttackable(attackPoint.X, attackPoint.Y, this.PathToGo);
				}

				if (!isFindBlock)
				{
					// 대각선 방향 2 검사
					attackPoint.X = (float)((Math.Sin(this.Position.Rotation + diagonal) * this.AttackReach) + this.Position.X);
					attackPoint.Y = (float)((Math.Cos(this.Position.Rotation + diagonal) * this.AttackReach) + this.Position.Y);
					isFindBlock = this.Manager.GameMap.IsAttackable(attackPoint.X, attackPoint.Y, this.PathToGo);
				}

				// 공격 대상 블럭을 찾은 경우
				if (isFindBlock)
				{
					this.Manager.GameMap.Attacked(attackPoint.X, attackPoint.Y, this.Damage);
					if (this.Manager.GamePlayer.GetDistance(this.Position.X, this.Position.Y) < this.AttackReach * 2)
					{
						isAttackedPlayer = true;
						this.Manager.GamePlayer.Hit(this.Damage);
					}

					this.AttackDelay = this.AttackDelayIni;
				}

				if ((!isAttackedPlayer)||
					(this.PathToGo == EGoDirection.Player) ||
					(this.PathToGo == EGoDirection.PlayerIgnoreBlock))
				{
					// 플레이어가 근접하면 공격한다.
					if (this.Manager.GamePlayer.GetDistance(this.Position.X, this.Position.Y) < this.AttackReach)
					{
						this.Manager.GamePlayer.Hit(this.Damage);
						this.AttackDelay = this.AttackDelayIni;
					}
				}
			}

			#endregion

			#region Move and Collision
			
			float moveAmountX = this.MoveAmount.X * this.MoveSpeed * deltaRatio + this.BounceAmount.X;
			float moveAmountY = this.MoveAmount.Y * this.MoveSpeed * deltaRatio + this.BounceAmount.Y;

			this.BounceAmount.X = 0;
			this.BounceAmount.Y = 0;

			// Collision Detected
			int cellSize = this.Manager.GameMap.CellSize;

			float hWidth  = this.Width  / 2;
			float hHeight = this.Height / 2;
			
			this.Position.X += moveAmountX;
			
			int cRight	= (int)((this.Position.X + hWidth) / cellSize);
			int cLeft	= (int)((this.Position.X - hWidth) / cellSize);
			int cBottom	= (int)((this.Position.Y + hHeight) / cellSize);
			int cTop	= (int)((this.Position.Y - hHeight) / cellSize);
			bool detectRightTop		= this.Manager.GameMap.GetCellPointCollision(cRight, cTop   , false);
			bool detectRightBottom	= this.Manager.GameMap.GetCellPointCollision(cRight, cBottom, false);
			bool detectLeftTop		= this.Manager.GameMap.GetCellPointCollision(cLeft , cTop   , false);
			bool detectLeftBottom	= this.Manager.GameMap.GetCellPointCollision(cLeft , cBottom, false);

			if (moveAmountX > 0)
			{
				if (detectRightTop || detectRightBottom)
				{
					this.Position.X = cRight * cellSize - this.Width / 2 - 1;
				}
			}
			else if (moveAmountX < 0)
			{
				if (detectLeftTop || detectLeftBottom)
				{
					this.Position.X = (cLeft + 1) * cellSize + this.Width / 2;
				}
			}

			this.Position.Y += moveAmountY;
			
			cRight	= (int)((this.Position.X + hWidth) / cellSize);
			cLeft	= (int)((this.Position.X - hWidth) / cellSize);
			cBottom	= (int)((this.Position.Y + hHeight) / cellSize);
			cTop	= (int)((this.Position.Y - hHeight) / cellSize);

			detectRightTop		= this.Manager.GameMap.GetCellPointCollision(cRight, cTop   , false);
			detectRightBottom	= this.Manager.GameMap.GetCellPointCollision(cRight, cBottom, false);
			detectLeftTop		= this.Manager.GameMap.GetCellPointCollision(cLeft , cTop   , false);
			detectLeftBottom	= this.Manager.GameMap.GetCellPointCollision(cLeft , cBottom, false);

			if (moveAmountY > 0)
			{
				if (detectRightBottom || detectLeftBottom)
				{
					this.Position.Y = cBottom * cellSize - this.Height / 2 - 1;
				}
			}
			else if (moveAmountY < 0)
			{
				if (detectRightTop || detectLeftTop)
				{
					this.Position.Y = (cTop + 1) * cellSize + this.Height / 2;
				}
			}

			#endregion

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

				for (int index = 0; index < this.BodyPolygons.Count(); index ++)
				{
					this.BodyPolygons[index].SetColor(this.BodyColors[this.BodyPolygons.Count()]);
				}
			}
			else if (this.HitDelay == 1)
			{
				this.HitDelay = 0;

				for (int index = 0; index < this.BodyPolygons.Count(); index ++)
				{
					this.BodyPolygons[index].SetColor(this.BodyColors[index]);
				}
			}

			for (int index = 0; index < this.BodyPolygons.Count(); index ++)
			{
				this.BodyPolygons[index].SetPosition(this.Position.X, this.Position.Y);
				this.BodyPolygons[index].Rotate(this.Position.Rotation);
				this.BodyPolygons[index].Draw(graphics, this.Manager.GameCamera);
			}
			
			#region Debug

			// Draw Direction
			if (this.Manager.MainForm.IsDebugging)
			{
				float diagonal = (float)(Math.PI / 4);
				Vector2D vector1 = new Vector2D ((float)Math.Sin(this.Position.Rotation), (float)Math.Cos(this.Position.Rotation));
				Vector2D vector2 = new Vector2D ((float)Math.Sin(this.Position.Rotation - diagonal), (float)Math.Cos(this.Position.Rotation - diagonal));
				Vector2D vector3 = new Vector2D ((float)Math.Sin(this.Position.Rotation + diagonal), (float)Math.Cos(this.Position.Rotation + diagonal));
				
				Vector2D positionVector = new Vector2D(this.Position.X, this.Position.Y);
				positionVector.X -= this.Manager.GameCamera.GetX();
				positionVector.Y -= this.Manager.GameCamera.GetY();

				vector1 *= this.AttackReach;
				vector1 += positionVector;

				vector2 *= this.AttackReach;
				vector2 += positionVector;

				vector3 *= this.AttackReach;
				vector3 += positionVector;

				using (Pen pen = new Pen(Brushes.Blue, 1))
				{
					graphics.DrawLine(pen, positionVector.GetPoint(), vector1.GetPoint());
					graphics.DrawLine(pen, positionVector.GetPoint(), vector2.GetPoint());
					graphics.DrawLine(pen, positionVector.GetPoint(), vector3.GetPoint());
				}
			}
			#endregion
		}

		public bool IsDisabled()
		{
			return this.IsDead;
		}

		public void Kill()
		{
			this.HealthPoint = 0;
			if (IsBig)
			{
				this.Manager.CreateDeadEffect((int)this.Position.X, (int)this.Position.Y, 6, true);
			}
			else
			{
				this.Manager.CreateDeadEffect((int)this.Position.X, (int)this.Position.Y, 3, false);
			}
			this.Position.X = -1000;
			this.Position.Y = -1000;
			this.IsDead = true;
		}

		public void Hit(int damage)
		{
			this.HitDelay = this.HitDelayIni;

			if (this.HealthPoint > damage)
			{
				this.HealthPoint -= damage;
			}
			else
			{
				this.Manager.AddRecourceCount(this.DropResource);
				this.Kill();
			}
		}

		public int GetWidth()
		{
			return this.Width;
		}

		public int GetHeight()
		{
			return this.Height;
		}

		public void Move(float amountX, float amountY)
		{
			this.BounceAmount.X = amountX;
			this.BounceAmount.Y = amountY;
		}
	}
}
