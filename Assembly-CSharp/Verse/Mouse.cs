using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9C RID: 3740
	public static class Mouse
	{
		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06005840 RID: 22592 RVA: 0x002D31C8 File Offset: 0x002D15C8
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x002D3230 File Offset: 0x002D1630
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
