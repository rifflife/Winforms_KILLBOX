using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.Page
{
	public class ShopPage
	{
		// External Data
		private Image ShopIconWeapon = Image.FromFile("ShopIconWeapon.png");
		private Image ShopIconCore = Image.FromFile("ShopIconCore.png");

		private int CenterX;
		private int CenterY;

		private GameManager Manager;

		#region Shop Section

		private TextButton CloseShopButton;
		private TextButton OpenShopButton;
		private TextButton SetBuildModeButton;
		private TextButton NextWaveButton;

		#endregion

		private int ButtonIndex = 0;

		// nt = nextText
		// vt = valueText
		// r = 오른쪽

		#region Weapon Section

		private TextButton ReturnButton;
		private ImageButton WeaponButton;
		private ImageButton CoreButton;

		private TextButton[] ButtonsWeapon1;
		private Bitmap[] NameTextWeapon1;
		private Bitmap[] ValueTextWeapon1;
		
		private TextButton[] ButtonsWeapon2;
		private Bitmap[] NameTextWeapon2;
		private Bitmap[] ValueTextWeapon2;

		private Image PictureShotgun = Image.FromFile("PictureShotgun.png");
		private Image PictureMachineGun = Image.FromFile("PictureMachineGun.png");
		
		private int penWidth = 3;

		private int distanceY = 65;
		private int fontSize = 30;
		
		private int bWidth = 100;
		private int bHeight = 55;

		private int ntDockX = -450;
		private int vtDockX = -300;
		private int dockX = -120;

		private int dockY1 = -50;
		private int dockY2;
		private int dockY3;
		private int dockY4;

		private int ntFontSize = 40;
		
		private int ntDockY1;
		private int ntDockY2;
		private int ntDockY3;
		private int ntDockY4;

		private int ntWidth = 200;
		private int ntHeight = 55;
		
		private int vtWidth = 130;
		private int vtHeight = 55;

		private int vtFontSize = 40;
		
		private int rntDockX = 50;
		private int rvtDockX = 200;
		private int rDockX = 380;

		#endregion
		
		#region Core Section

		private TextButton[] ButtonsCore;
		private Bitmap[] NameTextCore;
		private Bitmap[] ValueTextCore;
		
		private TextButton[] ButtonsPlayer;
		private Bitmap[] NameTextPlayer;
		private Bitmap[] ValueTextPlayer;

		private Image PictureCore = Image.FromFile("PictureCore.png");
		private Image PicturePlayer = Image.FromFile("PicturePlayer.png");
		
		private int CpenWidth = 3;

		private int CdistanceY = 65;
		private int CfontSize = 30;
		
		private int CbWidth = 100;
		private int CbHeight = 55;

		private int CntDockX = -470;
		private int CvtDockX = -290;
		private int CdockX = -100;

		private int CdockY1 = -50;
		private int CdockY2;
		private int CdockY3;
		private int CdockY4;

		private int CntFontSize = 40;
		
		private int CntDockY1;
		private int CntDockY2;
		private int CntDockY3;
		private int CntDockY4;

		private int CntWidth = 200;
		private int CntHeight = 55;
		
		private int CvtWidth = 130;
		private int CvtHeight = 55;

		private int CvtFontSize = 30;
		
		private int CrntDockX = 30;
		private int CrvtDockX = 210;
		private int CrDockX = 400;

		#endregion

		public ShopPage(GameManager gameManager)
		{
			this.Manager = gameManager;

			#region Shop Section

			this.CloseShopButton = new TextButton(Manager, CloseShop, true, 0, 150, 200, 50, "상점 나가기", "맑은 고딕", 30, EDock.Center, EDock.Bottom);
			this.OpenShopButton  = new TextButton(Manager, OpenShop, true, -210, 150, 200, 50, "상점", "맑은 고딕", 30, EDock.Center, EDock.Bottom);
			this.NextWaveButton  = new TextButton(Manager, Manager.GoNextWave, true, 0, 150, 200, 50, "웨이브 시작", "맑은 고딕", 30, EDock.Center, EDock.Bottom);
			this.SetBuildModeButton = new TextButton(Manager, SetBuildMode, true, 210, 150, 200, 50, "건설", "맑은 고딕", 30, EDock.Center, EDock.Bottom);

			#endregion
			
			this.ReturnButton = new TextButton(Manager, ReturnShop, true, 0, 150, 200, 50, "돌아가기", "맑은 고딕", 30, EDock.Center, EDock.Bottom);
			this.CoreButton   = new ImageButton(Manager, OpenCoreUpgrade, true, 200, 0, ShopIconCore.Width, ShopIconCore.Height, ShopIconCore, 1f, EDock.Center, EDock.Center);
			this.WeaponButton = new ImageButton(Manager, OpenWeaponUpgrade, true, -200, 0, ShopIconWeapon.Width, ShopIconWeapon.Height, ShopIconWeapon, 1f, EDock.Center, EDock.Center);

			#region Weapon Section

			// Left Weapon Buttons and Text Bitmaps
			ButtonsWeapon1 = new TextButton[4];
			
			dockY2 = dockY1 + distanceY;
			dockY3 = dockY2 + distanceY;
			dockY4 = dockY3 + distanceY;

			ButtonsWeapon1[0] = new TextButton(Manager, UpgradeLeftFireRate , true, dockX, dockY1, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			ButtonsWeapon1[1] = new TextButton(Manager, UpgradeLeftDamage   , true, dockX, dockY2, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			ButtonsWeapon1[2] = new TextButton(Manager, UpgradeLeftRecoil   , true, dockX, dockY3, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			ButtonsWeapon1[3] = new TextButton(Manager, UpgradeLeftExtension, true, dockX, dockY4, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			
			NameTextWeapon1 = new Bitmap[4];

			ntDockY1 = dockY1 - (ntHeight / 2);
			ntDockY2 = dockY2 - (ntHeight / 2);
			ntDockY3 = dockY3 - (ntHeight / 2);
			ntDockY4 = dockY4 - (ntHeight / 2);

			NameTextWeapon1[0] = Manager.GetTextBitmap("연사력", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);
			NameTextWeapon1[1] = Manager.GetTextBitmap("공격력", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);
			NameTextWeapon1[2] = Manager.GetTextBitmap("반　동", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);
			NameTextWeapon1[3] = Manager.GetTextBitmap("총　열", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);

			ValueTextWeapon1 = new Bitmap[4];


			// Right Weapon Buttons and Text Bitmaps
			ButtonsWeapon2 = new TextButton[4];
			
			ButtonsWeapon2[0] = new TextButton(Manager, UpgradeRightFireRate , true, rDockX, dockY1, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			ButtonsWeapon2[1] = new TextButton(Manager, UpgradeRightDamage   , true, rDockX, dockY2, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			ButtonsWeapon2[2] = new TextButton(Manager, UpgradeRightRecoil   , true, rDockX, dockY3, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			ButtonsWeapon2[3] = new TextButton(Manager, UpgradeRightExtension, true, rDockX, dockY4, bWidth, bHeight, "강화", "맑은 고딕", fontSize, EDock.Center, EDock.Center);
			
			NameTextWeapon2 = new Bitmap[4];

			ntDockY1 = dockY1 - (ntHeight / 2);
			ntDockY2 = dockY2 - (ntHeight / 2);
			ntDockY3 = dockY3 - (ntHeight / 2);
			ntDockY4 = dockY4 - (ntHeight / 2);

			NameTextWeapon2[0] = Manager.GetTextBitmap("연사력", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);
			NameTextWeapon2[1] = Manager.GetTextBitmap("공격력", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);
			NameTextWeapon2[2] = Manager.GetTextBitmap("반　동", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);
			NameTextWeapon2[3] = Manager.GetTextBitmap("펠　릿", penWidth, Color.White, Color.Black, ntWidth, ntHeight, "맑은 고딕", (int)FontStyle.Bold, ntFontSize, -1);

			ValueTextWeapon2 = new Bitmap[4];

			#endregion
			
			#region Core Section

			// Left Core Buttons and Text Bitmaps
			ButtonsCore = new TextButton[4];
			
			CdockY2 = CdockY1 + CdistanceY;
			CdockY3 = CdockY2 + CdistanceY;
			CdockY4 = CdockY3 + CdistanceY;

			ButtonsCore[0] = new TextButton(Manager, UpgradeCoreHealth  , true, CdockX, CdockY1, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			ButtonsCore[1] = new TextButton(Manager, UpgradeCoreRegen   , true, CdockX, CdockY2, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			ButtonsCore[2] = new TextButton(Manager, UpgradeCoreAmmo    , true, CdockX, CdockY3, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			ButtonsCore[3] = new TextButton(Manager, UpgradeCoreResource, true, CdockX, CdockY4, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			
			NameTextCore = new Bitmap[4];

			CntDockY1 = CdockY1 - (CntHeight / 2);
			CntDockY2 = CdockY2 - (CntHeight / 2);
			CntDockY3 = CdockY3 - (CntHeight / 2);
			CntDockY4 = CdockY4 - (CntHeight / 2);

			NameTextCore[0] = Manager.GetTextBitmap("코어체력", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);
			NameTextCore[1] = Manager.GetTextBitmap("자동회복", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);
			NameTextCore[2] = Manager.GetTextBitmap("탄약생산", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);
			NameTextCore[3] = Manager.GetTextBitmap("자원생산", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);

			ValueTextCore = new Bitmap[4];


			// Right Player Buttons and Text Bitmaps
			ButtonsPlayer = new TextButton[4];
			
			ButtonsPlayer[0] = new TextButton(Manager, UpgradePlayerHealth    , true, CrDockX, CdockY1, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			ButtonsPlayer[1] = new TextButton(Manager, UpgradePlayerRegen     , true, CrDockX, CdockY2, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			ButtonsPlayer[2] = new TextButton(Manager, UpgradeCoreAmmoHasRatio, true, CrDockX, CdockY3, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			ButtonsPlayer[3] = new TextButton(Manager, UpgradePlayerReborn    , true, CrDockX, CdockY4, CbWidth, CbHeight, "강화", "맑은 고딕", CfontSize, EDock.Center, EDock.Center);
			
			NameTextPlayer = new Bitmap[4];

			CntDockY1 = CdockY1 - (CntHeight / 2);
			CntDockY2 = CdockY2 - (CntHeight / 2);
			CntDockY3 = CdockY3 - (CntHeight / 2);
			CntDockY4 = CdockY4 - (CntHeight / 2);

			NameTextPlayer[0] = Manager.GetTextBitmap("기체체력", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);
			NameTextPlayer[1] = Manager.GetTextBitmap("자동회복", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);
			NameTextPlayer[2] = Manager.GetTextBitmap("탄약소지", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);
			NameTextPlayer[3] = Manager.GetTextBitmap("생성속도", penWidth, Color.White, Color.Black, CntWidth, CntHeight, "맑은 고딕", (int)FontStyle.Bold, CntFontSize, -1);

			ValueTextPlayer = new Bitmap[4];

			ValueBitmapReset();

			#endregion

			#region Button Indexing

			ReturnButton.Index = 0;
			ButtonsWeapon1[0].Index = 1;
			ButtonsWeapon1[1].Index = 2;
			ButtonsWeapon1[2].Index = 3;
			ButtonsWeapon1[3].Index = 4;
			ButtonsWeapon2[0].Index = 5;
			ButtonsWeapon2[1].Index = 6;
			ButtonsWeapon2[2].Index = 7;
			ButtonsWeapon2[3].Index = 8;
			ButtonsCore[0].Index = 9;
			ButtonsCore[1].Index = 10;
			ButtonsCore[2].Index = 11;
			ButtonsCore[3].Index = 12;
			ButtonsPlayer[0].Index = 13;
			ButtonsPlayer[1].Index = 14;
			ButtonsPlayer[2].Index = 15;
			ButtonsPlayer[3].Index = 16;

			#endregion
		}

		public void ValueBitmapReset()
		{
			// 플레이어의 데이터도 리셋한다.
			Manager.GamePlayer.SetPlayerData();

			// 코어의 데이터도 리셋한다.
			Manager.GameMap.SetCoreData();

			#region Weapon Section

			int fireRate1 = DataTable.LeftWeaponFireRate[0, Manager.GamePlayer.LevelLeftWeaponFireRate];
			string text = (3600 / fireRate1).ToString();
			ValueTextWeapon1[0] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			text =  DataTable.LeftWeaponDamage[0, Manager.GamePlayer.LevelLeftWeaponDamage].ToString();
			ValueTextWeapon1[1] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			text =  DataTable.LeftWeaponRecoil[0, Manager.GamePlayer.LevelLeftWeaponRecoil].ToString();
			ValueTextWeapon1[2] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			text =  DataTable.LeftWeaponExtension[0, Manager.GamePlayer.LevelLeftWeaponExtension].ToString();
			ValueTextWeapon1[3] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			
			int fireRate2 = DataTable.RightWeaponFireRate[0, Manager.GamePlayer.LevelRightWeaponFireRate];
			text = (3600 / fireRate2).ToString();
			ValueTextWeapon2[0] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			text =  DataTable.RightWeaponDamage[0, Manager.GamePlayer.LevelRightWeaponDamage].ToString();
			ValueTextWeapon2[1] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			text =  DataTable.RightWeaponRecoil[0, Manager.GamePlayer.LevelRightWeaponRecoil].ToString();
			ValueTextWeapon2[2] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			text =  DataTable.RightWeaponExtension[0, Manager.GamePlayer.LevelRightWeaponExtension].ToString();
			ValueTextWeapon2[3] = Manager.GetTextBitmap(text, penWidth, Color.Yellow, Color.Black, vtWidth, vtHeight, "맑은 고딕", (int)FontStyle.Bold, vtFontSize, -1);

			#endregion

			#region Core Section

			// Core Section
			StringBuilder cText = new StringBuilder();
			cText.Append(DataTable.CoreHealth[0, Manager.GameMap.LevelCoreHealth]);
			ValueTextCore[0] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);

			cText.Clear();
			cText.Append(DataTable.CoreRegen[0, Manager.GameMap.LevelCoreRegen]);
			cText.Append("(초당)");
			ValueTextCore[1] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);
			
			cText.Clear();
			int second = DataTable.CoreGenerateAmmunitionMakeTime[0, Manager.GameMap.LevelCoreAmmunitionMakeTime];

			cText.Append(second / 10);
			cText.Append('.');
			cText.Append(second % 10);
			cText.Append("초");
			ValueTextCore[2] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);
			
			cText.Clear();
			cText.Append(DataTable.CoreGenerateResourceRatio[0, Manager.GameMap.LevelCoreResourceRatio]);
			cText.Append("%");
			ValueTextCore[3] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);

			// Player Section
			cText.Clear();
			cText.Append(DataTable.PlayerHealth[0, Manager.GamePlayer.LevelPlayerHealth]);
			ValueTextPlayer[0] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);

			cText.Clear();
			cText.Append(DataTable.PlayerRegen[0, Manager.GamePlayer.LevelPlayerRegen]);
			cText.Append("(초당)");
			ValueTextPlayer[1] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);
			
			cText.Clear();
			cText.Append(DataTable.CoreAmmoHasRatio[0, Manager.GameMap.LevelCoreAmmoHaveRatio]);
			cText.Append("%");
			ValueTextPlayer[2] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);
			
			cText.Clear();
			cText.Append(DataTable.PlayerRebornTime[0, Manager.GamePlayer.LevelPlayerReborn]);
			cText.Append("초");
			ValueTextPlayer[3] = Manager.GetTextBitmap(cText.ToString(), penWidth, Color.Yellow, Color.Black, CvtWidth, CvtHeight, "맑은 고딕", (int)FontStyle.Bold, CvtFontSize, -1);

			#endregion
		}

		public void Reset()
		{
			this.CenterX = this.Manager.MainForm.ClientSize.Width / 2;
			this.CenterY = this.Manager.MainForm.ClientSize.Height / 2;
			ValueBitmapReset();
		}

		public void Update(Point virtualMousePosition, Point guiMousePosition)
		{
			this.ButtonIndex = 0;

			switch(this.Manager.GetGameSituation())
			{
				case ESystemSituation.Standby:
					this.OpenShopButton.Update(virtualMousePosition);
					this.NextWaveButton.Update(virtualMousePosition);
					this.SetBuildModeButton.Update(virtualMousePosition);
					break;

				case ESystemSituation.Shop:
					this.CloseShopButton.Update(guiMousePosition);
					this.WeaponButton.Update(guiMousePosition);
					this.CoreButton.Update(guiMousePosition);
					break;

				case ESystemSituation.WeaponShop:
					this.ReturnButton.Update(guiMousePosition);

					for (int buttons = 0; buttons < 4; buttons ++)
					{
						this.ButtonsWeapon1[buttons].Update(guiMousePosition);
						this.ButtonsWeapon2[buttons].Update(guiMousePosition);

						if (ButtonsWeapon1[buttons].IsOn)
						{
							this.ButtonIndex = ButtonsWeapon1[buttons].Index;
							break;
						}

						if (ButtonsWeapon2[buttons].IsOn)
						{
							this.ButtonIndex = ButtonsWeapon2[buttons].Index;
							break;
						}
					}
					break;

				case ESystemSituation.CoreShop:
					this.ReturnButton.Update(guiMousePosition);

					for (int buttons = 0; buttons < 4; buttons ++)
					{
						this.ButtonsCore[buttons].Update(guiMousePosition);
						this.ButtonsPlayer[buttons].Update(guiMousePosition);
						
						if (ButtonsCore[buttons].IsOn)
						{
							this.ButtonIndex = ButtonsCore[buttons].Index;
							break;
						}

						if (ButtonsPlayer[buttons].IsOn)
						{
							this.ButtonIndex = ButtonsPlayer[buttons].Index;
							break;
						}
					}
					break;
			}
		}

		public void Draw(Graphics graphics)
		{
			switch(this.Manager.GetGameSituation())
			{
				case ESystemSituation.Standby:
					this.OpenShopButton.Draw(graphics);
					this.NextWaveButton.Draw(graphics);
					this.SetBuildModeButton.Draw(graphics);
					break;

				case ESystemSituation.Shop:
					this.Manager.DrawDarkScreen(graphics, 150);

					this.CloseShopButton.Draw(graphics);
					this.WeaponButton.Draw(graphics);
					this.CoreButton.Draw(graphics);
					break;

				case ESystemSituation.WeaponShop:
					this.Manager.DrawDarkScreen(graphics, 150);

					// Draw Weapon Picture
					graphics.DrawImage(PictureMachineGun, CenterX - 30 - PictureMachineGun.Width, CenterY - 300, PictureMachineGun.Width, PictureMachineGun.Height);
					graphics.DrawImage(PictureShotgun, CenterX + 30, CenterY - 300, PictureShotgun.Width, PictureShotgun.Height);

					for (int index = 0; index < 4; index ++)
					{
						this.ButtonsWeapon1[index].Draw(graphics);
						graphics.DrawImage(NameTextWeapon1[index], CenterX + ntDockX, CenterY + ntDockY1 + (distanceY * index), NameTextWeapon1[index].Width, NameTextWeapon1[index].Height);
						graphics.DrawImage(ValueTextWeapon1[index], CenterX + vtDockX, CenterY + ntDockY1 + (distanceY * index), ValueTextWeapon1[index].Width, ValueTextWeapon1[index].Height);
						
						this.ButtonsWeapon2[index].Draw(graphics);
						graphics.DrawImage(NameTextWeapon2[index], CenterX + rntDockX, CenterY + ntDockY1 + (distanceY * index), NameTextWeapon2[index].Width, NameTextWeapon2[index].Height);
						graphics.DrawImage(ValueTextWeapon2[index], CenterX + rvtDockX, CenterY + ntDockY1 + (distanceY * index), ValueTextWeapon2[index].Width, ValueTextWeapon2[index].Height);
					}

					break;

				case ESystemSituation.CoreShop:
					this.Manager.DrawDarkScreen(graphics, 150);

					// Draw Weapon Picture
					graphics.DrawImage(PictureCore, CenterX - 30 - PictureMachineGun.Width, CenterY - 300, PictureMachineGun.Width, PictureMachineGun.Height);
					graphics.DrawImage(PicturePlayer, CenterX + 30, CenterY - 300, PictureShotgun.Width, PictureShotgun.Height);

					for (int index = 0; index < 4; index ++)
					{
						this.ButtonsCore[index].Draw(graphics);
						graphics.DrawImage(NameTextCore[index], CenterX + CntDockX, CenterY + CntDockY1 + (CdistanceY * index), NameTextCore[index].Width, NameTextCore[index].Height);
						graphics.DrawImage(ValueTextCore[index], CenterX + CvtDockX, CenterY + CntDockY1 + (CdistanceY * index), ValueTextCore[index].Width, ValueTextCore[index].Height);
						
						this.ButtonsPlayer[index].Draw(graphics);
						graphics.DrawImage(NameTextPlayer[index], CenterX + CrntDockX, CenterY + CntDockY1 + (CdistanceY * index), NameTextPlayer[index].Width, NameTextPlayer[index].Height);
						graphics.DrawImage(ValueTextPlayer[index], CenterX + CrvtDockX, CenterY + CntDockY1 + (CdistanceY * index), ValueTextPlayer[index].Width, ValueTextPlayer[index].Height);
					}

					break;
			}

			if (Manager.GetGameSituation() == ESystemSituation.WeaponShop || Manager.GetGameSituation() == ESystemSituation.CoreShop)
			{
				if (ButtonIndex == 0)
				{
					this.ReturnButton.Draw(graphics);
				}
				else
				{
					using(StringFormat sf = new StringFormat())
					using(Font font = new Font("맑은 고딕", 20))
					{
						graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Center;

						int price = GetPrice(ButtonIndex);

						int positionCenterX = CenterX;
						int positionCenterY = CenterY + 230;
						Rectangle rect = new Rectangle(positionCenterX - 130, positionCenterY - 25, 260, 50);

						if (price == -1)
						{
							graphics.FillRectangle(Brushes.Black, rect);

							graphics.DrawString("업그레이드 완료", font, Brushes.White, positionCenterX, positionCenterY, sf);
						}
						else if (price > 0)
						{
							graphics.FillRectangle(Brushes.Black, rect);

							StringBuilder sb = new StringBuilder();
							sb.Append("자원 필요 : ");
							sb.Append(price);
							graphics.DrawString(sb.ToString(), font, Brushes.White, positionCenterX, positionCenterY, sf);
						}


						graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					}
				}
			}
		}

		#region Shop Section

		private void CloseShop()
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Shop)
			{
				this.Manager.SetGameSituation(ESystemSituation.Standby);
				this.Manager.GameMouse.Reset();
				this.Manager.MainForm.ResetMousePosition();
			}
		}

		private void OpenShop()
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Standby)
			{
				this.Manager.SetGameSituation(ESystemSituation.Shop);
			}
		}

		private void SetBuildMode()
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Standby)
			{
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, "블럭 제거시 자원을 돌려받습니다.");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, "[Mouse Left] 블럭 설치");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, "[Mouse Right] 블럭 제거");
				this.Manager.GameGUI.AddNotification(EMessageType.Normal, "ESC로 종료");
				this.Manager.GameMap.SetBuildModeInitaiDelay();
				this.Manager.SetGameSituation(ESystemSituation.Build);
			}
		}
		
		private void ReturnShop()
		{
			this.Manager.SetGameSituation(ESystemSituation.Shop);
		}

		private void OpenWeaponUpgrade()
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Shop)
			{
				this.Manager.SetGameSituation(ESystemSituation.WeaponShop);
			}
		}
		
		private void OpenCoreUpgrade()
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Shop)
			{
				this.Manager.SetGameSituation(ESystemSituation.CoreShop);
			}
		}

		#endregion


		#region Weapon Section

		// 기회가 되면 다음에는 다른 데이터형으로 좀 더 추상화해야 할 것 같다.
		// Left Weapon Upgrade Function

		private void UpgradeLeftFireRate()
		{
			if (this.Manager.GamePlayer.LevelLeftWeaponFireRate < DataTable.LeftWeaponFireRate[1, 0] - 1)
			{
				int price = DataTable.LeftWeaponFireRate[1, this.Manager.GamePlayer.LevelLeftWeaponFireRate + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelLeftWeaponFireRate ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeLeftDamage()
		{
			if (this.Manager.GamePlayer.LevelLeftWeaponDamage < DataTable.LeftWeaponDamage[1, 0] - 1)
			{
				int price = DataTable.LeftWeaponDamage[1, this.Manager.GamePlayer.LevelLeftWeaponDamage + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelLeftWeaponDamage ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeLeftRecoil()
		{
			if (this.Manager.GamePlayer.LevelLeftWeaponRecoil < DataTable.LeftWeaponRecoil[1, 0] - 1)
			{
				int price = DataTable.LeftWeaponRecoil[1, this.Manager.GamePlayer.LevelLeftWeaponRecoil + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelLeftWeaponRecoil ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeLeftExtension()
		{
			if (this.Manager.GamePlayer.LevelLeftWeaponExtension < DataTable.LeftWeaponExtension[1, 0] - 1)
			{
				int price = DataTable.LeftWeaponExtension[1, this.Manager.GamePlayer.LevelLeftWeaponExtension + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelLeftWeaponExtension ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		// Right Weapon Upgrade Function
		private void UpgradeRightFireRate()
		{
			if (this.Manager.GamePlayer.LevelRightWeaponFireRate < DataTable.RightWeaponFireRate[1, 0] - 1)
			{
				int price = DataTable.RightWeaponFireRate[1, this.Manager.GamePlayer.LevelRightWeaponFireRate + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelRightWeaponFireRate ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeRightDamage()
		{
			if (this.Manager.GamePlayer.LevelRightWeaponDamage < DataTable.RightWeaponDamage[1, 0] - 1)
			{
				int price = DataTable.RightWeaponDamage[1, this.Manager.GamePlayer.LevelRightWeaponDamage + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelRightWeaponDamage ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeRightRecoil()
		{
			if (this.Manager.GamePlayer.LevelRightWeaponRecoil < DataTable.RightWeaponRecoil[1, 0] - 1)
			{
				int price = DataTable.RightWeaponRecoil[1, this.Manager.GamePlayer.LevelRightWeaponRecoil + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelRightWeaponRecoil ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeRightExtension()
		{
			if (this.Manager.GamePlayer.LevelRightWeaponExtension < DataTable.RightWeaponExtension[1, 0] - 1)
			{
				int price = DataTable.RightWeaponExtension[1, this.Manager.GamePlayer.LevelRightWeaponExtension + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelRightWeaponExtension ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		#endregion

		#region Core Section

		private void UpgradeCoreHealth()
		{
			if (this.Manager.GameMap.LevelCoreHealth < DataTable.CoreHealth[1, 0] - 1)
			{
				int price = DataTable.CoreHealth[1, this.Manager.GameMap.LevelCoreHealth + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GameMap.LevelCoreHealth ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeCoreRegen()
		{
			if (this.Manager.GameMap.LevelCoreRegen < DataTable.CoreRegen[1, 0] - 1)
			{
				int price = DataTable.CoreRegen[1, this.Manager.GameMap.LevelCoreRegen + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GameMap.LevelCoreRegen ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeCoreAmmo()
		{
			if (this.Manager.GameMap.LevelCoreAmmunitionMakeTime < DataTable.CoreGenerateAmmunitionMakeTime[1, 0] - 1)
			{
				int price = DataTable.CoreGenerateAmmunitionMakeTime[1, this.Manager.GameMap.LevelCoreAmmunitionMakeTime + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GameMap.LevelCoreAmmunitionMakeTime ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeCoreResource()
		{
			if (this.Manager.GameMap.LevelCoreResourceRatio < DataTable.CoreGenerateResourceRatio[1, 0] - 1)
			{
				int price = DataTable.CoreGenerateResourceRatio[1, this.Manager.GameMap.LevelCoreResourceRatio + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GameMap.LevelCoreResourceRatio ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradePlayerHealth()
		{
			if (this.Manager.GamePlayer.LevelPlayerHealth < DataTable.PlayerHealth[1, 0] - 1)
			{
				int price = DataTable.PlayerHealth[1, this.Manager.GamePlayer.LevelPlayerHealth + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelPlayerHealth ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradePlayerRegen()
		{
			if (this.Manager.GamePlayer.LevelPlayerRegen < DataTable.PlayerRegen[1, 0] - 1)
			{
				int price = DataTable.PlayerRegen[1, this.Manager.GamePlayer.LevelPlayerRegen + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelPlayerRegen ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradeCoreAmmoHasRatio()
		{
			if (this.Manager.GameMap.LevelCoreAmmoHaveRatio < DataTable.CoreAmmoHasRatio[1, 0] - 1)
			{
				int price = DataTable.CoreAmmoHasRatio[1, this.Manager.GameMap.LevelCoreAmmoHaveRatio + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GameMap.LevelCoreAmmoHaveRatio ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		private void UpgradePlayerReborn()
		{
			if (this.Manager.GamePlayer.LevelPlayerReborn < DataTable.PlayerRebornTime[1, 0] - 1)
			{
				int price = DataTable.PlayerRebornTime[1, this.Manager.GamePlayer.LevelPlayerReborn + 1];
				
				if (this.Manager.ConsumResource(price))
				{
					this.Manager.GamePlayer.LevelPlayerReborn ++;
					this.Manager.GameGUI.AddNotification(EMessageType.System, "업그레이드 완료");
					ValueBitmapReset();
				}
			}
			else
			{
				this.Manager.GameGUI.AddNotification(EMessageType.System, "더 이상 업그레이드 할 수 없음");
			}
		}

		#endregion

		/// <summary>
		/// 해당 인덱스의 가격을 반환한다. 업그레이드가 완료되면 -1을 반환한다. 아무것도 없다면 0을 반환한다.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private int GetPrice(int index)
		{
			// 처음에 잘못 만들어서 이런 사단이 났다
			//ReturnButton.Index = 0;
			//ButtonsWeapon1[0].Index = 1;
			//ButtonsWeapon1[1].Index = 2;
			//ButtonsWeapon1[2].Index = 3;
			//ButtonsWeapon1[3].Index = 4;
			//ButtonsWeapon2[0].Index = 5;
			//ButtonsWeapon2[1].Index = 6;
			//ButtonsWeapon2[2].Index = 7;
			//ButtonsWeapon2[3].Index = 8;
			//ButtonsCore[0].Index = 9;
			//ButtonsCore[1].Index = 10;
			//ButtonsCore[2].Index = 11;
			//ButtonsCore[3].Index = 12;
			//ButtonsPlayer[0].Index = 13;
			//ButtonsPlayer[1].Index = 14;
			//ButtonsPlayer[2].Index = 15;
			//ButtonsPlayer[3].Index = 16;

			switch(index)
			{
				case 1:
					if (this.Manager.GamePlayer.LevelLeftWeaponFireRate < DataTable.LeftWeaponFireRate[1, 0] - 1)
					{
						return DataTable.LeftWeaponFireRate[1, this.Manager.GamePlayer.LevelLeftWeaponFireRate + 1];
					}
					else
					{
						return -1;
					}

				case 2:
					if (this.Manager.GamePlayer.LevelLeftWeaponDamage < DataTable.LeftWeaponDamage[1, 0] - 1)
					{
						return DataTable.LeftWeaponDamage[1, this.Manager.GamePlayer.LevelLeftWeaponDamage + 1];
					}
					else
					{
						return -1;
					}

				case 3:
					if (this.Manager.GamePlayer.LevelLeftWeaponRecoil < DataTable.LeftWeaponRecoil[1, 0] - 1)
					{
						return DataTable.LeftWeaponRecoil[1, this.Manager.GamePlayer.LevelLeftWeaponRecoil + 1];
					}
					else
					{
						return -1;
					}

				case 4:
					if (this.Manager.GamePlayer.LevelLeftWeaponExtension < DataTable.LeftWeaponExtension[1, 0] - 1)
					{
						return DataTable.LeftWeaponExtension[1, this.Manager.GamePlayer.LevelLeftWeaponExtension + 1];
					}
					else
					{
						return -1;
					}

				case 5:
					if (this.Manager.GamePlayer.LevelRightWeaponFireRate < DataTable.RightWeaponFireRate[1, 0] - 1)
					{
						return DataTable.RightWeaponFireRate[1, this.Manager.GamePlayer.LevelRightWeaponFireRate + 1];
					}
					else
					{
						return -1;
					}

				case 6:
					if (this.Manager.GamePlayer.LevelRightWeaponDamage < DataTable.RightWeaponDamage[1, 0] - 1)
					{
						return DataTable.RightWeaponDamage[1, this.Manager.GamePlayer.LevelRightWeaponDamage + 1];
					}
					else
					{
						return -1;
					}

				case 7:
					if (this.Manager.GamePlayer.LevelRightWeaponRecoil < DataTable.RightWeaponRecoil[1, 0] - 1)
					{
						return DataTable.RightWeaponRecoil[1, this.Manager.GamePlayer.LevelRightWeaponRecoil + 1];
					}
					else
					{
						return -1;
					}

				case 8:
					if (this.Manager.GamePlayer.LevelRightWeaponExtension < DataTable.RightWeaponExtension[1, 0] - 1)
					{
						return DataTable.RightWeaponExtension[1, this.Manager.GamePlayer.LevelRightWeaponExtension + 1];
					}
					else
					{
						return -1;
					}

				case 9:
					if (this.Manager.GameMap.LevelCoreHealth < DataTable.CoreHealth[1, 0] - 1)
					{
						return DataTable.CoreHealth[1, this.Manager.GameMap.LevelCoreHealth + 1];
					}
					else
					{
						return -1;
					}

				case 10:
					if (this.Manager.GameMap.LevelCoreRegen < DataTable.CoreRegen[1, 0] - 1)
					{
						return DataTable.CoreRegen[1, this.Manager.GameMap.LevelCoreRegen + 1];
					}
					else
					{
						return -1;
					}

				case 11:
					if (this.Manager.GameMap.LevelCoreAmmunitionMakeTime < DataTable.CoreGenerateAmmunitionMakeTime[1, 0] - 1)
					{
						return DataTable.CoreGenerateAmmunitionMakeTime[1, this.Manager.GameMap.LevelCoreAmmunitionMakeTime + 1];
					}
					else
					{
						return -1;
					}

				case 12:
					if (this.Manager.GameMap.LevelCoreResourceRatio < DataTable.CoreGenerateResourceRatio[1, 0] - 1)
					{
						return DataTable.CoreGenerateResourceRatio[1, this.Manager.GameMap.LevelCoreResourceRatio + 1];
					}
					else
					{
						return -1;
					}

				case 13:
					if (this.Manager.GamePlayer.LevelPlayerHealth < DataTable.PlayerHealth[1, 0] - 1)
					{
						return DataTable.PlayerHealth[1, this.Manager.GamePlayer.LevelPlayerHealth + 1];
					}
					else
					{
						return -1;
					}

				case 14:
					if (this.Manager.GamePlayer.LevelPlayerRegen < DataTable.PlayerRegen[1, 0] - 1)
					{
						return DataTable.PlayerRegen[1, this.Manager.GamePlayer.LevelPlayerRegen + 1];
					}
					else
					{
						return -1;
					}

				case 15:
					if (this.Manager.GameMap.LevelCoreAmmoHaveRatio < DataTable.CoreAmmoHasRatio[1, 0] - 1)
					{
						return DataTable.CoreAmmoHasRatio[1, this.Manager.GameMap.LevelCoreAmmoHaveRatio + 1];
					}
					else
					{
						return -1;
					}

				case 16:
					if (this.Manager.GamePlayer.LevelPlayerReborn < DataTable.PlayerRebornTime[1, 0] - 1)
					{
						return DataTable.PlayerRebornTime[1, this.Manager.GamePlayer.LevelPlayerReborn + 1];
					}
					else
					{
						return -1;
					}
			}

			return 0;
		}

	}
}
