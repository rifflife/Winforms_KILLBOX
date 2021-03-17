using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject
{
	public class PlayerBullet
	{
		private GameManager Manager;
		private readonly int Width = 3;
		private Pen BulletColor;
		private int Length;
		private Vector2D StartPosition;
		private Vector2D EndPosition;
		private Vector2D Normalize;
		private Point HitPosition = new Point(-1000, -1000);
		public readonly int Speed;
		public readonly int Damage;
		private float DestoryDelay;
		public bool IsHit;
		private float DisappearDelay;

		public PlayerBullet(GameManager gameManager, float playerX, float playerY, float mouseX, float mouseY, int speed, int damage, Color color, float recoil)
		{
			this.Manager = gameManager;
			this.BulletColor = new Pen(color, Width); //Color.FromArgb(255, 100, 0), Width);
			this.StartPosition = new Vector2D(playerX, playerY);
			this.Speed = speed;
			this.Length = this.Speed * 4;
			this.DestoryDelay = 150;
			this.Damage = damage;
			this.IsHit = false;

			#region Add recoil

			float directionX = mouseX - playerX;
			float directionY = mouseY - playerY;
			float directionScalar = (float)Math.Sqrt(directionX * directionX + directionY * directionY);

			this.Normalize = new Vector2D(directionX / directionScalar, directionY / directionScalar);

			float tempX = Normalize.X;
			float tempY = Normalize.Y;

			// X Y 평면상에서 회전
			float rotatedScalar = (float)Math.Sqrt(tempX * tempX + tempY * tempY);
			float setRotatedAngle = (float)Math.Atan2(-tempY, tempX) + recoil;

			tempX = (float)Math.Cos(setRotatedAngle) * rotatedScalar;
			tempY = (float)Math.Sin(setRotatedAngle) * rotatedScalar * -1;

			Normalize.X += tempX;
			Normalize.Y += tempY;

			#endregion

			this.DisappearDelay = 15;
		}

		public void Update(float deltaRatio)
		{
			if (this.IsHit)
			{
				if (this.DisappearDelay > 0)
				{
					this.DisappearDelay -= deltaRatio;
				}
				else
				{
					this.DestoryDelay = 0;
				}

				return;
			}

			if (DestoryDelay > 0)
			{
				DestoryDelay -= deltaRatio;
			}
			else
			{
				return;
			}

			this.StartPosition += (this.Normalize * this.Speed * deltaRatio);
			this.EndPosition = this.StartPosition + (this.Normalize * this.Length);
		}

		public bool IsDisabled()
		{
			if (DestoryDelay > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public Vector2D GetStartVector()
		{
			return this.StartPosition;
		}

		public Vector2D GetEndVector()
		{
			return this.EndPosition;
		}

		public void SetHit(int x, int y)
		{
			this.IsHit = true;

			this.EndPosition.X = -1000;
			this.EndPosition.Y = -1000;
			this.StartPosition.X = -1000;
			this.StartPosition.Y = -1000;

			this.HitPosition.X = x;
			this.HitPosition.Y = y;
		}

		public void Draw(Graphics graphics)
		{
			if (IsHit)
			{
				// Draw Hit Effect
				int radius = (int)(this.DisappearDelay / 2);
				SolidBrush solidBrush = new SolidBrush(Color.FromArgb(200, 50, 0));
				int x = this.HitPosition.X - this.Manager.GameCamera.GetX();
				int y = this.HitPosition.Y - this.Manager.GameCamera.GetY();
				graphics.FillEllipse(solidBrush, x - radius, y - radius, (int)this.DisappearDelay, (int)this.DisappearDelay);

				return;
			}

			Vector2D cameraVector = this.Manager.GameCamera.GetVector2D();
			graphics.DrawLine(BulletColor, (this.StartPosition - cameraVector).GetPoint(), (this.EndPosition - cameraVector).GetPoint());
		}
	}
}
