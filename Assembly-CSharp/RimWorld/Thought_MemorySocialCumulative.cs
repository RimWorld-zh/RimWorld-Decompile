using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	public class Thought_MemorySocialCumulative : Thought_MemorySocial
	{
		private const float OpinionOffsetChangePerDay = 1f;

		public override bool ShouldDiscard
		{
			get
			{
				return base.opinionOffset == 0.0;
			}
		}

		public override float OpinionOffset()
		{
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return Mathf.Min(base.opinionOffset, base.def.maxCumulatedOpinionOffset);
		}

		public override void ThoughtInterval()
		{
			base.ThoughtInterval();
			if (base.age >= 60000)
			{
				if (base.opinionOffset < 0.0)
				{
					base.opinionOffset += 1f;
					if (base.opinionOffset > 0.0)
					{
						base.opinionOffset = 0f;
					}
				}
				else if (base.opinionOffset > 0.0)
				{
					base.opinionOffset -= 1f;
					if (base.opinionOffset < 0.0)
					{
						base.opinionOffset = 0f;
					}
				}
				base.age = 0;
			}
		}

		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			List<Thought_Memory> memories = base.pawn.needs.mood.thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				if (memories[i].def == base.def)
				{
					Thought_MemorySocialCumulative thought_MemorySocialCumulative = (Thought_MemorySocialCumulative)memories[i];
					if (thought_MemorySocialCumulative.OtherPawn() == base.otherPawn)
					{
						thought_MemorySocialCumulative.opinionOffset += base.opinionOffset;
						return true;
					}
				}
			}
			return false;
		}
	}
}
