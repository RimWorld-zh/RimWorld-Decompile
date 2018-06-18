using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053C RID: 1340
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x00057DF8 File Offset: 0x000561F8
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00057E2C File Offset: 0x0005622C
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00057E48 File Offset: 0x00056248
		public virtual float OpinionOffset()
		{
			return base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00057E68 File Offset: 0x00056268
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00057EB0 File Offset: 0x000562B0
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x04000EAA RID: 3754
		public Pawn otherPawn;
	}
}
