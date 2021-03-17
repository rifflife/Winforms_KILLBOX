using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterDefenceGame.GameObject.DrawObject
{

	public class TextButton : Button
	{
		//protected GameManager Manager;

		//protected int X;
		//protected int Y;
		//protected int XFromCenter;
		//protected int YFromCenter;
		//protected int HalfWidth;
		//protected int HalfHeight;
		//protected ButtonDelegate ButtonAction;
		//protected Bitmap bitmap;
		//protected bool IsOn;

		public TextButton(GameManager gameManager, ButtonDelegate buttonAction, bool isActiveReleased, int xPositionFromDock, int yPositionFromDock,
						  int width, int height, string textString, string fontName, int fontSize, EDock dockHorizontal, EDock dockVertical)
			: base(gameManager, buttonAction, isActiveReleased, xPositionFromDock, yPositionFromDock, width, height, dockHorizontal, dockVertical)
		{
			this.bitmap = this.Manager.GetTextBitmap(textString, 4, Color.White, this.Manager.BackgroundColor, this.HalfWidth * 2, this.HalfHeight * 2,
														 fontName, (int)FontStyle.Bold, fontSize, 0);
		}
	}
}
