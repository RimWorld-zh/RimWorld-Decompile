using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047F RID: 1151
	public struct IndividualThoughtToAdd
	{
		// Token: 0x06001429 RID: 5161 RVA: 0x000AF370 File Offset: 0x000AD770
		public IndividualThoughtToAdd(ThoughtDef thoughtDef, Pawn addTo, Pawn otherPawn = null, float moodPowerFactor = 1f, float opinionOffsetFactor = 1f)
		{
			this.addTo = addTo;
			this.otherPawn = otherPawn;
			this.thought = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			this.thought.moodPowerFactor = moodPowerFactor;
			this.thought.otherPawn = otherPawn;
			Thought_MemorySocial thought_MemorySocial = this.thought as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= opinionOffsetFactor;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x000AF3D8 File Offset: 0x000AD7D8
		public string LabelCap
		{
			get
			{
				string text = this.thought.LabelCap;
				float num = this.thought.MoodOffset();
				if (num != 0f)
				{
					text = text + " " + Mathf.RoundToInt(num).ToStringWithSign();
				}
				return text;
			}
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x000AF428 File Offset: 0x000AD828
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		// Token: 0x04000C07 RID: 3079
		public Thought_Memory thought;

		// Token: 0x04000C08 RID: 3080
		public Pawn addTo;

		// Token: 0x04000C09 RID: 3081
		private Pawn otherPawn;
	}
}
