using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CenterDefenceGame.GameObject;

namespace CenterDefenceGame.GameObject.DrawObject
{
	public class Polygon2D
	{
		private Vector2D[] Vectors;
		private SolidBrush FillColor;
		private float Rotation;
		private Point[] DrawPoints;

		public Polygon2D(int X, int Y, int VX, int VY, int Width, int Height, Color color)
		{
			this.Vectors = new Vector2D[4];
			this.Vectors[0] = new Vector2D(X, Y, VX			, VY			);
			this.Vectors[1] = new Vector2D(X, Y, VX			, VY + Height	);
			this.Vectors[2] = new Vector2D(X, Y, VX + Width	, VY + Height	);
			this.Vectors[3] = new Vector2D(X, Y, VX + Width	, VY			);

			this.FillColor = new SolidBrush(color);
			this.Rotation = 0;

			this.DrawPoints = new Point[4];

			for (int index = 0; index < 4; index ++)
			{
				this.DrawPoints[index] = new Point(0, 0);
			}
		}
		
		public Polygon2D(float X, float Y, int VX, int VY, int Width, int Height, Color color)
		{
			this.Vectors = new Vector2D[4];
			this.Vectors[0] = new Vector2D(X, Y, VX			, VY			);
			this.Vectors[1] = new Vector2D(X, Y, VX			, VY + Height	);
			this.Vectors[2] = new Vector2D(X, Y, VX + Width	, VY + Height	);
			this.Vectors[3] = new Vector2D(X, Y, VX + Width	, VY			);

			this.FillColor = new SolidBrush(color);
			this.Rotation = 0;

			this.DrawPoints = new Point[4];

			for (int index = 0; index < 4; index ++)
			{
				this.DrawPoints[index] = new Point(0, 0);
			}
		}

		public void SetColor(Color color)
		{
			FillColor.Color = color;
		}

		public void SetPosition(float x, float y)
		{
			for (int index = 0; index < Vectors.Count(); index ++)
			{
				this.Vectors[index].X = x;
				this.Vectors[index].Y = y;
			}
		}

		public void Rotate(float rotation)
		{
			this.Rotation = rotation;

			for (int index = 0; index < 4; index ++)
			{
				this.Vectors[index].Rotate(Rotation);
			}
		}

		/// <summary>
		/// 폴리곤을 출력합니다.
		/// </summary>
		/// <param name="g"></param>
		public void Draw(Graphics g, Camera camera)
		{
			this.Rotate(this.Rotation);
			for (int index = 0; index < 4; index ++)
			{
				this.DrawPoints[index].X = (int)Vectors[index].RX - camera.GetX();
				this.DrawPoints[index].Y = (int)Vectors[index].RY - camera.GetY();
			}

			g.FillPolygon(FillColor, this.DrawPoints);
		}
	}
}
