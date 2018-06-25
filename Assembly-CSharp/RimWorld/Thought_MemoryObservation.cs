using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000536 RID: 1334
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x04000EA6 RID: 3750
		private int targetThingID;

		// Token: 0x17000377 RID: 887
		// (set) Token: 0x060018C5 RID: 6341 RVA: 0x000D884E File Offset: 0x000D6C4E
		public Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000D885D File Offset: 0x000D6C5D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x000D8878 File Offset: 0x000D6C78
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
	}
}
