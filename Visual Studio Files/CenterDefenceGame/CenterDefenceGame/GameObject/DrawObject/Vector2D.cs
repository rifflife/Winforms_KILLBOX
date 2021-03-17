using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.DrawObject
{
	public class Vector2D
	{

		// 원점 좌표
		public float X;
		public float Y;
		
		// 상대 좌표
		public float VX;
		public float VY;

		// 가상 렌더링 좌표 (원점 좌표와 상대 좌표의 합)
		public float RX;
		public float RY;

		// 회전 변수
		public float Rotation;

		#region Constructor

		public Vector2D(float x, float y, float vx, float vy)
		{
			this.X = x;
			this.Y = y;
			this.VX = vx;
			this.VY = vy;
			this.Rotation = 0;

			this.RX = 0;
			this.RY = 0;
			this.Rotate(0);
		}

		public Vector2D(int x, int y, int vx, int vy)
		{
			this.X = x;
			this.Y = y;
			this.VX = vx;
			this.VY = vy;
			this.Rotation = 0;

			this.RX = 0;
			this.RY = 0;
			this.Rotate(0);
		}

		public Vector2D(float x, float y)
		{
			this.X = x;
			this.Y = y;
			this.VX = 0;
			this.VY = 0;
			this.Rotation = 0;

			this.RX = 0;
			this.RY = 0;
			//this.Rotate(0);
		}

		public Vector2D(int x, int y)
		{
			this.X = x;
			this.Y = y;
			this.VX = 0;
			this.VY = 0;
			this.Rotation = 0;

			this.RX = 0;
			this.RY = 0;
			this.Rotate(0);
		}

		#endregion
		
		#region Operator

		public static Vector2D operator+ (Vector2D vec1, Vector2D vec2)
		{
			return new Vector2D(vec1.X + vec2.X, vec1.Y + vec2.Y);
		}
		
		public static Vector2D operator- (Vector2D vec1, Vector2D vec2)
		{
			return new Vector2D(vec1.X - vec2.X, vec1.Y - vec2.Y);
		}

		public static Vector2D operator* (Vector2D vec1, Vector2D vec2)
		{
			return new Vector2D(vec1.X * vec2.X, vec1.Y * vec2.Y);
		}

		public static Vector2D operator/ (Vector2D vec1, Vector2D vec2)
		{
			return new Vector2D(vec1.X / vec2.X, vec1.Y / vec2.Y);
		}

		public static Vector2D operator* (Vector2D vec, int mul)
		{
			return new Vector2D(vec.X * mul, vec.Y * mul);
		}
		
		public static Vector2D operator* (Vector2D vec, float mul)
		{
			return new Vector2D(vec.X * mul, vec.Y * mul);
		}

		public static Vector2D operator/ (Vector2D vec, int mul)
		{
			if (mul != 0)
			{
				return new Vector2D(vec.X / mul, vec.Y / mul);
			}
			else
			{
				return new Vector2D(0, 0);
			}
		}

		#endregion

		public Vector2D GetNormalize()
		{
			float scalar = (float)Math.Sqrt(X * X + Y * Y);
			return new Vector2D(X / scalar, Y / scalar);
		}

		public Point GetPoint()
		{
			return new Point((int)this.X, (int)this.Y);
		}

		public float GetScalar()
		{
			return (float)Math.Sqrt(X * X + Y * Y);
		}

		public override string ToString()
		{
			return $"({X} , {Y})";
		}

		/// <summary>
		/// Vector2D의 회전 렌더링 좌표를 회전값으로 설정합니다.
		/// </summary>
		/// <param name="rotation">호도법 각도</param>
		public void Rotate(float rotation)
		{
			this.Rotation = rotation;

			float tempX = VX;
			float tempY = VY;

			// X Y 평면상에서 회전
			float setRotatedAngle = 0;
			float rotatedScalar = (float)Math.Sqrt(tempX * tempX + tempY * tempY);
			setRotatedAngle = (float)Math.Atan2(-tempY, tempX) + this.Rotation;
			// 360도
			//setRotatedAngle = (float)Math.Atan2(-tempY, tempX) + (this.Rotation * (float)Math.PI / 180f);

			tempX = (float)Math.Cos(setRotatedAngle) * rotatedScalar;
			tempY = (float)Math.Sin(setRotatedAngle) * rotatedScalar * -1;

			this.RX = this.X + tempX;
			this.RY = this.Y + tempY;
		}

	}
}
