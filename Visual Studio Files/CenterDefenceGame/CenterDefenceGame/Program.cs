using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterDefenceGame
{
	static class Program
	{
		private static Main _form;

		/// <summary>
		/// 해당 애플리케이션의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            _form = new Main();
			Thread t = new Thread(Update);
			t.IsBackground = false;
			t.Start();
            Application.Run(_form);
		}

		private static void Update()
        {
            Thread.Sleep(1000);
            while (true)
			{
				_form.InvokeControl(() =>
				{
					_form?.UpdateGame();
				});
				Thread.Sleep(1);
			}
        }
	}
}