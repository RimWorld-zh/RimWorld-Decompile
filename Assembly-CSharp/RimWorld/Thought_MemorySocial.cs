using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000535 RID: 1333
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x04000EA3 RID: 3747
		public float opinionOffset;

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x000D85A4 File Offset: 0x000D69A4
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x000D85E0 File Offset: 0x000D69E0
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x000D8614 File Offset: 0x000D6A14
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x000D8640 File Offset: 0x000D6A40
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x000D8670 File Offset: 0x000D6A70
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

		// Token: 0x060018CB RID: 6347 RVA: 0x000D86A8 File Offset: 0x000D6AA8
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x000D86C3 File Offset: 0x000D6AC3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x000D86E2 File Offset: 0x000D6AE2
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x000D86FC File Offset: 0x000D6AFC
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x000D8718 File Offset: 0x000D6B18
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}
	}
}
