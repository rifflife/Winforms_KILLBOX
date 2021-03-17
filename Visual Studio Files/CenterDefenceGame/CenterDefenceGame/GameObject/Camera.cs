using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject
{
	public class Camera
	{
		private GameManager Manager;
		private float X;
		private float Y;
		private int HalfWidth;
		private int HalfHeight;
		private float Speed;

		public Camera(GameManager gameManager, int x, int y, int width, int height)
		{
			this.Manager = gameManager;
			this.X = x;
			this.Y = y;
			this.HalfWidth  = width  / 2;
			this.HalfHeight = height / 2;
			this.Speed = 0.05f;
		}

		public void Update(float deltaTime, int destinationX, int destinationY)
		{
			this.X = Lerp(this.X, destinationX - HalfWidth, this.Speed * deltaTime);
			this.Y = Lerp(this.Y, destinationY - HalfHeight, this.Speed * deltaTime);
		}

		public float Lerp(float value1, float value2, float ratio)
		{
			return value1 + ((value2 - value1) * ratio);
		}

		public int GetX()
		{
			return (int)X;
		}
		
		public int GetY()
		{
			return (int)Y;
		}
		
		public Vector2D GetVector2D()
		{
			return new Vector2D(X, Y);
		}
	}
}
