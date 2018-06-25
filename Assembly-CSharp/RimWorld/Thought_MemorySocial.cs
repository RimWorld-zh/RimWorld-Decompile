using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000537 RID: 1335
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x04000EA3 RID: 3747
		public float opinionOffset;

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x000D86F4 File Offset: 0x000D6AF4
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x000D8730 File Offset: 0x000D6B30
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x000D8764 File Offset: 0x000D6B64
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x000D8790 File Offset: 0x000D6B90
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x000D87C0 File Offset: 0x000D6BC0
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

		// Token: 0x060018CF RID: 6351 RVA: 0x000D87F8 File Offset: 0x000D6BF8
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x000D8813 File Offset: 0x000D6C13
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x000D8832 File Offset: 0x000D6C32
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000D884C File Offset: 0x000D6C4C
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x000D8868 File Offset: 0x000D6C68
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}
	}
}
