using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F8 RID: 2040
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x040017CE RID: 6094
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040017CF RID: 6095
		private float scrollViewHeight;

		// Token: 0x06002D39 RID: 11577 RVA: 0x0017C643 File Offset: 0x0017AA43
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x0017C66C File Offset: 0x0017AA6C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x0017C690 File Offset: 0x0017AA90
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight);
		}
	}
}
