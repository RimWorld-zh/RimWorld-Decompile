using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200086D RID: 2157
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x04001A83 RID: 6787
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001A84 RID: 6788
		private float scrollViewHeight;

		// Token: 0x06003113 RID: 12563 RVA: 0x001AA66E File Offset: 0x001A8A6E
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}
	}
}
