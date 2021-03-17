using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.EnemyObject
{
	public class EnemyCustom : EnemyType
	{
		public EnemyCustom(GameManager gameManager, int x, int y, float moveSpeed, int healthPointMax, float attackDelayIni, int attackReach, int damage, EGoDirection pathToGo, int dropResource)
			: base(gameManager, x, y, moveSpeed, healthPointMax, attackDelayIni, attackReach, damage, dropResource, pathToGo)
		{
			#region Enemy Body Polygons Setting

			this.Width = 30;
			this.Height = 30;

			// 초기 적 시선은 아래
			this.Position.Rotation = (float)(270 * Math.PI / 180);

			// Enemy Bodies Color Set
			this.BodyColors = new Color[5];
			// Body
			this.BodyColors[0] = Color.FromArgb(240, 100, 100);
			// Arms
			this.BodyColors[1] = Color.FromArgb(240, 50, 50);
			// Head
			this.BodyColors[2] = Color.FromArgb(240, 30, 30);
			// Gun
			this.BodyColors[3] = Color.FromArgb(30, 30, 30);
			// If Enemy takes damage
			this.BodyColors[4] = Color.FromArgb(255, 0, 0);

			this.BodyPolygons = new Polygon2D[4];

			// Body Size
			int bWidth = 30;
			int bHeight = 23;
			int bOffset = 2;
			this.BodyPolygons[0] = new Polygon2D(x, y, -bWidth / 2, -bHeight / 2 + bOffset, bWidth, bHeight, this.BodyColors[0]);

			// Arms Size
			int aWidth = 15;
			int aHeight = 15;
			int aOffset = 17;
			this.BodyPolygons[1] = new Polygon2D(x, y, -aWidth - aOffset, -aHeight / 2, aHeight, aWidth, this.BodyColors[1]);
			this.BodyPolygons[2] = new Polygon2D(x, y, aOffset, -aHeight / 2, aHeight, aWidth, this.BodyColors[1]);

			// Head Size
			int hWidth = 14;
			int hHeight = 14;
			this.BodyPolygons[3] = new Polygon2D(x, y, -hHeight / 2, -hWidth / 2, hWidth, hHeight, this.BodyColors[2]);

			#endregion
		}
	}
}
