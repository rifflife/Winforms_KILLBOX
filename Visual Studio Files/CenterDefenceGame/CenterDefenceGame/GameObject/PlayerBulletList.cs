using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject
{
	public class PlayerBulletList
	{
		private GameManager Manager;
		public PlayerBullet[] Bullets;
		public readonly int MaxCount = 2000;

		public PlayerBulletList(GameManager gameManager)
		{
			this.Manager = gameManager;
			this.Bullets = new PlayerBullet[this.MaxCount];
		}

		public void Reset()
		{
			for (int clear = 0; clear < this.MaxCount; clear ++)
			{
				this.Bullets[clear] = null;
			}
		}

		public void Update(float deltaRatio)
		{
			for(int index = 0; index < this.MaxCount ; index ++)
			{
				if (this.Bullets[index] != null)
				{
					this.Bullets[index].Update(deltaRatio);

					if(this.Bullets[index].IsDisabled())
					{
						this.Bullets[index] = null;
					}
				}
			}
		}

		public void Draw(Graphics graphics)
		{
			// Draw Player Bullt
			for(int index = 0; index < this.MaxCount ; index ++)
			{
				if(this.Bullets[index] != null)
				{
					this.Bullets[index].Draw(graphics);
				}
			}
		}
		
		public bool CreateBullet(PlayerBullet playerBullet)
		{
			for(int index = 0; index < this.MaxCount ; index ++)
			{
				if(this.Bullets[index] == null)
				{
					this.Bullets[index] = playerBullet;
					return true;
				}
			}
			return false;
		}
	}
}
