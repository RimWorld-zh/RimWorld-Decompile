using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000534 RID: 1332
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x17000377 RID: 887
		// (set) Token: 0x060018C2 RID: 6338 RVA: 0x000D8496 File Offset: 0x000D6896
		public Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x000D84A5 File Offset: 0x000D68A5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x000D84C0 File Offset: 0x000D68C0
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

		// Token: 0x04000EA2 RID: 3746
		private int targetThingID;
	}
}
