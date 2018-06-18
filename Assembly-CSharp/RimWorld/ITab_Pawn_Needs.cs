using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200085A RID: 2138
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x06003062 RID: 12386 RVA: 0x001A5058 File Offset: 0x001A3458
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x001A5078 File Offset: 0x001A3478
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x001A50B8 File Offset: 0x001A34B8
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x001A50D5 File Offset: 0x001A34D5
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x001A510E File Offset: 0x001A350E
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}

		// Token: 0x04001A3B RID: 6715
		private Vector2 thoughtScrollPosition;
	}
}
