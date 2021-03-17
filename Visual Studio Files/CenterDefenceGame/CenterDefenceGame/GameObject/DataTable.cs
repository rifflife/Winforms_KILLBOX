using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject
{
	public static class DataTable
	{
		#region Data Table

		public static readonly int InitializeResource = 700;

		// Enemy
		public static readonly int[,] EnemyDataTable = new int[20, 7];

		// Player
		public static readonly int[,] PlayerDataTable = new int[50, 50];

		// Tutorial String
		public static readonly string[] TutorialDataTable = new string[50];

		// Wave
		public static readonly int WaveTimerMax = 60 * 60 + 59;
		public static readonly int EndWave = 30;
		public static readonly int GameOverDelayMax = 60 * 2;

		// Block Price
		public static readonly int PriceWall = 25;
		public static readonly int PriceObstacle = 10;
		public static readonly int PriceAmmunitionGenerator = 30;
		public static readonly int PriceResourceGenerator = 20;

		// Block
		public static readonly int CoreAmmoCount = 50;
		public static readonly int CoreAmmoGeneratorAmount = 20;
		public static readonly int AmmunitionGeneratorAmmoCount = 5;

		public static readonly int ResourceGeneratorMakeAmount = 10;
		public static readonly int CoreResourceMakeAmount = 40;

		public static readonly int WallHealthPoint = 150;
		public static readonly int ObstacleHealthPoint = 120;
		public static readonly int ResourceGeneratorHealthPoint = 60;
		public static readonly int AmmunitionGeneratorHealthPoint = 60;

		#endregion

		#region Weapon

		public static readonly float MaxRecoil = (float)((Math.PI / 1800) * 200); // 20도 + 20도 한쪽 각도만 계산
		public static readonly float DecreaseRecoil = (float)((Math.PI / 1800) * 5); // 반동 계수 5
		public static readonly float LeftWeaponFireWidth = 6f; // 반동 계수 5

		public static readonly int[,] LeftWeaponFireRate;
		public static readonly int[,] LeftWeaponDamage;
		public static readonly int[,] LeftWeaponExtension;
		public static readonly int[,] LeftWeaponRecoil;

		public static readonly float RightWeaponMinFireAngle = (float)((Math.PI / 180) * 10); // 최소 발사각

		public static readonly int[,] RightWeaponFireRate;
		public static readonly int[,] RightWeaponDamage;
		public static readonly int[,] RightWeaponExtension;
		public static readonly int[,] RightWeaponRecoil;

		#endregion

		#region Core and Player

		public static readonly int[,] CoreHealth;
		public static readonly int[,] CoreRegen;
		public static readonly int[,] CoreGenerateAmmunitionMakeTime;
		public static readonly int[,] CoreGenerateResourceRatio;

		public static readonly int[,] PlayerHealth;
		public static readonly int[,] PlayerRegen;
		public static readonly int[,] CoreAmmoHasRatio;
		public static readonly int[,] PlayerRebornTime;

		#endregion

		static DataTable()
		{
			#region Enemy Data

			// 쉬  움 //  Core              // 벽을 제외한 코어를 향한다.
			// 보  통 //  AllExceptWall     // 벽을 제외한 모든 구조물을 향한다.
			// 어려움 //  All               // 모든 구조물을 향한다.
			// 쉬  움 //  Player            // 벽과 구조물을 제외한 공간의 플레이어를 향한다.
			// 쉬  움 //  PlayerIgnoreBlock // 벽을 제외한 구조물을 모두 무시하고 플레이어를 향한다.
			// 어려움 //  Center            // 다 무시하고 중앙으로 간다. (코어 위치)

			//    Speed  Helath   Delay   Reach   Damage  Drop     PathFinding
			SetEnemyData(0, 0, 1, 10, 1, 1, 1, EGoDirection.All);
			SetEnemyData(1, 3, 15, 60, 25, 4, 5, EGoDirection.Player);
			SetEnemyData(2, 2, 20, 80, 25, 8, 7, EGoDirection.Core);
			SetEnemyData(3, 2, 25, 80, 25, 8, 10, EGoDirection.AllExceptWall);
			SetEnemyData(4, 3, 30, 40, 25, 7, 15, EGoDirection.Core);
			SetEnemyData(5, 4, 75, 20, 35, 5, 20, EGoDirection.AllExceptWall);
			SetEnemyData(6, 6, 100, 10, 35, 5, 30, EGoDirection.All);
			SetEnemyData(7, 8, 150, 8, 25, 5, 40, EGoDirection.PlayerIgnoreBlock);
			SetEnemyData(8, 3, 500, 150, 25, 150, 60, EGoDirection.Core);
			SetEnemyData(9, 9, 300, 3, 35, 5, 100, EGoDirection.All);
			SetEnemyData(10, 3, 40, 80, 50, 10, 15, EGoDirection.Player);
			SetEnemyData(11, 2, 60, 100, 50, 20, 20, EGoDirection.Core);
			SetEnemyData(12, 2, 80, 100, 50, 20, 25, EGoDirection.AllExceptWall);
			SetEnemyData(13, 2, 100, 60, 50, 20, 40, EGoDirection.Core);
			SetEnemyData(14, 3, 150, 30, 50, 15, 50, EGoDirection.AllExceptWall);
			SetEnemyData(15, 5, 300, 20, 50, 20, 80, EGoDirection.All);
			SetEnemyData(16, 7, 400, 13, 50, 20, 100, EGoDirection.PlayerIgnoreBlock);
			SetEnemyData(17, 3, 1000, 150, 50, 300, 200, EGoDirection.Core);
			SetEnemyData(18, 9, 700, 6, 50, 18, 300, EGoDirection.All);


			#endregion
			// Player Data										
			// 상단은 값 하단은 가격										
			// 가격 0번째는 한계값										

			#region DATA BINDING

			#region LeftWeaponFireRate												

			LeftWeaponFireRate = new int[2, 13];

			LeftWeaponFireRate[0, 0] = 20;
			LeftWeaponFireRate[0, 1] = 15;
			LeftWeaponFireRate[0, 2] = 12;
			LeftWeaponFireRate[0, 3] = 10;
			LeftWeaponFireRate[0, 4] = 9;
			LeftWeaponFireRate[0, 5] = 8;
			LeftWeaponFireRate[0, 6] = 7;
			LeftWeaponFireRate[0, 7] = 6;
			LeftWeaponFireRate[0, 8] = 5;
			LeftWeaponFireRate[0, 9] = 4;
			LeftWeaponFireRate[0, 10] = 3;
			LeftWeaponFireRate[0, 11] = 2;
			LeftWeaponFireRate[0, 12] = 1;

			LeftWeaponFireRate[1, 0] = 13;
			LeftWeaponFireRate[1, 1] = 150;
			LeftWeaponFireRate[1, 2] = 200;
			LeftWeaponFireRate[1, 3] = 300;
			LeftWeaponFireRate[1, 4] = 500;
			LeftWeaponFireRate[1, 5] = 700;
			LeftWeaponFireRate[1, 6] = 1000;
			LeftWeaponFireRate[1, 7] = 1500;
			LeftWeaponFireRate[1, 8] = 2000;
			LeftWeaponFireRate[1, 9] = 2500;
			LeftWeaponFireRate[1, 10] = 3000;
			LeftWeaponFireRate[1, 11] = 5000;
			LeftWeaponFireRate[1, 12] = 7000;

			#endregion

			#region LeftWeaponDamage												

			// 2번은 탄속												
			LeftWeaponDamage = new int[3, 26];

			LeftWeaponDamage[0, 0] = 5;
			LeftWeaponDamage[0, 1] = 6;
			LeftWeaponDamage[0, 2] = 7;
			LeftWeaponDamage[0, 3] = 8;
			LeftWeaponDamage[0, 4] = 9;
			LeftWeaponDamage[0, 5] = 10;
			LeftWeaponDamage[0, 6] = 11;
			LeftWeaponDamage[0, 7] = 12;
			LeftWeaponDamage[0, 8] = 13;
			LeftWeaponDamage[0, 9] = 14;
			LeftWeaponDamage[0, 10] = 15;
			LeftWeaponDamage[0, 11] = 16;
			LeftWeaponDamage[0, 12] = 17;
			LeftWeaponDamage[0, 13] = 18;
			LeftWeaponDamage[0, 14] = 19;
			LeftWeaponDamage[0, 15] = 20;
			LeftWeaponDamage[0, 16] = 22;
			LeftWeaponDamage[0, 17] = 24;
			LeftWeaponDamage[0, 18] = 26;
			LeftWeaponDamage[0, 19] = 28;
			LeftWeaponDamage[0, 20] = 30;
			LeftWeaponDamage[0, 21] = 33;
			LeftWeaponDamage[0, 22] = 36;
			LeftWeaponDamage[0, 23] = 40;
			LeftWeaponDamage[0, 24] = 50;
			LeftWeaponDamage[0, 25] = 70;

			LeftWeaponDamage[1, 0] = 26;
			LeftWeaponDamage[1, 1] = 50;
			LeftWeaponDamage[1, 2] = 70;
			LeftWeaponDamage[1, 3] = 100;
			LeftWeaponDamage[1, 4] = 130;
			LeftWeaponDamage[1, 5] = 160;
			LeftWeaponDamage[1, 6] = 200;
			LeftWeaponDamage[1, 7] = 240;
			LeftWeaponDamage[1, 8] = 280;
			LeftWeaponDamage[1, 9] = 320;
			LeftWeaponDamage[1, 10] = 360;
			LeftWeaponDamage[1, 11] = 400;
			LeftWeaponDamage[1, 12] = 450;
			LeftWeaponDamage[1, 13] = 500;
			LeftWeaponDamage[1, 14] = 600;
			LeftWeaponDamage[1, 15] = 700;
			LeftWeaponDamage[1, 16] = 800;
			LeftWeaponDamage[1, 17] = 900;
			LeftWeaponDamage[1, 18] = 1000;
			LeftWeaponDamage[1, 19] = 1200;
			LeftWeaponDamage[1, 20] = 1400;
			LeftWeaponDamage[1, 21] = 1600;
			LeftWeaponDamage[1, 22] = 1800;
			LeftWeaponDamage[1, 23] = 2000;
			LeftWeaponDamage[1, 24] = 3000;
			LeftWeaponDamage[1, 25] = 4000;

			LeftWeaponDamage[2, 0] = 8;
			LeftWeaponDamage[2, 1] = 8;
			LeftWeaponDamage[2, 2] = 9;
			LeftWeaponDamage[2, 3] = 9;
			LeftWeaponDamage[2, 4] = 10;
			LeftWeaponDamage[2, 5] = 10;
			LeftWeaponDamage[2, 6] = 11;
			LeftWeaponDamage[2, 7] = 11;
			LeftWeaponDamage[2, 8] = 12;
			LeftWeaponDamage[2, 9] = 12;
			LeftWeaponDamage[2, 10] = 13;
			LeftWeaponDamage[2, 11] = 13;
			LeftWeaponDamage[2, 12] = 14;
			LeftWeaponDamage[2, 13] = 14;
			LeftWeaponDamage[2, 14] = 15;
			LeftWeaponDamage[2, 15] = 15;
			LeftWeaponDamage[2, 16] = 16;
			LeftWeaponDamage[2, 17] = 16;
			LeftWeaponDamage[2, 18] = 17;
			LeftWeaponDamage[2, 19] = 17;
			LeftWeaponDamage[2, 20] = 18;
			LeftWeaponDamage[2, 21] = 18;
			LeftWeaponDamage[2, 22] = 19;
			LeftWeaponDamage[2, 23] = 19;
			LeftWeaponDamage[2, 24] = 20;
			LeftWeaponDamage[2, 25] = 20;

			#endregion

			#region LeftWeaponExtension												

			LeftWeaponExtension = new int[2, 5];

			LeftWeaponExtension[0, 0] = 1;
			LeftWeaponExtension[0, 1] = 2;
			LeftWeaponExtension[0, 2] = 3;
			LeftWeaponExtension[0, 3] = 4;
			LeftWeaponExtension[0, 4] = 5;

			LeftWeaponExtension[1, 0] = 5;
			LeftWeaponExtension[1, 1] = 1000;
			LeftWeaponExtension[1, 2] = 2000;
			LeftWeaponExtension[1, 3] = 3000;
			LeftWeaponExtension[1, 4] = 4000;

			#endregion

			#region LeftWeaponRecoil												

			LeftWeaponRecoil = new int[2, 8];

			LeftWeaponRecoil[0, 0] = 150;
			LeftWeaponRecoil[0, 1] = 130;
			LeftWeaponRecoil[0, 2] = 110;
			LeftWeaponRecoil[0, 3] = 90;
			LeftWeaponRecoil[0, 4] = 70;
			LeftWeaponRecoil[0, 5] = 50;
			LeftWeaponRecoil[0, 6] = 30;
			LeftWeaponRecoil[0, 7] = 10;

			LeftWeaponRecoil[1, 0] = 8;
			LeftWeaponRecoil[1, 1] = 100;
			LeftWeaponRecoil[1, 2] = 250;
			LeftWeaponRecoil[1, 3] = 500;
			LeftWeaponRecoil[1, 4] = 750;
			LeftWeaponRecoil[1, 5] = 1000;
			LeftWeaponRecoil[1, 6] = 1500;
			LeftWeaponRecoil[1, 7] = 2000;

			#endregion

			#region RightWeaponFireRate												

			RightWeaponFireRate = new int[2, 14];

			RightWeaponFireRate[0, 0] = 90;
			RightWeaponFireRate[0, 1] = 80;
			RightWeaponFireRate[0, 2] = 70;
			RightWeaponFireRate[0, 3] = 60;
			RightWeaponFireRate[0, 4] = 50;
			RightWeaponFireRate[0, 5] = 42;
			RightWeaponFireRate[0, 6] = 35;
			RightWeaponFireRate[0, 7] = 30;
			RightWeaponFireRate[0, 8] = 25;
			RightWeaponFireRate[0, 9] = 20;
			RightWeaponFireRate[0, 10] = 17;
			RightWeaponFireRate[0, 11] = 15;
			RightWeaponFireRate[0, 12] = 10;
			RightWeaponFireRate[0, 13] = 5;

			RightWeaponFireRate[1, 0] = 14;
			RightWeaponFireRate[1, 1] = 100;
			RightWeaponFireRate[1, 2] = 150;
			RightWeaponFireRate[1, 3] = 200;
			RightWeaponFireRate[1, 4] = 300;
			RightWeaponFireRate[1, 5] = 500;
			RightWeaponFireRate[1, 6] = 1000;
			RightWeaponFireRate[1, 7] = 1500;
			RightWeaponFireRate[1, 8] = 2000;
			RightWeaponFireRate[1, 9] = 3000;
			RightWeaponFireRate[1, 10] = 5000;
			RightWeaponFireRate[1, 11] = 8000;
			RightWeaponFireRate[1, 12] = 15000;
			RightWeaponFireRate[1, 13] = 30000;

			#endregion

			#region RightWeaponDamage												

			RightWeaponDamage = new int[3, 17];

			RightWeaponDamage[0, 0] = 5;
			RightWeaponDamage[0, 1] = 7;
			RightWeaponDamage[0, 2] = 9;
			RightWeaponDamage[0, 3] = 11;
			RightWeaponDamage[0, 4] = 13;
			RightWeaponDamage[0, 5] = 15;
			RightWeaponDamage[0, 6] = 17;
			RightWeaponDamage[0, 7] = 20;
			RightWeaponDamage[0, 8] = 25;
			RightWeaponDamage[0, 9] = 30;
			RightWeaponDamage[0, 10] = 35;
			RightWeaponDamage[0, 11] = 40;
			RightWeaponDamage[0, 12] = 50;
			RightWeaponDamage[0, 13] = 60;
			RightWeaponDamage[0, 14] = 70;
			RightWeaponDamage[0, 15] = 80;
			RightWeaponDamage[0, 16] = 100;

			RightWeaponDamage[1, 0] = 17;
			RightWeaponDamage[1, 1] = 100;
			RightWeaponDamage[1, 2] = 150;
			RightWeaponDamage[1, 3] = 200;
			RightWeaponDamage[1, 4] = 300;
			RightWeaponDamage[1, 5] = 400;
			RightWeaponDamage[1, 6] = 500;
			RightWeaponDamage[1, 7] = 700;
			RightWeaponDamage[1, 8] = 1000;
			RightWeaponDamage[1, 9] = 1400;
			RightWeaponDamage[1, 10] = 1800;
			RightWeaponDamage[1, 11] = 2400;
			RightWeaponDamage[1, 12] = 3000;
			RightWeaponDamage[1, 13] = 3000;
			RightWeaponDamage[1, 14] = 3000;
			RightWeaponDamage[1, 15] = 3000;
			RightWeaponDamage[1, 16] = 4000;

			RightWeaponDamage[2, 0] = 6;
			RightWeaponDamage[2, 1] = 6;
			RightWeaponDamage[2, 2] = 7;
			RightWeaponDamage[2, 3] = 7;
			RightWeaponDamage[2, 4] = 8;
			RightWeaponDamage[2, 5] = 8;
			RightWeaponDamage[2, 6] = 9;
			RightWeaponDamage[2, 7] = 9;
			RightWeaponDamage[2, 8] = 10;
			RightWeaponDamage[2, 9] = 10;
			RightWeaponDamage[2, 10] = 11;
			RightWeaponDamage[2, 11] = 11;
			RightWeaponDamage[2, 12] = 12;
			RightWeaponDamage[2, 13] = 12;
			RightWeaponDamage[2, 14] = 13;
			RightWeaponDamage[2, 15] = 13;
			RightWeaponDamage[2, 16] = 15;

			#endregion

			#region RightWeaponExtension												

			RightWeaponExtension = new int[2, 10];

			RightWeaponExtension[0, 0] = 3;
			RightWeaponExtension[0, 1] = 4;
			RightWeaponExtension[0, 2] = 5;
			RightWeaponExtension[0, 3] = 6;
			RightWeaponExtension[0, 4] = 7;
			RightWeaponExtension[0, 5] = 8;
			RightWeaponExtension[0, 6] = 9;
			RightWeaponExtension[0, 7] = 10;
			RightWeaponExtension[0, 8] = 11;
			RightWeaponExtension[0, 9] = 12;

			RightWeaponExtension[1, 0] = 10;
			RightWeaponExtension[1, 1] = 200;
			RightWeaponExtension[1, 2] = 400;
			RightWeaponExtension[1, 3] = 700;
			RightWeaponExtension[1, 4] = 1000;
			RightWeaponExtension[1, 5] = 1500;
			RightWeaponExtension[1, 6] = 2000;
			RightWeaponExtension[1, 7] = 2500;
			RightWeaponExtension[1, 8] = 3000;
			RightWeaponExtension[1, 9] = 4000;

			#endregion

			#region RightWeaponRecoil												

			RightWeaponRecoil = new int[2, 8];

			RightWeaponRecoil[0, 0] = 350;
			RightWeaponRecoil[0, 1] = 300;
			RightWeaponRecoil[0, 2] = 250;
			RightWeaponRecoil[0, 3] = 200;
			RightWeaponRecoil[0, 4] = 150;
			RightWeaponRecoil[0, 5] = 100;
			RightWeaponRecoil[0, 6] = 50;
			RightWeaponRecoil[0, 7] = 10;

			RightWeaponRecoil[1, 0] = 8;
			RightWeaponRecoil[1, 1] = 100;
			RightWeaponRecoil[1, 2] = 250;
			RightWeaponRecoil[1, 3] = 500;
			RightWeaponRecoil[1, 4] = 750;
			RightWeaponRecoil[1, 5] = 1000;
			RightWeaponRecoil[1, 6] = 1500;
			RightWeaponRecoil[1, 7] = 2000;

			#endregion

			#region CoreHealth												

			CoreHealth = new int[2, 15];

			CoreHealth[0, 0] = 500;
			CoreHealth[0, 1] = 600;
			CoreHealth[0, 2] = 700;
			CoreHealth[0, 3] = 800;
			CoreHealth[0, 4] = 900;
			CoreHealth[0, 5] = 1000;
			CoreHealth[0, 6] = 1200;
			CoreHealth[0, 7] = 1400;
			CoreHealth[0, 8] = 1600;
			CoreHealth[0, 9] = 1800;
			CoreHealth[0, 10] = 2000;
			CoreHealth[0, 11] = 2500;
			CoreHealth[0, 12] = 3000;
			CoreHealth[0, 13] = 3500;
			CoreHealth[0, 14] = 4000;

			CoreHealth[1, 0] = 15;
			CoreHealth[1, 1] = 100;
			CoreHealth[1, 2] = 150;
			CoreHealth[1, 3] = 200;
			CoreHealth[1, 4] = 250;
			CoreHealth[1, 5] = 300;
			CoreHealth[1, 6] = 350;
			CoreHealth[1, 7] = 400;
			CoreHealth[1, 8] = 500;
			CoreHealth[1, 9] = 700;
			CoreHealth[1, 10] = 1000;
			CoreHealth[1, 11] = 1500;
			CoreHealth[1, 12] = 2000;
			CoreHealth[1, 13] = 2500;
			CoreHealth[1, 14] = 3000;

			#endregion

			#region CoreRegen												

			CoreRegen = new int[2, 15];

			CoreRegen[0, 0] = 0;
			CoreRegen[0, 1] = 2;
			CoreRegen[0, 2] = 4;
			CoreRegen[0, 3] = 6;
			CoreRegen[0, 4] = 8;
			CoreRegen[0, 5] = 10;
			CoreRegen[0, 6] = 12;
			CoreRegen[0, 7] = 14;
			CoreRegen[0, 8] = 16;
			CoreRegen[0, 9] = 18;
			CoreRegen[0, 10] = 20;
			CoreRegen[0, 11] = 24;
			CoreRegen[0, 12] = 28;
			CoreRegen[0, 13] = 32;
			CoreRegen[0, 14] = 40;

			CoreRegen[1, 0] = 15;
			CoreRegen[1, 1] = 100;
			CoreRegen[1, 2] = 150;
			CoreRegen[1, 3] = 200;
			CoreRegen[1, 4] = 250;
			CoreRegen[1, 5] = 300;
			CoreRegen[1, 6] = 350;
			CoreRegen[1, 7] = 400;
			CoreRegen[1, 8] = 500;
			CoreRegen[1, 9] = 650;
			CoreRegen[1, 10] = 800;
			CoreRegen[1, 11] = 1000;
			CoreRegen[1, 12] = 1500;
			CoreRegen[1, 13] = 2000;
			CoreRegen[1, 14] = 3000;

			#endregion

			#region CoreGenerateAmmunitionMakeTime												

			CoreGenerateAmmunitionMakeTime = new int[2, 9];

			CoreGenerateAmmunitionMakeTime[0, 0] = 50;
			CoreGenerateAmmunitionMakeTime[0, 1] = 45;
			CoreGenerateAmmunitionMakeTime[0, 2] = 40;
			CoreGenerateAmmunitionMakeTime[0, 3] = 35;
			CoreGenerateAmmunitionMakeTime[0, 4] = 30;
			CoreGenerateAmmunitionMakeTime[0, 5] = 25;
			CoreGenerateAmmunitionMakeTime[0, 6] = 20;
			CoreGenerateAmmunitionMakeTime[0, 7] = 15;
			CoreGenerateAmmunitionMakeTime[0, 8] = 10;

			CoreGenerateAmmunitionMakeTime[1, 0] = 9;
			CoreGenerateAmmunitionMakeTime[1, 1] = 200;
			CoreGenerateAmmunitionMakeTime[1, 2] = 400;
			CoreGenerateAmmunitionMakeTime[1, 3] = 700;
			CoreGenerateAmmunitionMakeTime[1, 4] = 1000;
			CoreGenerateAmmunitionMakeTime[1, 5] = 1500;
			CoreGenerateAmmunitionMakeTime[1, 6] = 2000;
			CoreGenerateAmmunitionMakeTime[1, 7] = 2500;
			CoreGenerateAmmunitionMakeTime[1, 8] = 3000;

			#endregion

			#region CoreGenerateResourceRatio												

			CoreGenerateResourceRatio = new int[2, 9];

			CoreGenerateResourceRatio[0, 0] = 0;
			CoreGenerateResourceRatio[0, 1] = 20;
			CoreGenerateResourceRatio[0, 2] = 40;
			CoreGenerateResourceRatio[0, 3] = 60;
			CoreGenerateResourceRatio[0, 4] = 80;
			CoreGenerateResourceRatio[0, 5] = 100;
			CoreGenerateResourceRatio[0, 6] = 120;
			CoreGenerateResourceRatio[0, 7] = 160;
			CoreGenerateResourceRatio[0, 8] = 200;

			CoreGenerateResourceRatio[1, 0] = 9;
			CoreGenerateResourceRatio[1, 1] = 200;
			CoreGenerateResourceRatio[1, 2] = 400;
			CoreGenerateResourceRatio[1, 3] = 700;
			CoreGenerateResourceRatio[1, 4] = 1000;
			CoreGenerateResourceRatio[1, 5] = 1400;
			CoreGenerateResourceRatio[1, 6] = 1800;
			CoreGenerateResourceRatio[1, 7] = 2200;
			CoreGenerateResourceRatio[1, 8] = 2600;

			#endregion

			#region PlayerHealth												

			PlayerHealth = new int[2, 15];

			PlayerHealth[0, 0] = 50;
			PlayerHealth[0, 1] = 75;
			PlayerHealth[0, 2] = 100;
			PlayerHealth[0, 3] = 125;
			PlayerHealth[0, 4] = 150;
			PlayerHealth[0, 5] = 175;
			PlayerHealth[0, 6] = 200;
			PlayerHealth[0, 7] = 225;
			PlayerHealth[0, 8] = 250;
			PlayerHealth[0, 9] = 275;
			PlayerHealth[0, 10] = 300;
			PlayerHealth[0, 11] = 350;
			PlayerHealth[0, 12] = 400;
			PlayerHealth[0, 13] = 450;
			PlayerHealth[0, 14] = 500;

			PlayerHealth[1, 0] = 15;
			PlayerHealth[1, 1] = 100;
			PlayerHealth[1, 2] = 150;
			PlayerHealth[1, 3] = 200;
			PlayerHealth[1, 4] = 250;
			PlayerHealth[1, 5] = 300;
			PlayerHealth[1, 6] = 350;
			PlayerHealth[1, 7] = 400;
			PlayerHealth[1, 8] = 500;
			PlayerHealth[1, 9] = 600;
			PlayerHealth[1, 10] = 700;
			PlayerHealth[1, 11] = 800;
			PlayerHealth[1, 12] = 900;
			PlayerHealth[1, 13] = 1000;
			PlayerHealth[1, 14] = 1500;

			#endregion

			#region PlayerRegen												

			PlayerRegen = new int[2, 15];

			PlayerRegen[0, 0] = 0;
			PlayerRegen[0, 1] = 2;
			PlayerRegen[0, 2] = 4;
			PlayerRegen[0, 3] = 6;
			PlayerRegen[0, 4] = 8;
			PlayerRegen[0, 5] = 10;
			PlayerRegen[0, 6] = 12;
			PlayerRegen[0, 7] = 14;
			PlayerRegen[0, 8] = 16;
			PlayerRegen[0, 9] = 18;
			PlayerRegen[0, 10] = 20;
			PlayerRegen[0, 11] = 24;
			PlayerRegen[0, 12] = 28;
			PlayerRegen[0, 13] = 32;
			PlayerRegen[0, 14] = 40;

			PlayerRegen[1, 0] = 15;
			PlayerRegen[1, 1] = 100;
			PlayerRegen[1, 2] = 150;
			PlayerRegen[1, 3] = 200;
			PlayerRegen[1, 4] = 300;
			PlayerRegen[1, 5] = 400;
			PlayerRegen[1, 6] = 500;
			PlayerRegen[1, 7] = 600;
			PlayerRegen[1, 8] = 700;
			PlayerRegen[1, 9] = 800;
			PlayerRegen[1, 10] = 900;
			PlayerRegen[1, 11] = 1000;
			PlayerRegen[1, 12] = 1200;
			PlayerRegen[1, 13] = 1400;
			PlayerRegen[1, 14] = 2000;

			#endregion

			#region CoreAmmoHasRatio												

			CoreAmmoHasRatio = new int[2, 15];

			CoreAmmoHasRatio[0, 0] = 0;
			CoreAmmoHasRatio[0, 1] = 20;
			CoreAmmoHasRatio[0, 2] = 40;
			CoreAmmoHasRatio[0, 3] = 60;
			CoreAmmoHasRatio[0, 4] = 80;
			CoreAmmoHasRatio[0, 5] = 100;
			CoreAmmoHasRatio[0, 6] = 130;
			CoreAmmoHasRatio[0, 7] = 160;
			CoreAmmoHasRatio[0, 8] = 200;
			CoreAmmoHasRatio[0, 9] = 240;
			CoreAmmoHasRatio[0, 10] = 300;
			CoreAmmoHasRatio[0, 11] = 350;
			CoreAmmoHasRatio[0, 12] = 400;
			CoreAmmoHasRatio[0, 13] = 450;
			CoreAmmoHasRatio[0, 14] = 500;

			CoreAmmoHasRatio[1, 0] = 15;
			CoreAmmoHasRatio[1, 1] = 100;
			CoreAmmoHasRatio[1, 2] = 150;
			CoreAmmoHasRatio[1, 3] = 200;
			CoreAmmoHasRatio[1, 4] = 250;
			CoreAmmoHasRatio[1, 5] = 300;
			CoreAmmoHasRatio[1, 6] = 400;
			CoreAmmoHasRatio[1, 7] = 500;
			CoreAmmoHasRatio[1, 8] = 600;
			CoreAmmoHasRatio[1, 9] = 700;
			CoreAmmoHasRatio[1, 10] = 1000;
			CoreAmmoHasRatio[1, 11] = 1500;
			CoreAmmoHasRatio[1, 12] = 2000;
			CoreAmmoHasRatio[1, 13] = 2500;
			CoreAmmoHasRatio[1, 14] = 3000;

			#endregion

			#region PlayerRebornTime												

			PlayerRebornTime = new int[2, 8];

			PlayerRebornTime[0, 0] = 7;
			PlayerRebornTime[0, 1] = 6;
			PlayerRebornTime[0, 2] = 5;
			PlayerRebornTime[0, 3] = 4;
			PlayerRebornTime[0, 4] = 3;
			PlayerRebornTime[0, 5] = 2;
			PlayerRebornTime[0, 6] = 1;

			PlayerRebornTime[1, 0] = 7;
			PlayerRebornTime[1, 1] = 100;
			PlayerRebornTime[1, 2] = 200;
			PlayerRebornTime[1, 3] = 300;
			PlayerRebornTime[1, 4] = 500;
			PlayerRebornTime[1, 5] = 700;
			PlayerRebornTime[1, 6] = 1000;

			#endregion

			#endregion
		}

		#region Enemy Data Setter Getter

		static private void SetEnemyData(int level,
			int moveSpeed,
			int maxHealth,
			int attackDelay,
			int attackReach,
			int damage,
			int drapResorce,
			EGoDirection pathToGo)
		{
			EnemyDataTable[level, 0] = moveSpeed;
			EnemyDataTable[level, 1] = maxHealth;
			EnemyDataTable[level, 2] = attackDelay;
			EnemyDataTable[level, 3] = attackReach;
			EnemyDataTable[level, 4] = damage;
			EnemyDataTable[level, 5] = drapResorce;
			EnemyDataTable[level, 6] = (int)pathToGo;
		}

		static public int GetEnemyMoveSpeed(int level)
		{
			return EnemyDataTable[level, 0];
		}

		static public int GetEnemyMaxHealth(int level)
		{
			return EnemyDataTable[level, 1];
		}

		static public int GetEnemyAttackDelay(int level)
		{
			return EnemyDataTable[level, 2];
		}

		static public int GetEnemyAttackReach(int level)
		{
			return EnemyDataTable[level, 3];
		}

		static public int GetEnemyDamage(int level)
		{
			return EnemyDataTable[level, 4];
		}

		static public int GetEnemyDropResource(int level)
		{
			return EnemyDataTable[level, 5];
		}

		static public EGoDirection GetEnemyPathToGo(int level)
		{
			return (EGoDirection)EnemyDataTable[level, 6];
		}

		#endregion
	}
}
