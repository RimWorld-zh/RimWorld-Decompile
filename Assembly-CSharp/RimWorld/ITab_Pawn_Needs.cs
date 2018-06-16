using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200085A RID: 2138
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x06003060 RID: 12384 RVA: 0x001A4F90 File Offset: 0x001A3390
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x001A4FB0 File Offset: 0x001A33B0
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x001A4FF0 File Offset: 0x001A33F0
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x001A500D File Offset: 0x001A340D
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x001A5046 File Offset: 0x001A3446
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}

		// Token: 0x04001A3B RID: 6715
		private Vector2 thoughtScrollPosition;
	}
}
