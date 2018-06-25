using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000537 RID: 1335
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x04000EA7 RID: 3751
		public float opinionOffset;

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x000D895C File Offset: 0x000D6D5C
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x000D8998 File Offset: 0x000D6D98
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x000D89CC File Offset: 0x000D6DCC
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x000D89F8 File Offset: 0x000D6DF8
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x000D8A28 File Offset: 0x000D6E28
		public virtual float OpinionOffset()
		{
			float result;
			if (this.ShouldDiscard)
			{
				result = 0f;
			}
			else
			{
				result = this.opinionOffset * this.AgeFactor;
			}
			return result;
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x000D8A60 File Offset: 0x000D6E60
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x000D8A7B File Offset: 0x000D6E7B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x000D8A9A File Offset: 0x000D6E9A
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x000D8AB4 File Offset: 0x000D6EB4
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000D8AD0 File Offset: 0x000D6ED0
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}
	}
}
