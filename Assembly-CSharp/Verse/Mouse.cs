using UnityEngine;

namespace Verse
{
	public static class Mouse
	{
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				if (windowStack.MouseObscuredNow)
				{
					return true;
				}
				if (!windowStack.CurrentWindowGetsInput)
				{
					return true;
				}
				return false;
			}
		}

		public static bool IsOver(Rect rect)
		{
			if (rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow)
			{
				return true;
			}
			return false;
		}
	}
}
