using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000858 RID: 2136
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x04001A39 RID: 6713
		private Vector2 thoughtScrollPosition;

		// Token: 0x0600305F RID: 12383 RVA: 0x001A5388 File Offset: 0x001A3788
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06003060 RID: 12384 RVA: 0x001A53A8 File Offset: 0x001A37A8
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x001A53E8 File Offset: 0x001A37E8
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x001A5405 File Offset: 0x001A3805
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x001A543E File Offset: 0x001A383E
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}
	}
}
