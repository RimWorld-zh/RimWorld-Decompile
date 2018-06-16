using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FA RID: 2042
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x06002D3B RID: 11579 RVA: 0x0017C023 File Offset: 0x0017A423
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002D3C RID: 11580 RVA: 0x0017C04C File Offset: 0x0017A44C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x0017C070 File Offset: 0x0017A470
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight);
		}

		// Token: 0x040017CC RID: 6092
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040017CD RID: 6093
		private float scrollViewHeight;
	}
}
