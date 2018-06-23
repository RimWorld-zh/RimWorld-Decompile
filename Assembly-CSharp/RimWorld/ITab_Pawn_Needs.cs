using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000856 RID: 2134
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x04001A39 RID: 6713
		private Vector2 thoughtScrollPosition;

		// Token: 0x0600305B RID: 12379 RVA: 0x001A5238 File Offset: 0x001A3638
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600305C RID: 12380 RVA: 0x001A5258 File Offset: 0x001A3658
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x001A5298 File Offset: 0x001A3698
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x001A52B5 File Offset: 0x001A36B5
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x001A52EE File Offset: 0x001A36EE
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}
	}
}
