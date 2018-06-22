using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000536 RID: 1334
	public class Thought_MemorySocialCumulative : Thought_MemorySocial
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x000D8768 File Offset: 0x000D6B68
		public override bool ShouldDiscard
		{
			get
			{
				return this.opinionOffset == 0f;
			}
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000D878C File Offset: 0x000D6B8C
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

		// Token: 0x060018D3 RID: 6355 RVA: 0x000D87D0 File Offset: 0x000D6BD0
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

		// Token: 0x060018D4 RID: 6356 RVA: 0x000D8880 File Offset: 0x000D6C80
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

		// Token: 0x04000EA4 RID: 3748
		private const float OpinionOffsetChangePerDay = 1f;
	}
}
