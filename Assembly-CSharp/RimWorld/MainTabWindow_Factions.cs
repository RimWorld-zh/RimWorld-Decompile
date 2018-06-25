using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200086F RID: 2159
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x04001A83 RID: 6787
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001A84 RID: 6788
		private float scrollViewHeight;

		// Token: 0x06003117 RID: 12567 RVA: 0x001AA7BE File Offset: 0x001A8BBE
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}
	}
}
