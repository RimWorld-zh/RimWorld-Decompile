using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000538 RID: 1336
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x04000EA7 RID: 3751
		public Pawn otherPawn;

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x00057E3C File Offset: 0x0005623C
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00057E70 File Offset: 0x00056270
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x00057E8C File Offset: 0x0005628C
		public virtual float OpinionOffset()
		{
			return base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x00057EAC File Offset: 0x000562AC
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00057EF4 File Offset: 0x000562F4
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}
	}
}
