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
	public class StartPage
	{
		private GameManager Manager;
		
		private Bitmap TitleTextBitmap;
		private Bitmap TextBitmap;

		private TextButton GameStartButton;
		private TextButton GameCloseButton;

		public StartPage(GameManager gameManager)
		{
			this.Manager = gameManager;
			this.GameStartButton = new TextButton(this.Manager, StartGame, true, 0, 250, 230, 50, "게임 시작", "맑은 고딕", 30, EDock.Center, EDock.Bottom);
			this.GameCloseButton  = new TextButton(this.Manager, CloseGame, true, 0, 190, 230, 50, "게임 종료", "맑은 고딕", 30, EDock.Center, EDock.Bottom);

			#region Initialize Title Text and Start Infomation Text 

			Bitmap bitmap = new Bitmap(500, 200);
			Graphics g = Graphics.FromImage(bitmap);

			// Create Titile Text Bitmap
			using(GraphicsPath textPath = new GraphicsPath())
			{
				g.SmoothingMode = SmoothingMode.HighQuality;

				for (int index = -3; index <= 3; index ++)
				{
					using(GraphicsPath titlePath = new GraphicsPath())
					{
						Point titlePosition = new Point(bitmap.Width / 2 + index, 50);
						using(StringFormat sf = new StringFormat())
						{
							sf.Alignment	 = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Center;

							using(FontFamily fm = new FontFamily("맑은 고딕"))
							{
								titlePath.AddString("KILL BOX", fm, (int)FontStyle.Italic, 100, titlePosition, sf);
							}
						}

						using (SolidBrush sb = new SolidBrush(Color.White))
						{
							g.FillPath(sb, titlePath);
						}
					}
				}

				// Reset Smoothing Mode
				g.SmoothingMode = SmoothingMode.None;
			}

			g.Dispose();
			this.TitleTextBitmap = bitmap;

			string contents = "게임 플레이 방법은 별도의 문서로 첨부했습니다.\n30 웨이브까지 도전해보세요.\n\n- 202013207 최지욱- ";
			this.TextBitmap = this.Manager.GetTextBitmap(contents, 4, Color.White, Color.Black, 500, 120, "맑은 고딕", 0, 20, 0);

			#endregion
		}

		public void Reset()
		{

		}

		public void Update(Point guiMousePosition)
		{
			if (this.Manager.GetGameSituation() != ESystemSituation.Start)
			{
				return;
			}
			
			this.GameStartButton.Update(guiMousePosition);
			this.GameCloseButton.Update(guiMousePosition);
		}

		public void Draw(Graphics graphics)
		{
			if (this.Manager.GetGameSituation() != ESystemSituation.Start)
			{
				return;
			}

			this.Manager.DrawDarkScreen(graphics, 150);
			
			int titlePositionX = (this.Manager.MainForm.ClientSize.Width  - this.TitleTextBitmap.Width ) / 2 - 10;
			int titlePositionY = (this.Manager.MainForm.ClientSize.Height - this.TitleTextBitmap.Height) / 2 - 100;
			
			graphics.DrawImage(this.TitleTextBitmap, titlePositionX, titlePositionY);
			
			int xPosition = (this.Manager.MainForm.ClientSize.Width  - this.TextBitmap.Width ) / 2;
			int yPosition = (this.Manager.MainForm.ClientSize.Height - this.TextBitmap.Height) / 2;

			graphics.DrawImage(this.TextBitmap, xPosition, yPosition);

			this.GameStartButton.Draw(graphics);
			this.GameCloseButton.Draw(graphics);
		}

		public void StartGame()
		{
			if (this.Manager.GetGameSituation() != ESystemSituation.Start)
			{
				return;
			}

			this.Manager.SetGameSituation(ESystemSituation.Standby);
			this.Manager.GameMouse.Reset();
			this.Manager.MainForm.ResetMousePosition();
		}

		public void CloseGame()
		{
			this.Manager.MainForm.Close();
		}
	}
}
