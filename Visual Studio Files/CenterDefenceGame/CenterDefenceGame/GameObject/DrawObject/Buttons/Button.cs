using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterDefenceGame.GameObject.DrawObject
{
	public class Button
	{
		protected GameManager Manager;

		protected int X;
		protected int Y;
		protected int XFromDock;
		protected int YFromDock;
		protected int Width;
		protected int Height;
		protected int HalfWidth;
		protected int HalfHeight;
		protected ButtonDelegate ButtonAction;
		protected Bitmap bitmap;
		public bool IsOn { get; set; }
		public int Index;
		private Point MousePosition = new Point(0, 0);
		private EDock DockHorizontal;
		private EDock DockVertical;
		private bool IsActiveReleased;

		public Button(GameManager gameManager, ButtonDelegate buttonAction, bool isActiveReleased, int xPositionFromDock, int yPositionFromDock, int width, int height, EDock dockHorizontal, EDock dockVertical)
		{
			// 위치 기준을 화면 정중앙에서 상하, 좌우 기준으로 바꿈
			this.Manager = gameManager;
			this.ButtonAction = buttonAction;
			this.IsActiveReleased = isActiveReleased;

			this.IsOn = false;
			this.XFromDock = xPositionFromDock;
			this.YFromDock = yPositionFromDock;

			this.DockHorizontal = dockHorizontal;
			this.DockVertical = dockVertical;

			this.Width = width;
			this.Height = height;

			this.HalfWidth  = this.Width / 2;
			this.HalfHeight = this.Height / 2;

			this.Reset();
		}

		public bool IsMouseOnButton()
		{
			return ((this.X - this.HalfWidth  < this.MousePosition.X) &&
				    (this.X + this.HalfWidth  > this.MousePosition.X) &&
				    (this.Y - this.HalfHeight < this.MousePosition.Y) &&
				    (this.Y + this.HalfHeight > this.MousePosition.Y));
		}

		/// <summary>
		/// Reset Button Position
		/// </summary>
		public void Reset()
		{
			int clientWidth	 = this.Manager.MainForm.ClientSize.Width;
			int clientHeight = this.Manager.MainForm.ClientSize.Height;
			
			if (this.DockHorizontal == EDock.Left)
			{
				this.X = this.XFromDock;
			}
			else if (this.DockHorizontal == EDock.Right)
			{
				this.X = clientWidth - this.XFromDock;
			}
			else if (this.DockHorizontal == EDock.Center)
			{
				this.X = clientWidth / 2 + this.XFromDock;
			}

			if (this.DockVertical == EDock.Top)
			{
				this.Y = this.YFromDock;
			}
			else if (this.DockVertical == EDock.Bottom)
			{
				this.Y = clientHeight - this.YFromDock;
			}
			else if (this.DockVertical == EDock.Center)
			{
				this.Y = clientHeight / 2 + this.YFromDock;
			}
		}

		public void Update(Point mousePosition)
		{
			this.MousePosition = mousePosition;

			if ((this.X - this.HalfWidth  < this.MousePosition.X) &&
				(this.X + this.HalfWidth  > this.MousePosition.X) &&
				(this.Y - this.HalfHeight < this.MousePosition.Y) &&
				(this.Y + this.HalfHeight > this.MousePosition.Y))
			{
				this.IsOn = true;
			}
			else
			{
				this.IsOn = false;
			}

			if (this.IsOn)
			{
				if (this.IsActiveReleased)
				{
					if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Left] == 1)
					{
						this.CallAction();
						this.IsOn = false;
					}
				}
				else
				{
					if (this.Manager.IsMouseReleasedSwitch[MouseButtons.Left] > 0)
					{
						this.CallAction();
						this.IsOn = false;
					}
				}
			}
		}

		public void Draw(Graphics graphics)
		{
			if (this.IsOn)
			{
				graphics.FillRectangle(Brushes.White, this.X - this.HalfWidth, this.Y - this.HalfHeight, this.HalfWidth * 2, this.HalfHeight * 2);
			}

			graphics.DrawImage(this.bitmap, this.X - this.HalfWidth, this.Y - this.HalfHeight);
		}

		public void CallAction()
		{
			if (this.ButtonAction != null)
			{
				this.ButtonAction();
			}
		}
	}
}
