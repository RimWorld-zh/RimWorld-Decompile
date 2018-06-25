using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200086F RID: 2159
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x04001A87 RID: 6791
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001A88 RID: 6792
		private float scrollViewHeight;

		// Token: 0x06003116 RID: 12566 RVA: 0x001AAA26 File Offset: 0x001A8E26
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}
	}
}
