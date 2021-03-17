using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.DrawObject
{
	public class ImageButton : Button
	{
		public ImageButton(GameManager gameManager, ButtonDelegate buttonAction, bool isActiveReleased, int xPositionFromDock, int yPositionFromDock,
						   int width, int height, Image image, float imageScale, EDock dockHorizontal, EDock dockVertical)
			:base(gameManager, buttonAction, isActiveReleased, xPositionFromDock, yPositionFromDock, width, height, dockHorizontal, dockVertical)
		{
			this.bitmap = this.Manager.GetImageButtonBitmap(image, this.Width, this.Height, imageScale);
		}
	}
}
