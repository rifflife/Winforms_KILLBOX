using CenterDefenceGame.GameObject.DrawObject;
using CenterDefenceGame.GameObject.EnemyObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject
{
	public class EnemyList
	{
		private GameManager Manager;
		private EnemyType[] Enemies;
		public readonly int MaxCount = 100;
		private int Count = 0;
		private readonly float PushEachOtherAmount = 0.1f;

		public EnemyList(GameManager gameManager)
		{
			this.Manager = gameManager;

			this.Enemies = new EnemyType[this.MaxCount];
		}

		public void Reset()
		{
			for (int clear = 0; clear < this.MaxCount; clear++)
			{
				this.Enemies[clear] = null;
			}

			this.Count = 0;
		}

		/// <summary>
		/// 성공시 true 실패시 false, 최대 생성수를 넘으면 실패한다.
		/// </summary>
		/// <param name="enemy"></param>
		/// <returns></returns>
		public bool CreateEnemy(int level)
		{
			Point spawnPoint = this.Manager.GameMap.GetEnemySpawnPoint();

			EnemyType enemy;

			switch(level)
			{
				case 1:
					enemy = new Enemy_1_SmallGreenBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 2:
					enemy = new Enemy_2_SmallOrangeBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 3:
					enemy = new Enemy_3_SmallBrownBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 4:
					enemy = new Enemy_4_SmallBlueBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 5:
					enemy = new Enemy_5_SmallRedBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 6:
					enemy = new Enemy_6_SmallPurpleBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 7:
					enemy = new Enemy_7_SmallWhiteBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 8:
					enemy = new Enemy_8_SmallBlackBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 9:
					enemy = new Enemy_9_SmallRainbowBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
					
				case 10:
					enemy = new Enemy_10_BigGreenBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 11:
					enemy = new Enemy_11_BigOrangeBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 12:
					enemy = new Enemy_12_BigBrownBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 13:
					enemy = new Enemy_13_BigBlueBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 14:
					enemy = new Enemy_14_BigRedBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 15:
					enemy = new Enemy_15_BigPurpleBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 16:
					enemy = new Enemy_16_BigWhiteBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 17:
					enemy = new Enemy_17_BigBlackBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;
				case 18:
					enemy = new Enemy_18_BigRainbowBox(this.Manager, spawnPoint.X, spawnPoint.Y);
					break;

				default:
					return false;
			}

			for (int index = 0; index < this.MaxCount; index ++)
			{
				if (this.Enemies[index] == null)
				{
					this.Enemies[index] = enemy;
					this.Count ++;
					return true;
				}
			}

			return false;
		}

		public void Update(float deltaRatio)
		{
			// Update Enemies
			for (int index = 0; index < this.MaxCount; index ++)
			{
				if (this.Enemies[index] != null)
				{
					if (this.Enemies[index].IsDisabled())
					{
						this.Enemies[index] = null;
						this.Count --;
					}
					else
					{
						this.Enemies[index].Update(deltaRatio);
					}
				}
			}
			
			#region Bullet Collision Detection
			
			for (int index = 0; index < this.MaxCount; index ++)
			{
				if (this.Enemies[index] != null)
				{
					// Collision Checking
					EnemyType enemy = this.Enemies[index];
					int bulletMaxCount = this.Manager.GamePlayerBulletList.MaxCount;

					for (int bulletIndex = 0; bulletIndex < bulletMaxCount; bulletIndex ++)
					{
						PlayerBullet bullet = this.Manager.GamePlayerBulletList.Bullets[bulletIndex];

						if (bullet == null)
						{
							continue;
						}
						
						//if (bullet.IsHit == true)
						//{
						//	continue;
						//}

						Vector2D enemyPosition = enemy.GetPosition();

						bool isCollideRapidly = this.Manager.CheckCollisionRapidly(enemyPosition.X, enemyPosition.Y,
							enemy.GetWidth(), enemy.GetWidth(), bullet.GetStartVector(), bullet.GetEndVector());

						if (isCollideRapidly)
						{
							Vector2D detectionPositioin = this.Manager.CheckCollisionPrecise(enemyPosition.X, enemyPosition.Y,
							enemy.GetWidth(), enemy.GetWidth(), bullet.GetStartVector(), bullet.GetEndVector());

							if (detectionPositioin != null)
							{
								enemy.Hit(bullet.Damage);
								bullet.SetHit((int)detectionPositioin.X, (int)detectionPositioin.Y);
							}
						}
					}
				}
			}

			#endregion

			#region Push Each Others

			List<int> checkArray = new List<int>();

			for (int index = 0; index < this.MaxCount; index ++)
			{
				if (this.Enemies[index] == null)
				{
					continue;
				}

				if (checkArray.Contains(index))
				{
					continue;
				}

				for (int check = 0; check < this.MaxCount; check ++)
				{
					
					if (this.Enemies[check] == null)
					{
						continue;
					}

					if (index == check)
					{
						continue;
					}

					if (checkArray.Contains(check))
					{
						continue;
					}

					EnemyType baseEnemy  = this.Enemies[index];
					EnemyType checkEnemy = this.Enemies[check];

					Vector2D basePosition  = baseEnemy.GetPosition();
					Vector2D checkPosition = checkEnemy.GetPosition();
					Vector2D distance = basePosition - checkPosition;

					if ((Math.Abs(distance.X) < ((baseEnemy.GetWidth()  + checkEnemy.GetWidth())  / 2)) &&
						(Math.Abs(distance.Y) < ((baseEnemy.GetHeight() + checkEnemy.GetHeight()) / 2)))
					{
						checkArray.Add(index);
						checkArray.Add(check);
						
						Vector2D pushAmount = distance * PushEachOtherAmount;
						baseEnemy.Move(pushAmount.X, pushAmount.Y);
						checkEnemy.Move(-pushAmount.X, -pushAmount.Y);

						break;
					}
				}
			}

			#endregion
		}

		public void Draw(Graphics graphics)
		{
			for (int index = 0; index < this.MaxCount; index ++)
			{
				if (this.Enemies[index] != null)
				{
					this.Enemies[index].Draw(graphics);
				}
			}
		}

		public bool IsEmpty()
		{
			if (this.Count <= 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
