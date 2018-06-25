using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000858 RID: 2136
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x04001A3D RID: 6717
		private Vector2 thoughtScrollPosition;

		// Token: 0x0600305E RID: 12382 RVA: 0x001A55F0 File Offset: 0x001A39F0
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x001A5610 File Offset: 0x001A3A10
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x001A5650 File Offset: 0x001A3A50
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x001A566D File Offset: 0x001A3A6D
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x001A56A6 File Offset: 0x001A3AA6
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}
	}
}
