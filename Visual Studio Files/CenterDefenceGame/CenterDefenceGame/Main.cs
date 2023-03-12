using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// 게임 오브젝트 클래스 정의
using CenterDefenceGame.GameObject;

namespace CenterDefenceGame
{
    public partial class Main : Form
	{
		// System
		public bool IsDebugging = false;

		// Delta Time
		private DateTime PreviousTime;
		private TimeSpan DeltaTime;

		public float DeltaRatio;
		private readonly float DeltaConst = 1000 / 60;

		// Graphics
		private Bitmap GameBitmap;
		private Graphics GameGraphics;

		// Key Input
		public Dictionary<Keys, bool> IsKeyPressed;
		public Dictionary<MouseButtons, bool> IsMousePressed;

		// Mouse Cursor
		public Point InitializeMouseCursorPosition;
		public Point MouseMovedAmount;

		// GameManager Object
		private GameManager Manager;

		public Main()
		{
			InitializeComponent();

			// Initialize Delta Time
			this.DeltaRatio = 0;

			// Initialize Input Keys
			this.IsKeyPressed = new Dictionary<Keys, bool>();
			this.IsKeyPressed.Add(Keys.W, false);
			this.IsKeyPressed.Add(Keys.A, false);
			this.IsKeyPressed.Add(Keys.S, false);
			this.IsKeyPressed.Add(Keys.D, false);
			this.IsKeyPressed.Add(Keys.Escape, false);
			this.IsKeyPressed.Add(Keys.F1, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F2, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F3, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F4, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F5, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F6, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F7, false); // Debugging Key
			this.IsKeyPressed.Add(Keys.F8, false); // Debugging Key
			
			// Initialize Input Mouse Buttons
			this.IsMousePressed = new Dictionary<MouseButtons, bool>();
			this.IsMousePressed.Add(MouseButtons.Left, false);
			this.IsMousePressed.Add(MouseButtons.Right, false);
			this.IsMousePressed.Add(MouseButtons.Middle, false);

			// Initialize Mouse Position
			this.InitializeMouseCursorPosition = new Point(0, 0);
			this.MouseMovedAmount = new Point(0, 0);
			this.InitializeMouseCursorPosition.X = this.Location.X + this.ClientSize.Width / 2;
			this.InitializeMouseCursorPosition.Y = this.Location.Y + this.ClientSize.Width / 2;

			// Initialize Double Buffering
			this.GameBitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			this.GameGraphics = Graphics.FromImage(GameBitmap);

			// Initialize All Game Components
			this.Manager = new GameManager(this);

			// Initialize Graphics
			//this.GameGraphics.SmoothingMode = SmoothingMode.HighQuality;
			
			this.Activate();

        }

		private bool _isLoaded = false;
        private void Main_Load(object sender, EventArgs e)
        {
			if (_isLoaded)
				return;
			_isLoaded = true;

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    this?.InvokeControl(() =>
                    {
                        this?.UpdateGame();
                    });
                    Thread.Sleep(1);
                }
            });
            t.IsBackground = false;
            t.Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e)	{}

		# region Set Key Input

		private void Main_KeyDown(object sender, KeyEventArgs e)
		{
			this.SetKeyInput(e.KeyCode, true);
		}

		private void Main_KeyUp(object sender, KeyEventArgs e)
		{
			this.SetKeyInput(e.KeyCode, false);
		}

		private void SetKeyInput(Keys key, bool isPressed)
		{
			if (!this.IsKeyPressed.ContainsKey(key))
			{
				return;
			}

			this.IsKeyPressed[key] = isPressed;
		}

		#endregion
		
		# region Set Mouse Input

		private void SetMouseInput(MouseButtons button, bool isPressed)
		{
			if (!this.IsMousePressed.ContainsKey(button))
			{
				return;
			}

			this.IsMousePressed[button] = isPressed;
		}

		private void Main_MouseDown(object sender, MouseEventArgs e)
		{
			this.SetMouseInput(e.Button, true);
		}

		private void Main_MouseUp(object sender, MouseEventArgs e)
		{
			this.SetMouseInput(e.Button, false);
		}

		#endregion

        public void UpdateGame()
        {
            // Mouse Cursor
            Cursor.Hide();
            Text = "KILL BOX (202013207 최지욱)";
            if (this.Focused)
            {
                this.Activate();
                if (this.Manager.GameMouse.GetVirtualMouseMode())
                {
                    this.MouseMovedAmount.X = Cursor.Position.X - this.InitializeMouseCursorPosition.X;
                    this.MouseMovedAmount.Y = Cursor.Position.Y - this.InitializeMouseCursorPosition.Y;
                    Cursor.Position = this.InitializeMouseCursorPosition;
                }
                else
                {
                    this.MouseMovedAmount.X = 0;
                    this.MouseMovedAmount.Y = 0;
                }
            }
            else
            {
                this.Manager.SetGameSituation(ESystemSituation.Pause);
            }

            // Delta Set
            this.DeltaTime = DateTime.Now - this.PreviousTime;
            this.PreviousTime = DateTime.Now;
            this.DeltaRatio = (this.DeltaTime.Milliseconds / this.DeltaConst);

            // Game Update
            this.Manager.Update(this.DeltaRatio, GameGraphics, this.MouseMovedAmount, this.PointToClient(Cursor.Position));

            this.Invalidate();
        }
		
		private void Main_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(GameBitmap, 0, 0);
		}

		public void ResetMousePosition()
		{
			Cursor.Position = this.InitializeMouseCursorPosition;
		}

		private void FormSizeOrLocationChanged(object sender, EventArgs e)
		{
			this.InitializeMouseCursorPosition.X = this.Location.X + this.ClientSize.Width  / 2;
			this.InitializeMouseCursorPosition.Y = this.Location.Y + this.ClientSize.Height / 2;
		}
    }
}
