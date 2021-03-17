using CenterDefenceGame.GameObject.EnemyObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.LevelObject
{
	public class LevelManager
	{
		private GameManager Manager;

		public int[,] EnemySpawnGridIni;
		public int[,] EnemySpawnGrid;
		public int[] EnemyCount;

		// 변경되면 데이터도 수정해야함
		private const int MAX_WAVE = 31;
		private int WaveTime;
		public int SpawnDelay = 0;
		private bool IsSpawnFinished;

		public LevelManager(GameManager gameManager)
		{
			this.Manager = gameManager;

			this.WaveTime = this.Manager.WaveTimerMax;

			EnemySpawnGridIni = new int[MAX_WAVE, 19]{
			//  0       1       2       3       4       5       6       7       8       9      10      11      12      13      14      15      16      17      18
			{   0   ,   15  ,   5   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   3   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   20  ,   10  ,   1   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   3   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   25  ,   15  ,   5   ,   1   ,   0   ,   0   ,   0   ,   0   ,   0   ,   3   ,   3   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   10  ,   2   ,   0   ,   0   ,   0   ,   0   ,   0   ,   5   ,   5   ,   2   ,   1   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   5   ,   0   ,   0   ,   0   ,   0   ,   0   ,   5   ,   5   ,   3   ,   1   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   10  ,   0   ,   0   ,   0   ,   0   ,   0   ,   5   ,   5   ,   5   ,   1   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   15  ,   1   ,   0   ,   0   ,   0   ,   0   ,   5   ,   5   ,   5   ,   2   ,   0   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   3   ,   0   ,   0   ,   0   ,   0   ,   5   ,   5   ,   5   ,   2   ,   1   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   5   ,   0   ,   0   ,   0   ,   0   ,   7   ,   5   ,   6   ,   2   ,   2   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   5   ,   0   ,   0   ,   0   ,   0   ,   7   ,   5   ,   6   ,   3   ,   2   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   10  ,   0   ,   0   ,   0   ,   0   ,   7   ,   5   ,   6   ,   3   ,   2   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   10  ,   1   ,   0   ,   0   ,   0   ,   7   ,   7   ,   6   ,   4   ,   3   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   15  ,   3   ,   0   ,   0   ,   0   ,   7   ,   7   ,   7   ,   4   ,   3   ,   0   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   15  ,   3   ,   0   ,   0   ,   0   ,   7   ,   7   ,   7   ,   5   ,   3   ,   2   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   20  ,   5   ,   1   ,   0   ,   0   ,   10  ,   7   ,   7   ,   5   ,   3   ,   3   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   20  ,   5   ,   2   ,   0   ,   0   ,   10  ,   10  ,   7   ,   7   ,   4   ,   3   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   25  ,   5   ,   3   ,   0   ,   0   ,   10  ,   10  ,   10  ,   7   ,   4   ,   3   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   25  ,   10  ,   4   ,   0   ,   0   ,   10  ,   10  ,   10  ,   9   ,   4   ,   3   ,   0   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   30  ,   10  ,   5   ,   0   ,   0   ,   10  ,   10  ,   10  ,   9   ,   4   ,   3   ,   1   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   30  ,   10  ,   5   ,   1   ,   0   ,   15  ,   10  ,   10  ,   11  ,   7   ,   3   ,   2   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   30  ,   15  ,   5   ,   2   ,   0   ,   15  ,   15  ,   15  ,   11  ,   7   ,   3   ,   3   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   35  ,   15  ,   5   ,   3   ,   0   ,   15  ,   15  ,   15  ,   11  ,   7   ,   3   ,   3   ,   0   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   35  ,   15  ,   5   ,   4   ,   0   ,   15  ,   15  ,   15  ,   11  ,   7   ,   3   ,   3   ,   1   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   35  ,   15  ,   5   ,   5   ,   1   ,   15  ,   15  ,   15  ,   13  ,   10  ,   3   ,   3   ,   2   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   40  ,   20  ,   5   ,   5   ,   2   ,   15  ,   15  ,   15  ,   13  ,   10  ,   3   ,   3   ,   3   ,   0   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   40  ,   20  ,   5   ,   5   ,   3   ,   15  ,   15  ,   20  ,   13  ,   10  ,   3   ,   3   ,   3   ,   1   }   ,
			{   0   ,   30  ,   20  ,   15  ,   20  ,   40  ,   20  ,   5   ,   5   ,   3   ,   15  ,   15  ,   20  ,   13  ,   10  ,   4   ,   3   ,   3   ,   2   }   ,
			{   0   ,   30  ,   20  ,   15  ,   25  ,   45  ,   25  ,   5   ,   5   ,   3   ,   15  ,   15  ,   20  ,   13  ,   13  ,   4   ,   4   ,   4   ,   3   }   ,
			{   0   ,   30  ,   20  ,   15  ,   30  ,   50  ,   25  ,   5   ,   5   ,   5   ,   15  ,   15  ,   20  ,   15  ,   13  ,   6   ,   6   ,   5   ,   4   }   ,
			{   0   ,   30  ,   20  ,   15  ,   35  ,   55  ,   25  ,   5   ,   5   ,   5   ,   15  ,   15  ,   20  ,   15  ,   13  ,   8   ,   8   ,   6   ,   5   }   ,
			{   0   ,   30  ,   20  ,   15  ,   40  ,   60  ,   30  ,   15  ,   15  ,   15  ,   15  ,   15  ,   20  ,   15  ,   13  ,   10  ,   10  ,   7   ,   6   }
			};
			EnemySpawnGrid = new int[MAX_WAVE, 19];
			EnemyCount = new int[MAX_WAVE];

			this.Reset();
		}

		public void Reset()
		{
			for (int wave = 0; wave < MAX_WAVE; wave ++)
			{
				int count = 0;

				for (int type = 0; type < 19; type ++)
				{
					EnemySpawnGrid[wave, type] = EnemySpawnGridIni[wave, type];
					count += EnemySpawnGrid[wave, type];
				}

				EnemyCount[wave] = count;
			}

			this.IsSpawnFinished = false;
			this.SpawnDelay = 0;
		}

		public bool IsSpawnComplete()
		{
			return this.IsSpawnFinished;
		}

		public int GetEnemyLeft()
		{
			int wave = Manager.GetWave();
			if (wave > 30)
			{
				return 0;
			}
			else
			{
				return EnemyCount[wave];
			}
		}

		public void Update()
		{
			int wave = this.Manager.GetWave();

			if (this.Manager.GetGameSituation() != ESystemSituation.Wave)
			{
				this.IsSpawnFinished = false;
				return;
			}

			if (EnemyCount[wave] == 0)
			{
				this.IsSpawnFinished = true;
				return;
			}
			else
			{
				this.IsSpawnFinished = false;
			}

			if (this.SpawnDelay > 0)
			{
				this.SpawnDelay--;
			}
			else
			{
				int currentWaveEnemyCount = EnemyCount[wave];

				int deadLimit = 1200;
				int leftTime = Manager.GetWaveTime() - deadLimit;

				if (currentWaveEnemyCount != 0)
				{
					if (leftTime > currentWaveEnemyCount)
					{
						SpawnDelay = leftTime / currentWaveEnemyCount;
					}
					else
					{
						SpawnDelay = (deadLimit - 100) / currentWaveEnemyCount;
						if (SpawnDelay < 0)
						{
							SpawnDelay = 0;
						}
					}
				}

				// 남아있는 수 이하의 난수를 생성한후 앞에서부터 남은 타입의 적기를 참조해서 생성
				// 예) 난수 = 200
				// 남은 적
				// 적1 70 | 적2 120 | 적3 30
				// 적3을 스폰 (200번대에 해당)
				Random rand = new Random();

				int typeLeftCounter = rand.Next(currentWaveEnemyCount + 1);

				int typeSelected = 0;

				for (int type = 0; type < 19; type ++)
				{
					int leftType = EnemySpawnGrid[wave, type];

					if (leftType != 0)
					{
						typeLeftCounter -= leftType;

						if (typeLeftCounter <= 0)
						{
							typeSelected = type;
							break;
						}
					}
				}
				
				if (typeSelected != 0)
				{
					if (this.SpawnEnemy(typeSelected))
					{
						EnemySpawnGrid[wave, typeSelected] --;
						EnemyCount[wave] --;
					}
				}
			}
		}

		public bool SpawnEnemy(int level)
		{
			return this.Manager.GameEnemyList.CreateEnemy(level);
		}
	}
}
