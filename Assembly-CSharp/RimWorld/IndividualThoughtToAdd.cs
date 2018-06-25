using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047D RID: 1149
	public struct IndividualThoughtToAdd
	{
		// Token: 0x04000C04 RID: 3076
		public Thought_Memory thought;

		// Token: 0x04000C05 RID: 3077
		public Pawn addTo;

		// Token: 0x04000C06 RID: 3078
		private Pawn otherPawn;

		// Token: 0x06001424 RID: 5156 RVA: 0x000AF4D8 File Offset: 0x000AD8D8
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
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x000AF540 File Offset: 0x000AD940
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

		// Token: 0x06001426 RID: 5158 RVA: 0x000AF590 File Offset: 0x000AD990
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}
	}
}
