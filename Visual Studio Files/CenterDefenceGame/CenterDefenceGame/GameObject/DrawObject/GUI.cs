using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterDefenceGame.GameObject.DrawObject
{
	public class GUI
	{
		private GameManager Manager;

		#region GUI Window Size

		private int FormWidth;
		private int FormHeight;
		private int GuiOffset;

		#endregion

		#region GUI Colors

		private Color FillColor;
		private Color ForegroundColor;
		private SolidBrush FillBrush;
		private SolidBrush ForegroundBrush;

		#endregion

		#region Health Bar Variables

		private int HealthBarX;
		private int HealthBarY;
		private int HealthBarWidth;
		private int HealthBarHeight;
		private int ForeHealthBarX;
		private int ForeHealthBarY;
		private int ForeHealthBarHeight;
		private float HealthLeftRatio;

		#endregion

		#region Health Bar Outline

		private Pen HealthBarOutlinePen;
		private Pen HealthBarForeOutlinePen;
		private int HealthBarOutlineX;
		private int HealthBarOutlineY;
		private int HealthBarOutlineWidth;
		private int HealthBarOutlineHeight;

		#endregion

		#region Core Health Bar

		private int ForeCoreBarY;
		private int ForeCoreBarHeight;

		private int CoreBarY;
		private int CoreBarHeight;
		private int CoreBarOutlineHeight;
		private int CoreBarOutlineY;
		private float CoreLeftRatio;
		private SolidBrush CoreFillBrush = new SolidBrush(Color.FromArgb(100, 255, 75));

		#endregion

		#region External Iamges

		private Image IconAmmunition;
		private Image IconResource;

		#endregion

		#region Resource and Ammunition Counters

		private int CounterWidth = 200;
		private int CounterHeight = 40;
		
		// Resource
		bool IsResourceChanged = false;
		private Bitmap ResourceNumberBitmap;
		private int ResourceCount;
		private Point ResourceTextPosition;
		private Rectangle ResourceIconRectangle;
		private Rectangle ResourceBackgroundRectangle;

		// Ammunition
		bool IsAmmunitionChanged = false;
		private Bitmap AmmunitionNumberBitmap;
		private int AmmunitionCount;
		private Point AmmunitionTextPosition;
		private Rectangle AmmunitionIconRectangle;
		private Rectangle AmmunitionBackgroundRectangle;

		#endregion

		#region Notification
		
		private List<Bitmap>	NotificationBitmaps;
		private List<float>		NotificationAlphaList;
		private float			NotificationAlphaDecrease = 0.01f;
		private int				NotificationMaxCount;
		private int				NotificationMinCount = 5;

		// PositionX from left side, PositionY from bottom side
		private Point			NotificationPosition = new Point(30, 150);
		// Notification will be added bottom to top
		private int				NotificationOffset = 20;

		private string			NotificationFontName = "맑은 고딕";
		private int				NotificationFontSize = 14;

		#endregion

		#region Timer
		
		bool IsTimerCountChanged = false;
		private int				TimerBitmapWidth = 120;
		private int				TimerBitmapHeight;
		private int				TimerFontSize = 70;
		private int				TimerCount;
		private Bitmap			TimerBitmap;
		private Point			TimerPosition;
		private string			TimerFontName;

		#endregion

		#region Wave Counter

		bool IsWaveCountChanged = false;
		private int				WaveBitmapWidth = 120;
		private int				WaveBitmapHeight;
		private int				WaveFontSize = 15;
		private int				WaveCount;
		private Bitmap			WaveBitmap;
		private Point			WavePosition;
		private string			WaveFontName;

		#endregion

		#region Enemy Left Counter
		
		bool IsEnemyCountChanged = false;
		private int				EnemyBitmapWidth = 100;
		private int				EnemyBitmapHeight;
		private int				EnemyFontSize = 40;
		private int				EnemyCount;
		private Bitmap			EnemyBitmap;
		private string			EnemyFontName;
		private Bitmap EnemyCounterText;

		#endregion

		public GUI(GameManager gameManager)
		{
			this.Manager = gameManager;

			this.IconAmmunition = Image.FromFile("IconAmmunition.png");
			this.IconResource = Image.FromFile("IconResource.png");

			this.NotificationBitmaps	= new List<Bitmap>();
			this.NotificationAlphaList	= new List<float>();

			this.Reset();
		}

		public void Reset()
		{
			#region Reset GUI Window Size

			this.FormWidth  = this.Manager.MainForm.ClientSize.Width;
			this.FormHeight = this.Manager.MainForm.ClientSize.Height;
			int formHalfWidth	= this.FormWidth / 2;
			int formHalfHeight	= this.FormHeight / 2;

			int offsetY = 50;

			this.GuiOffset = 2;
			
			#endregion

			#region Reset Colors

			this.FillColor = Color.White;
			this.FillBrush = new SolidBrush(FillColor);
			this.ForegroundColor = this.Manager.BackgroundColor;
			this.ForegroundBrush = new SolidBrush(ForegroundColor);

			#endregion

			#region Reset Health Bar Position

			// Health Bar
			this.HealthLeftRatio = 0;
			
			this.HealthBarWidth		= 300;
			this.HealthBarHeight	= GuiOffset * 6;
			this.HealthBarX = formHalfWidth - this.HealthBarWidth / 2;
			this.HealthBarY = this.FormHeight - offsetY + HealthBarHeight - 5;

			this.ForeHealthBarX	= this.HealthBarX - this.GuiOffset;
			this.ForeHealthBarY	= this.HealthBarY - this.GuiOffset;
			this.ForeHealthBarHeight = this.HealthBarHeight + this.GuiOffset * 2;

			// Health Bar Outline
			this.HealthBarOutlinePen = new Pen(this.FillColor, this.GuiOffset);
			this.HealthBarForeOutlinePen = new Pen(this.ForegroundColor, this.GuiOffset * 3);
			
			this.HealthBarOutlineX		= this.HealthBarX - this.GuiOffset;
			this.HealthBarOutlineY		= this.HealthBarY - this.GuiOffset;
			this.HealthBarOutlineWidth	= this.HealthBarWidth  + this.GuiOffset * 2;
			this.HealthBarOutlineHeight	= this.HealthBarHeight + this.GuiOffset * 2;

			this.CoreBarHeight = GuiOffset * 10;
			this.ForeCoreBarHeight = this.CoreBarHeight + this.GuiOffset * 2;
			this.CoreBarY      = this.FormHeight - offsetY - HealthBarHeight - 5;
			this.ForeCoreBarY  = this.CoreBarY - this.GuiOffset;
			this.CoreBarOutlineY = this.CoreBarY - this.GuiOffset;
			this.CoreBarOutlineHeight = this.CoreBarHeight + this.GuiOffset * 2;
			this.CoreLeftRatio = 0;

			#endregion

			#region Reset HMD Position
			
			int backRadius = 27;
			int offsetHMD = 230;
			int offsetText = 40;
			int offsetCounterY = this.FormHeight - offsetY;

			// 아이콘은 센터 포지션 텍스트는 Y는 센터 X는 좌측
			// Resource
			
			this.IsResourceChanged = false;
			this.ResourceCount = 0;
			int resourceCountIconX = formHalfWidth - offsetHMD;
			int resourceCountIconY = this.FormHeight - offsetY;
			int resourceCountTextX = resourceCountIconX - offsetText - this.CounterWidth;

			int rhWidth  = IconResource.Width  / 2;
			int rhHeight = IconResource.Height / 2;

			this.ResourceTextPosition = new Point(resourceCountTextX, offsetCounterY - (this.CounterHeight / 2));
			this.ResourceIconRectangle = new Rectangle(resourceCountIconX - rhWidth, resourceCountIconY - rhHeight, IconResource.Width, IconResource.Height);
			this.ResourceBackgroundRectangle = new Rectangle(resourceCountIconX - backRadius, resourceCountIconY - backRadius, backRadius * 2, backRadius * 2);

			// Ammunition
			
			this.IsAmmunitionChanged = false;
			this.AmmunitionCount = 0;
			int ammunitionCountIconX = formHalfWidth + offsetHMD;
			int ammunitionCountIconY = this.FormHeight - offsetY;
			int ammunitionCountTextX = ammunitionCountIconX + offsetText;
			
			int ahWidth  = IconAmmunition.Width  / 2;
			int ahHeight = IconAmmunition.Height / 2;

			this.AmmunitionTextPosition = new Point(ammunitionCountTextX, offsetCounterY - (this.CounterHeight / 2));
			this.AmmunitionIconRectangle = new Rectangle(ammunitionCountIconX - ahWidth, ammunitionCountIconY - ahHeight, IconAmmunition.Width, IconAmmunition.Height);
			this.AmmunitionBackgroundRectangle = new Rectangle(ammunitionCountIconX - backRadius, ammunitionCountIconY - backRadius, backRadius * 2, backRadius * 2);

			#endregion

			#region Reset Notification Text

			this.NotificationMaxCount	= NotificationMinCount;

			// Initialize Lists
			if (this.NotificationBitmaps.Count > 0)
			{
				for (int clear = 0; clear < this.NotificationBitmaps.Count; clear ++)
				{
					this.NotificationBitmaps[clear].Dispose();
				}

				this.NotificationBitmaps.Clear();
			}

			#endregion

			#region Reset Timer

			this.IsTimerCountChanged = false;
			this.TimerCount = 0;
			this.TimerBitmapHeight	= this.TimerFontSize + 10;
			this.TimerBitmap		= new Bitmap(this.TimerBitmapWidth, this.TimerBitmapHeight);
			this.TimerPosition		= new Point((this.FormWidth - this.TimerBitmapWidth) / 2, 20);
			this.TimerFontName		= "Arial";

			#endregion
			
			#region Reset Wave Counter

			this.IsWaveCountChanged = false;
			this.WaveCount		    = 0;
			this.WaveBitmapHeight	= this.WaveFontSize + 10;
			this.WaveBitmap		    = new Bitmap(this.WaveBitmapWidth, this.WaveBitmapHeight);
			this.WavePosition		= new Point((this.FormWidth - this.WaveBitmapWidth) / 2, this.TimerPosition.Y + this.TimerBitmapHeight);
			this.WaveFontName		= "Arial";

			#endregion
			
			#region Reset Enemy Counter

			this.IsEnemyCountChanged = false;
			this.EnemyCount		    = 0;
			this.EnemyBitmapHeight	= this.EnemyFontSize + 10;
			this.EnemyBitmap		= new Bitmap(this.EnemyBitmapWidth, this.EnemyBitmapHeight);
			this.EnemyFontName		= "Arial";
			EnemyCounterText = this.Manager.GetTextBitmap("대기중인 적", 3, Color.White, ForegroundColor, this.EnemyBitmapWidth, 20, "맑은 고딕", (int)FontStyle.Bold, 15, 0);

			#endregion
		}

		public void Update()
		{
			this.HealthLeftRatio = this.Manager.GamePlayer.GetHealthPointRatio();
			this.CoreLeftRatio = this.Manager.GameMap.GetCoreHealthRatio();

			this.SetResourceCounter();
			this.SetAmmunitionCounter();
			this.SetWaveCounter();
			this.SetTimer();
			this.SetEnemyCounter();
		}

		public void DrawHMD(Graphics graphics)
		{
			#region Draw Health Bar And Core Bar

			// Draw Health Bar
			int healthBarLength = (int)(this.HealthBarWidth * this.HealthLeftRatio);
			int foreHealthBarLength = healthBarLength + this.GuiOffset * 2;

			if (this.HealthLeftRatio > 0)
			{
				graphics.FillRectangle(ForegroundBrush, ForeHealthBarX, ForeHealthBarY, foreHealthBarLength, ForeHealthBarHeight);
				graphics.FillRectangle(FillBrush, HealthBarX, HealthBarY, healthBarLength, HealthBarHeight);
			}

			// Draw Core Bar

			int coreBarLength = (int)(this.HealthBarWidth * this.CoreLeftRatio);
			int foreCoreLength = coreBarLength + this.GuiOffset * 2;
			
			if (this.CoreLeftRatio > 0)
			{
				graphics.FillRectangle(ForegroundBrush, ForeHealthBarX, ForeCoreBarY, foreCoreLength, ForeCoreBarHeight);
				graphics.FillRectangle(CoreFillBrush, HealthBarX, CoreBarY, coreBarLength, CoreBarHeight);
			}
			
			graphics.DrawRectangle(this.HealthBarForeOutlinePen, HealthBarOutlineX, HealthBarOutlineY, HealthBarOutlineWidth, HealthBarOutlineHeight);
			graphics.DrawRectangle(this.HealthBarForeOutlinePen, HealthBarOutlineX, CoreBarOutlineY, HealthBarOutlineWidth, CoreBarOutlineHeight);
			graphics.DrawRectangle(this.HealthBarOutlinePen, HealthBarOutlineX, HealthBarOutlineY, HealthBarOutlineWidth, HealthBarOutlineHeight);
			graphics.DrawRectangle(this.HealthBarOutlinePen, HealthBarOutlineX, CoreBarOutlineY, HealthBarOutlineWidth, CoreBarOutlineHeight);

			#endregion

			#region Draw Counter Text And Icon

			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.FillEllipse(this.ForegroundBrush, this.ResourceBackgroundRectangle);
			graphics.FillEllipse(this.ForegroundBrush, this.AmmunitionBackgroundRectangle);
			graphics.SmoothingMode = SmoothingMode.None;


			graphics.DrawImage(this.IconResource, ResourceIconRectangle);
			graphics.DrawImage(this.IconAmmunition, AmmunitionIconRectangle);
			graphics.DrawImage(this.ResourceNumberBitmap, this.ResourceTextPosition);
			graphics.DrawImage(this.AmmunitionNumberBitmap, this.AmmunitionTextPosition);

			#endregion

			#region Draw Top Section

			// Draw Timer
			graphics.DrawImage(this.TimerBitmap, this.TimerPosition);
			// Wave Timer
			graphics.DrawImage(this.WaveBitmap, this.WavePosition);
			// Enemy Counter
			graphics.DrawImage(this.EnemyBitmap, this.FormWidth - 20 - this.EnemyBitmapWidth, 30);
			graphics.DrawImage(EnemyCounterText, this.FormWidth - 20 - this.EnemyBitmapWidth, 80);

			#endregion
		}

		public void DrawNotification(Graphics graphics)
		{
			#region Draw Notifications

			for (int alphaCount = 0; alphaCount < this.NotificationAlphaList.Count(); alphaCount ++)
			{
				if (this.NotificationAlphaList[alphaCount] > this.NotificationAlphaDecrease)
				{
					this.NotificationAlphaList[alphaCount] -= this.NotificationAlphaDecrease;
				}
				else
				{
					this.NotificationAlphaList[alphaCount] = 0f;
				}
			}

			int stringCount = 0;

			// 문자열이 출력된 비트맵을 순차적으로 출력한다.
			foreach(Bitmap bitmap in this.NotificationBitmaps)
			{
				if (this.NotificationAlphaList[stringCount] > 0)
				{
					ColorMatrix cm = new ColorMatrix();
					cm.Matrix33 = this.NotificationAlphaList[stringCount];
					ImageAttributes ia = new ImageAttributes();
					ia.SetColorMatrix(cm);
					int drawY = this.FormHeight - (this.NotificationPosition.Y + this.NotificationOffset * stringCount);
					Rectangle rectangle = new Rectangle(NotificationPosition.X, drawY, bitmap.Width, bitmap.Height);
					
					graphics.DrawImage(bitmap, rectangle, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);

					ia.Dispose();
				}

				stringCount++;
			}

			#endregion
		}

		public void SetResourceCounter()
		{
			int resource = this.Manager.GetResourceCount();
			if (this.ResourceCount == resource)
			{
				this.IsResourceChanged = false;
				return;
			}
			this.IsResourceChanged = true;
			this.ResourceCount = resource;

			if (this.ResourceNumberBitmap != null)
			{
				this.ResourceNumberBitmap.Dispose();
			}

			this.ResourceNumberBitmap = this.Manager.GetTextBitmap(this.ResourceCount.ToString(), GuiOffset * 3, Color.White, ForegroundColor,
																   this.CounterWidth, this.CounterHeight, "Arial", (int)FontStyle.Bold, 35, 1);
		}
		
		public void SetAmmunitionCounter()
		{
			int ammunition = this.Manager.GetAmmunitionCount();
			if (this.AmmunitionCount == ammunition)
			{
				this.IsAmmunitionChanged = false;
				return;
			}
			this.IsAmmunitionChanged = true;
			this.AmmunitionCount = ammunition;

			if (this.AmmunitionNumberBitmap != null)
			{
				this.AmmunitionNumberBitmap.Dispose();
			}

			this.AmmunitionNumberBitmap = this.Manager.GetTextBitmap(this.AmmunitionCount.ToString(), GuiOffset * 3, Color.White, ForegroundColor,
																   this.CounterWidth, this.CounterHeight, "Arial", (int)FontStyle.Bold, 35, -1);
		}

		public void SetTimer()
		{
			int second = this.Manager.GetTimer();
			if (second == this.TimerCount)
			{
				this.IsTimerCountChanged = false;
				return;
			}

			this.IsTimerCountChanged = true;
			this.TimerCount = second;

			if (this.TimerBitmap != null)
			{
				this.TimerBitmap.Dispose();
			}

			this.TimerBitmap = this.Manager.GetTextBitmap(TimerCount.ToString(), 6, Color.White, ForegroundColor, TimerBitmapWidth,
														  TimerBitmapHeight, TimerFontName, (int)FontStyle.Bold, TimerFontSize, 0);
		}
		
		public void SetWaveCounter()
		{
			int wave = this.Manager.GetWave();
			if (wave == this.WaveCount)
			{
				this.IsWaveCountChanged = false;
				return;
			}

			this.IsWaveCountChanged = true;
			this.WaveCount = wave;

			if (this.WaveBitmap != null)
			{
				this.WaveBitmap.Dispose();
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("- WAVE ");
			sb.Append(this.WaveCount);
			sb.Append("-");

			this.WaveBitmap = this.Manager.GetTextBitmap(sb.ToString(), 3, Color.White, ForegroundColor, WaveBitmapWidth,
														  WaveBitmapHeight, WaveFontName, (int)FontStyle.Bold, WaveFontSize, 0);
		}
		
		public void SetEnemyCounter()
		{
			int enemyLeft = this.Manager.GameLevelManager.GetEnemyLeft();
			if (enemyLeft == this.EnemyCount)
			{
				this.IsEnemyCountChanged = false;
				return;
			}

			this.IsEnemyCountChanged = true;
			this.EnemyCount = enemyLeft;

			if (this.EnemyBitmap != null)
			{
				this.EnemyBitmap.Dispose();
			}

			this.EnemyBitmap = this.Manager.GetTextBitmap(this.EnemyCount.ToString(), 3, Color.White, ForegroundColor, EnemyBitmapWidth,
														  EnemyBitmapHeight, EnemyFontName, (int)FontStyle.Bold, EnemyFontSize, 0);
		}

		public void AddNotification(EMessageType type, string text)
		{
			int bitmapHeight =  NotificationFontSize + 10;
			
			string prefix = "";
			
			Color textColor = new Color();
			switch(type)
			{
				case EMessageType.Normal:
					textColor = Color.White;
					break;

				case EMessageType.Debug:
					prefix = "Debug : ";
					textColor = Color.LightGreen;
					break;

				case EMessageType.System:
					prefix = "System : ";
					textColor = Color.Orange;
					break;

				case EMessageType.Warring:
					prefix = "System : [WARRING] ";
					textColor = Color.OrangeRed;
					break;
						
				case EMessageType.Error:
					prefix = "System : [ERROR] ";
					textColor = Color.Red;
					break;
			}

			StringBuilder textString = new StringBuilder();

			textString.Append(prefix);
			textString.Append(text);

			Bitmap textBitmap = this.Manager.GetTextBitmap(textString.ToString(), 2, textColor, Color.Black, 600, bitmapHeight,
												  NotificationFontName, (int)FontStyle.Bold, NotificationFontSize, -1);

			this.NotificationBitmaps.Insert(0, textBitmap);
			this.NotificationAlphaList.Insert(0, 3f);
			
			if (this.NotificationBitmaps.Count() > this.NotificationMaxCount)
			{
				this.NotificationBitmaps[this.NotificationMaxCount].Dispose();
				this.NotificationBitmaps.RemoveAt(this.NotificationMaxCount);
				this.NotificationAlphaList.RemoveAt(this.NotificationMaxCount);
			}
		}

		#region Getter Setter

		public void SetNotificationMaxCount(int counts)
		{
			if (counts < NotificationMinCount)
			{
				counts = NotificationMinCount;
			}

			if (counts > 25)
			{
				counts = 25;
			}
			
			this.NotificationMaxCount = counts;
		}

		#endregion
	}
}
