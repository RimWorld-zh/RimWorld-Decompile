using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053A RID: 1338
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x04000EAB RID: 3755
		public Pawn otherPawn;

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x00057E38 File Offset: 0x00056238
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00057E6C File Offset: 0x0005626C
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00057E88 File Offset: 0x00056288
		public virtual float OpinionOffset()
		{
			return base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00057EA8 File Offset: 0x000562A8
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00057EF0 File Offset: 0x000562F0
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}
	}
}
