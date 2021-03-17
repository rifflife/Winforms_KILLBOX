using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.Page
{
	class EndPage
	{
		private GameManager Manager;
		
		private Bitmap VictoryTitleTextBitmap;
		private Bitmap VictoryTextBitmap;
		private Bitmap DefeatTitleTextBitmap;
		private Bitmap DefeatTextBitmap;

		private TextButton GameRestartButton;
		private TextButton GameCloseButton;
		
		private int GameOverDelay;

		public EndPage(GameManager gameManager)
		{
			this.Manager = gameManager;
			this.GameRestartButton = new TextButton(this.Manager, RestartGame, true, 0, 250, 230, 50, "다시 하기", "맑은 고딕", 30, EDock.Center, EDock.Bottom);
			this.GameCloseButton  = new TextButton(this.Manager, CloseGame, true, 0, 190, 230, 50, "게임 종료", "맑은 고딕", 30, EDock.Center, EDock.Bottom);

			#region Initialize Victory Title and Text 

			Bitmap victoryBitmap = new Bitmap(500, 200);
			Graphics vg = Graphics.FromImage(victoryBitmap);

			// Create Titile Text Bitmap
			using(GraphicsPath textPath = new GraphicsPath())
			{
				vg.SmoothingMode = SmoothingMode.HighQuality;

				for (int index = -3; index <= 3; index ++)
				{
					using(GraphicsPath titlePath = new GraphicsPath())
					{
						Point titlePosition = new Point(victoryBitmap.Width / 2 + index, 50);
						using(StringFormat sf = new StringFormat())
						{
							sf.Alignment	 = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Center;

							using(FontFamily fm = new FontFamily("맑은 고딕"))
							{
								titlePath.AddString("VICTORY", fm, (int)FontStyle.Italic, 100, titlePosition, sf);
							}
						}

						using (SolidBrush sb = new SolidBrush(Color.White))
						{
							vg.FillPath(sb, titlePath);
						}
					}
				}

				// Reset Smoothing Mode
				vg.SmoothingMode = SmoothingMode.None;
			}

			vg.Dispose();
			this.VictoryTitleTextBitmap = victoryBitmap;

			string victoryContents = "승리했습니다 !\n플레이해주셔서 감사합니다.";
			this.VictoryTextBitmap = this.Manager.GetTextBitmap(victoryContents, 4, Color.White, Color.Black, 400, 120, "맑은 고딕", 0, 20, 0);

			#endregion
			
			#region Initialize Defeat Title and Text 

			Bitmap defeatBitmap = new Bitmap(500, 200);
			Graphics dg = Graphics.FromImage(defeatBitmap);

			// Create Titile Text Bitmap
			using(GraphicsPath textPath = new GraphicsPath())
			{
				dg.SmoothingMode = SmoothingMode.HighQuality;

				for (int index = -3; index <= 3; index ++)
				{
					using(GraphicsPath titlePath = new GraphicsPath())
					{
						Point titlePosition = new Point(defeatBitmap.Width / 2 + index, 50);
						using(StringFormat sf = new StringFormat())
						{
							sf.Alignment	 = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Center;

							using(FontFamily fm = new FontFamily("맑은 고딕"))
							{
								titlePath.AddString("DEFEAT", fm, (int)FontStyle.Italic, 100, titlePosition, sf);
							}
						}

						using (SolidBrush sb = new SolidBrush(Color.White))
						{
							dg.FillPath(sb, titlePath);
						}
					}
				}

				// Reset Smoothing Mode
				dg.SmoothingMode = SmoothingMode.None;
			}

			dg.Dispose();
			this.DefeatTitleTextBitmap = defeatBitmap;

			string defeatContents = "패배했습니다 !\n다시 도전해보세요 !";
			this.DefeatTextBitmap = this.Manager.GetTextBitmap(defeatContents, 4, Color.White, Color.Black, 400, 120, "맑은 고딕", 0, 20, 0);

			#endregion
		}

		public void Reset()
		{
			this.GameOverDelay = 0;
		}

		public void Update(Point guiMousePosition)
		{
			
			


			if (this.Manager.GetGameSituation() != ESystemSituation.End)
			{
				return;
			}
			else
			{
				if (this.GameOverDelay < DataTable.GameOverDelayMax)
				{
					this.GameOverDelay ++;
					return;
				}
			}

			this.GameRestartButton.Update(guiMousePosition);
			this.GameCloseButton.Update(guiMousePosition);
		}

		public void Draw(Graphics graphics)
		{
			if (this.Manager.GetGameSituation() != ESystemSituation.End)
			{
				return;
			}
			else
			{
				if (this.GameOverDelay < DataTable.GameOverDelayMax)
				{
					this.GameOverDelay ++;
					return;
				}
			}

			this.Manager.DrawDarkScreen(graphics, 150);
			
			// Defeat
			if (this.Manager.GameMap.IsCoreDestroyed())
			{
				int titlePositionX = (this.Manager.MainForm.ClientSize.Width  - this.DefeatTitleTextBitmap.Width ) / 2 - 10;
				int titlePositionY = (this.Manager.MainForm.ClientSize.Height - this.DefeatTitleTextBitmap.Height) / 2 - 100;
			
				graphics.DrawImage(this.DefeatTitleTextBitmap, titlePositionX, titlePositionY);
			
				int xPosition = (this.Manager.MainForm.ClientSize.Width  - this.DefeatTextBitmap.Width ) / 2;
				int yPosition = (this.Manager.MainForm.ClientSize.Height - this.DefeatTextBitmap.Height) / 2;

				graphics.DrawImage(this.DefeatTextBitmap, xPosition, yPosition);

				this.GameRestartButton.Draw(graphics);
				this.GameCloseButton.Draw(graphics);
			}
			else // Victory
			{
				int titlePositionX = (this.Manager.MainForm.ClientSize.Width  - this.VictoryTitleTextBitmap.Width ) / 2 - 10;
				int titlePositionY = (this.Manager.MainForm.ClientSize.Height - this.VictoryTitleTextBitmap.Height) / 2 - 100;
			
				graphics.DrawImage(this.VictoryTitleTextBitmap, titlePositionX, titlePositionY);
			
				int xPosition = (this.Manager.MainForm.ClientSize.Width  - this.VictoryTextBitmap.Width ) / 2;
				int yPosition = (this.Manager.MainForm.ClientSize.Height - this.VictoryTextBitmap.Height) / 2;

				graphics.DrawImage(this.VictoryTextBitmap, xPosition, yPosition);

				this.GameRestartButton.Draw(graphics);
				this.GameCloseButton.Draw(graphics);
			}
		}

		public void RestartGame()
		{
			this.Manager.Reset();
		}

		public void CloseGame()
		{
			this.Manager.MainForm.Close();
		}
	}
}
