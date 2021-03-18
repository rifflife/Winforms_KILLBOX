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
	public class PausePage
	{
		private GameManager Manager;
		private Bitmap TextBitmap;
		private ESystemSituation PreviousSituation;

		private TextButton GameResumeButton;
		private TextButton GameCloseButton;
		private TextButton GameRestart;

		public PausePage(GameManager gameManager)
		{
			this.Manager = gameManager;
			this.GameResumeButton = new TextButton(this.Manager, ResumeGame, true, 0, 280, 230, 50, "돌아가기", GameFont.GAME_FONT, 30, EDock.Center, EDock.Bottom);
			this.GameRestart  = new TextButton(this.Manager, this.Manager.Reset, true, 0, 220, 230, 50, "재시작", GameFont.GAME_FONT, 30, EDock.Center, EDock.Bottom);
			this.GameCloseButton  = new TextButton(this.Manager, CloseGame, true, 0, 160, 230, 50, "게임 종료", GameFont.GAME_FONT, 30, EDock.Center, EDock.Bottom);

			this.TextBitmap = this.Manager.GetTextBitmap("일시 정지", 3, Color.White, Color.White, 400, 120, GameFont.GAME_FONT, (int)FontStyle.Italic, 70, 0);
		}

		public void Reset()
		{

		}

		public void Update(Point guiMousePosition)
		{
			ESystemSituation situation = this.Manager.GetGameSituation();
			if (situation != ESystemSituation.Pause)
			{
				this.PreviousSituation = situation;
				return;
			}
			
			this.GameResumeButton.Update(guiMousePosition);
			this.GameRestart.Update(guiMousePosition);
			this.GameCloseButton.Update(guiMousePosition);
		}

		public void Draw(Graphics graphics)
		{
			if (this.Manager.GetGameSituation() != ESystemSituation.Pause)
			{
				return;
			}

			this.Manager.DrawDarkScreen(graphics, 220);

			int xPosition = (this.Manager.MainForm.ClientSize.Width  - this.TextBitmap.Width ) / 2;
			int yPosition = (this.Manager.MainForm.ClientSize.Height - this.TextBitmap.Height) / 2;

			graphics.DrawImage(this.TextBitmap, xPosition, yPosition - 100);

			this.GameResumeButton.Draw(graphics);
			this.GameRestart.Draw(graphics);
			this.GameCloseButton.Draw(graphics);
		}

		public void ResumeGame()
		{
			if (this.Manager.GetGameSituation() != ESystemSituation.Pause)
			{
				return;
			}

			this.Manager.SetGameSituation(this.PreviousSituation);
			bool isVirtualMouse = ((this.PreviousSituation == ESystemSituation.Wave) ||
								   (this.PreviousSituation == ESystemSituation.Standby) ||
								   (this.PreviousSituation == ESystemSituation.Build));
			if (isVirtualMouse)
			{
				this.Manager.MainForm.ResetMousePosition();
			}
			this.Manager.GameGUI.AddNotification(EMessageType.System, "게임 재개");
		}

		public void CloseGame()
		{
			this.Manager.MainForm.Close();
		}
	}
}
