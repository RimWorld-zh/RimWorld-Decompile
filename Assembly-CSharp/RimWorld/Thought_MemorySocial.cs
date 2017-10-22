using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		public float opinionOffset;

		public override bool ShouldDiscard
		{
			get
			{
				return base.otherPawn == null || this.opinionOffset == 0.0 || base.ShouldDiscard;
			}
		}

		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0.0;
			}
		}

		private float AgePct
		{
			get
			{
				return (float)base.age / (float)base.def.DurationTicks;
			}
		}

		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, base.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		public virtual float OpinionOffset()
		{
			return (float)((!this.ShouldDiscard) ? (this.opinionOffset * this.AgeFactor) : 0.0);
		}

		public Pawn OtherPawn()
		{
			return base.otherPawn;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && base.otherPawn == thought_MemorySocial.otherPawn;
		}
	}
}
