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
			return (float)((!this.ShouldDiscard) ? Mathf.Min(base.opinionOffset, base.def.maxCumulatedOpinionOffset) : 0.0);
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
			int num = 0;
			bool result;
			while (true)
			{
				if (num < memories.Count)
				{
					if (memories[num].def == base.def)
					{
						Thought_MemorySocialCumulative thought_MemorySocialCumulative = (Thought_MemorySocialCumulative)memories[num];
						if (thought_MemorySocialCumulative.OtherPawn() == base.otherPawn)
						{
							thought_MemorySocialCumulative.opinionOffset += base.opinionOffset;
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
