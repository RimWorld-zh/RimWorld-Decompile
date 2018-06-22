using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F6 RID: 2038
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x06002D36 RID: 11574 RVA: 0x0017C28F File Offset: 0x0017A68F
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x0017C2B8 File Offset: 0x0017A6B8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x0017C2DC File Offset: 0x0017A6DC
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight);
		}

		// Token: 0x040017CA RID: 6090
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040017CB RID: 6091
		private float scrollViewHeight;
	}
}
