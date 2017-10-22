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
				return (byte)(windowStack.MouseObscuredNow ? 1 : ((!windowStack.CurrentWindowGetsInput) ? 1 : 0)) != 0;
			}
		}

		public static bool IsOver(Rect rect)
		{
			return (byte)((rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow) ? 1 : 0) != 0;
		}
	}
}
