using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * 가상 마우스는 GUI상에서의 좌표만을 가진다.
 * 게임속 상호작용은 GameManager에서 가상마우스의 좌표와 Camera의 좌표를 참조해서 계산한다.
 */

namespace CenterDefenceGame.GameObject
{
	public class VirtualMouse
	{
		private GameManager Manager;

		private bool IsVirtualMouseMode = false;

		// Mouse Position Set
		public Point Position;
		private int FormWidth;
		private int FormHeight;

		// Aim Color Set
		private Pen FillPen;
		private Pen ForePen;
		private int AimLength;
		private int DefaultDistance;

		// Normal Cursor Pointer
		private Image CursorImage;

		public VirtualMouse(GameManager gameManager)
		{
			this.Manager = gameManager;

			this.CursorImage = Image.FromFile("Cursor.png");
			
			// Position
			this.FormWidth	= this.Manager.MainForm.ClientSize.Width;
			this.FormHeight = this.Manager.MainForm.ClientSize.Height;
			this.Position = new Point(this.FormWidth  / 2, this.FormHeight / 2);

			// Aim Color and Length
			this.FillPen = new Pen(Color.Yellow, 1);
			this.FillPen.EndCap	  = System.Drawing.Drawing2D.LineCap.Square;
			this.FillPen.StartCap = System.Drawing.Drawing2D.LineCap.Square;

			this.ForePen = new Pen(this.Manager.BackgroundColor, 3);
			this.ForePen.EndCap   = System.Drawing.Drawing2D.LineCap.Square;
			this.ForePen.StartCap = System.Drawing.Drawing2D.LineCap.Square;

			this.DefaultDistance = 5;
			this.AimLength = 16;
		}

		public void Reset()
		{
			this.FormWidth	= this.Manager.MainForm.ClientSize.Width;
			this.FormHeight = this.Manager.MainForm.ClientSize.Height;
			this.Position.X = this.FormWidth  / 2;
			this.Position.Y = this.FormHeight / 2;
		}

		public void Update(float deltaRatio, Point mouseMovedAmount)
		{
			if (this.Manager.GetGameSituation() == ESystemSituation.Wave ||
				this.Manager.GetGameSituation() == ESystemSituation.Standby ||
				this.Manager.GetGameSituation() == ESystemSituation.Build ||
				this.Manager.GetGameSituation() == ESystemSituation.WaveClear )
			{
				this.IsVirtualMouseMode = true;
			}
			else
			{
				this.IsVirtualMouseMode = false;
				return;
			}

			#region Virtual Mouse Set

			this.Position.X += mouseMovedAmount.X;
			this.Position.Y += mouseMovedAmount.Y;

			if (this.Position.X < 0)
			{
				this.Position.X = 0;
			}

			if (this.Position.X > this.FormWidth)
			{
				this.Position.X = this.FormWidth;
			}

			if (this.Position.Y < 0)
			{
				this.Position.Y = 0;
			}
			
			if (this.Position.Y > this.FormHeight)
			{
				this.Position.Y = this.FormHeight;
			}

			#endregion
		}

		public void Draw(Graphics graphics,Point guiMousePosition)
		{
			#region Debug Mouse

			if (this.Manager.MainForm.IsDebugging)
			{
				if (this.IsVirtualMouseMode)
				{
					using (Pen pen = new Pen(this.Manager.BackgroundColor, 2))
					{
						int width = 10;
						int doubleWidth = width * 2;
						int leftTopX = this.Position.X - width;
						int leftTopY = this.Position.Y - width;
						graphics.FillRectangle(Brushes.Black, leftTopX, leftTopY, doubleWidth, doubleWidth);
						graphics.DrawRectangle(pen, leftTopX, leftTopY, doubleWidth, doubleWidth);
						graphics.DrawLine(pen, leftTopX, leftTopY, leftTopX + doubleWidth, leftTopY + doubleWidth);
						graphics.DrawLine(pen, leftTopX + doubleWidth, leftTopY, leftTopX, leftTopY + doubleWidth);
					}
				}
				else
				{
					using (Pen pen = new Pen(this.Manager.BackgroundColor, 2))
					{
						int width = 10;
						int doubleWidth = width * 2;
						int leftTopX = guiMousePosition.X - width;
						int leftTopY = guiMousePosition.Y - width;
						graphics.FillRectangle(Brushes.Black, leftTopX, leftTopY, doubleWidth, doubleWidth);
						graphics.DrawRectangle(pen, leftTopX, leftTopY, doubleWidth, doubleWidth);
						graphics.DrawLine(pen, leftTopX, leftTopY, leftTopX + doubleWidth, leftTopY + doubleWidth);
						graphics.DrawLine(pen, leftTopX + doubleWidth, leftTopY, leftTopX, leftTopY + doubleWidth);
					}
				}
				return;
			}

			#endregion

			#region Draw Cursor or Aim

			if (!this.IsVirtualMouseMode)
			{
				graphics.DrawImage(this.CursorImage, guiMousePosition.X, guiMousePosition.Y, this.CursorImage.Width, this.CursorImage.Height);
				return;
			}
			else
			{
				int recoilOffset = this.Manager.GamePlayer.GetAimRecoil() + this.DefaultDistance;

				graphics.DrawLine(ForePen, Position.X + recoilOffset, Position.Y, Position.X + AimLength + recoilOffset, Position.Y); // Right
				graphics.DrawLine(FillPen, Position.X + recoilOffset, Position.Y, Position.X + AimLength + recoilOffset, Position.Y); // Right
				graphics.DrawLine(ForePen, Position.X - recoilOffset, Position.Y, Position.X - AimLength - recoilOffset, Position.Y); // Left
				graphics.DrawLine(FillPen, Position.X - recoilOffset, Position.Y, Position.X - AimLength - recoilOffset, Position.Y); // Left
				graphics.DrawLine(ForePen, Position.X, Position.Y + recoilOffset, Position.X, Position.Y + AimLength + recoilOffset); // Down
				graphics.DrawLine(FillPen, Position.X, Position.Y + recoilOffset, Position.X, Position.Y + AimLength + recoilOffset); // Down
				graphics.DrawLine(ForePen, Position.X, Position.Y - recoilOffset, Position.X, Position.Y - AimLength - recoilOffset); // Up
				graphics.DrawLine(FillPen, Position.X, Position.Y - recoilOffset, Position.X, Position.Y - AimLength - recoilOffset); // Up
			}

			#endregion
		}

		#region Getter Setter

		public bool GetVirtualMouseMode()
		{
			return this.IsVirtualMouseMode;
		}

		public Point GetPosition()
		{
			return Position;
		}

		#endregion

	}
}
