using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000871 RID: 2161
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x0600311A RID: 12570 RVA: 0x001AA486 File Offset: 0x001A8886
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}

		// Token: 0x04001A85 RID: 6789
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001A86 RID: 6790
		private float scrollViewHeight;
	}
}
