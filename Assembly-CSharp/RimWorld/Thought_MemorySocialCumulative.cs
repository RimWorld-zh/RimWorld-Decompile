using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000538 RID: 1336
	public class Thought_MemorySocialCumulative : Thought_MemorySocial
	{
		// Token: 0x04000EA8 RID: 3752
		private const float OpinionOffsetChangePerDay = 1f;

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x000D8B20 File Offset: 0x000D6F20
		public override bool ShouldDiscard
		{
			get
			{
				return this.opinionOffset == 0f;
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x000D8B44 File Offset: 0x000D6F44
		public override float OpinionOffset()
		{
			float result;
			if (this.ShouldDiscard)
			{
				result = 0f;
			}
			else
			{
				result = Mathf.Min(this.opinionOffset, this.def.maxCumulatedOpinionOffset);
			}
			return result;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x000D8B88 File Offset: 0x000D6F88
		public override void ThoughtInterval()
		{
			base.ThoughtInterval();
			if (this.age >= 60000)
			{
				if (this.opinionOffset < 0f)
				{
					this.opinionOffset += 1f;
					if (this.opinionOffset > 0f)
					{
						this.opinionOffset = 0f;
					}
				}
				else if (this.opinionOffset > 0f)
				{
					this.opinionOffset -= 1f;
					if (this.opinionOffset < 0f)
					{
						this.opinionOffset = 0f;
					}
				}
				this.age = 0;
			}
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x000D8C38 File Offset: 0x000D7038
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			List<Thought_Memory> memories = this.pawn.needs.mood.thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				if (memories[i].def == this.def)
				{
					Thought_MemorySocialCumulative thought_MemorySocialCumulative = (Thought_MemorySocialCumulative)memories[i];
					if (thought_MemorySocialCumulative.OtherPawn() == this.otherPawn)
					{
						thought_MemorySocialCumulative.opinionOffset += this.opinionOffset;
						return true;
					}
				}
			}
			return false;
		}
	}
}
