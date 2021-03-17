using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject
{
	public class DeadEffect
	{
		private GameManager Manager;

		private ImageAttributes TransparitySmall;
		private ImageAttributes TransparityBig;

		private int X = -1000;
		private int Y = -1000;

		private int Width = 5;
		private Pen EffectIn;
		private Pen EffectOut;

		private int Size = 0;
		private int RadiusIn = 0;
		private int RadiusOut = 0;
		private int Delay = 0;

		private bool IsBig = false;

		public DeadEffect(GameManager gameManager)
		{
			this.Manager = gameManager;
			this.EffectIn = new Pen(Color.White, Width);
			this.EffectOut = new Pen(Color.Yellow, Width);

			ColorMatrix cm = new ColorMatrix();
			cm.Matrix33 = 0.3f;
			this.TransparitySmall = new ImageAttributes();
			this.TransparitySmall.SetColorMatrix(cm);

			cm.Matrix33 = 0.7f;
			this.TransparityBig = new ImageAttributes();
			this.TransparityBig.SetColorMatrix(cm);
		}

		public void Active(int x, int y, int size, bool isBig)
		{
			this.X = x;
			this.Y = y;
			this.Size = size;
			this.RadiusIn = 0;
			this.RadiusOut = Width;
			this.Delay = 7;
			this.IsBig = isBig;
			this.DrawMark();
		}

		public void DrawMark()
		{
			Graphics graphics = Graphics.FromImage(this.Manager.GameMapBitmap);

			Image mark;

			if (this.IsBig)
			{
				mark = this.Manager.BigMark;
				Rectangle rect = new Rectangle(this.X - mark.Width / 2, this.Y - mark.Height / 2, mark.Width, mark.Height);
				graphics.DrawImage(mark, rect, 0, 0, mark.Width, mark.Height, GraphicsUnit.Pixel,  this.TransparityBig);
			}
			else
			{
				mark = this.Manager.SmallMark;
				Rectangle rect = new Rectangle(this.X - mark.Width / 2, this.Y - mark.Height / 2, mark.Width, mark.Height);
				graphics.DrawImage(mark, rect, 0, 0, mark.Width, mark.Height, GraphicsUnit.Pixel,  this.TransparitySmall);
			}

			graphics.Dispose();
		}

		public void Draw(Graphics graphics)
		{
			if (this.Delay > 0)
			{
				this.Delay --;
				this.RadiusIn += this.Size;
				this.RadiusOut += this.Size;
			}
			else
			{
				return;
			}

			int inDiameter = this.RadiusIn * 2;
			int outDiameter = this.RadiusOut * 2;
			int inX = this.X - this.RadiusIn - this.Manager.GameCamera.GetX();
			int inY = this.Y - this.RadiusIn - this.Manager.GameCamera.GetY();
			
			int outX = this.X - this.RadiusOut - this.Manager.GameCamera.GetX();
			int outY = this.Y - this.RadiusOut - this.Manager.GameCamera.GetY();

			graphics.DrawEllipse(this.EffectIn, inX, inY, inDiameter, inDiameter);
			graphics.DrawEllipse(this.EffectOut, outX, outY, outDiameter, outDiameter);
		}

	}
}
