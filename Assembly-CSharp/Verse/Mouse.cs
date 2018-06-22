using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9B RID: 3739
	public static class Mouse
	{
		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06005860 RID: 22624 RVA: 0x002D4DD8 File Offset: 0x002D31D8
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x002D4E40 File Offset: 0x002D3240
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
