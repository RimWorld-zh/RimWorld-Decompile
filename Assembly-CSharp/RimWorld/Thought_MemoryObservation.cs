using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000538 RID: 1336
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x17000377 RID: 887
		// (set) Token: 0x060018CB RID: 6347 RVA: 0x000D848A File Offset: 0x000D688A
		public Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x000D8499 File Offset: 0x000D6899
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x000D84B4 File Offset: 0x000D68B4
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			Thought_MemoryObservation thought_MemoryObservation = null;
			List<Thought_Memory> memories = thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemoryObservation thought_MemoryObservation2 = memories[i] as Thought_MemoryObservation;
				if (thought_MemoryObservation2 != null && thought_MemoryObservation2.def == this.def && thought_MemoryObservation2.targetThingID == this.targetThingID && (thought_MemoryObservation == null || thought_MemoryObservation2.age > thought_MemoryObservation.age))
				{
					thought_MemoryObservation = thought_MemoryObservation2;
				}
			}
			bool result;
			if (thought_MemoryObservation != null)
			{
				showBubble = (thought_MemoryObservation.age > thought_MemoryObservation.def.DurationTicks / 2);
				thought_MemoryObservation.Renew();
				result = true;
			}
			else
			{
				showBubble = true;
				result = false;
			}
			return result;
		}

		// Token: 0x04000EA5 RID: 3749
		private int targetThingID;
	}
}
