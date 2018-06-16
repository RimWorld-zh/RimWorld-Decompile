using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9D RID: 3741
	public static class Mouse
	{
		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06005842 RID: 22594 RVA: 0x002D31C8 File Offset: 0x002D15C8
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x002D3230 File Offset: 0x002D1630
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
