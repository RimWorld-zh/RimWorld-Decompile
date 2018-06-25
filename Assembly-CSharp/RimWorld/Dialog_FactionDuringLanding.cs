using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F8 RID: 2040
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x040017CA RID: 6090
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040017CB RID: 6091
		private float scrollViewHeight;

		// Token: 0x06002D3A RID: 11578 RVA: 0x0017C3DF File Offset: 0x0017A7DF
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x0017C408 File Offset: 0x0017A808
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x0017C42C File Offset: 0x0017A82C
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight);
		}
	}
}
