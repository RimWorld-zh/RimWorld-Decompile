using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000539 RID: 1337
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060018CE RID: 6350 RVA: 0x000D8544 File Offset: 0x000D6944
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x000D8580 File Offset: 0x000D6980
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x000D85B4 File Offset: 0x000D69B4
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.def.DurationTicks;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x000D85E0 File Offset: 0x000D69E0
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000D8610 File Offset: 0x000D6A10
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

		// Token: 0x060018D3 RID: 6355 RVA: 0x000D8648 File Offset: 0x000D6A48
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x000D8663 File Offset: 0x000D6A63
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x000D8682 File Offset: 0x000D6A82
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x000D869C File Offset: 0x000D6A9C
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x000D86B8 File Offset: 0x000D6AB8
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		// Token: 0x04000EA6 RID: 3750
		public float opinionOffset;
	}
}
