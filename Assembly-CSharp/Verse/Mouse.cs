using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9E RID: 3742
	public static class Mouse
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06005864 RID: 22628 RVA: 0x002D50F0 File Offset: 0x002D34F0
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06005865 RID: 22629 RVA: 0x002D5158 File Offset: 0x002D3558
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
