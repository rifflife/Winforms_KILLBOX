using System;
using System.Windows.Forms;

// 게임 오브젝트 클래스 정의

namespace CenterDefenceGame
{
    public static class Util
	{
		public static void InvokeControl(this Control ctl, Action func)
		{
			if (ctl == null)
			{
				return;
			}

			if (ctl.InvokeRequired)
			{
				ctl.Invoke(func); // 스레드 ID가 다르면 대리자로 실행
			}
			else // 스레드 ID가 같으면 그냥 실행
			{
				func();
			}
		}
	}
}
