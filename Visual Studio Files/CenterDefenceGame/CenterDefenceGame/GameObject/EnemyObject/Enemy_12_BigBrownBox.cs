using CenterDefenceGame.GameObject.DrawObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.EnemyObject
{
	// 약하고 빠르며 플레이어를 쫒는다.
	public class Enemy_12_BigBrownBox: EnemyType
	{
		private const int Level = 12;

		public Enemy_12_BigBrownBox(GameManager gameManager, int x, int y)
			: base(gameManager, x, y,
				   DataTable.GetEnemyMoveSpeed    (Level),
				   DataTable.GetEnemyMaxHealth    (Level),
				   DataTable.GetEnemyAttackDelay  (Level),
				   DataTable.GetEnemyAttackReach  (Level),
				   DataTable.GetEnemyDamage       (Level),
				   DataTable.GetEnemyDropResource (Level),
				   DataTable.GetEnemyPathToGo     (Level)
				  )
		{
			#region Enemy Body Polygons Setting

			this.IsBig = true;

			this.Width = 45;
			this.Height = 45;

			// 초기 적 시선은 아래
			this.Position.Rotation = (float)(270 * Math.PI / 180);

			// Enemy Bodies Color Set
			this.BodyColors = new Color[5];
			// Body
			this.BodyColors[0] = Color.FromArgb(239, 135, 78);
			// Arms 1
			this.BodyColors[1] = Color.FromArgb(220, 95, 26);
			// Arms 2
			this.BodyColors[2] = Color.FromArgb(220, 95, 26);
			// Head
			this.BodyColors[3] = Color.FromArgb(200, 70, 0);
			// If Enemy takes damage
			this.BodyColors[4] = Color.FromArgb(255, 213, 190);

			this.BodyPolygons = new Polygon2D[4];

			// Body Size
			int bWidth = 45;
			int bHeight = 40;
			int bOffset = 5;
			this.BodyPolygons[0] = new Polygon2D(x, y, -bWidth / 2, -bHeight / 2 + bOffset, bWidth, bHeight, this.BodyColors[0]);

			// Arms Size
			int aWidth = 23;
			int aHeight = 23;
			int aOffset = 30;
			this.BodyPolygons[1] = new Polygon2D(x, y, -aWidth - aOffset, -aHeight / 2, aHeight, aWidth, this.BodyColors[1]);
			this.BodyPolygons[2] = new Polygon2D(x, y, aOffset, -aHeight / 2, aHeight, aWidth, this.BodyColors[2]);

			// Head Size
			int hWidth = 25;
			int hHeight = 25;
			this.BodyPolygons[3] = new Polygon2D(x, y, -hHeight / 2, -hWidth / 2, hWidth, hHeight, this.BodyColors[3]);

			#endregion
		}
	}
}
