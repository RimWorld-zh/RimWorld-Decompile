using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200053A RID: 1338
	public class Thought_MemorySocialCumulative : Thought_MemorySocial
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x000D875C File Offset: 0x000D6B5C
		public override bool ShouldDiscard
		{
			get
			{
				return this.opinionOffset == 0f;
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x000D8780 File Offset: 0x000D6B80
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

		// Token: 0x060018DC RID: 6364 RVA: 0x000D87C4 File Offset: 0x000D6BC4
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

		// Token: 0x060018DD RID: 6365 RVA: 0x000D8874 File Offset: 0x000D6C74
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

		// Token: 0x04000EA7 RID: 3751
		private const float OpinionOffsetChangePerDay = 1f;
	}
}
