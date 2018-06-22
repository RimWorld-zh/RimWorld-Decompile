using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000352 RID: 850
	public class IncidentWorker_QuestItemStashAICore : IncidentWorker_QuestItemStash
	{
		// Token: 0x06000EB0 RID: 3760 RVA: 0x0007C584 File Offset: 0x0007A984
		protected override List<Thing> GenerateItems(Faction siteFaction)
		{
			return new List<Thing>
			{
				ThingMaker.MakeThing(ThingDefOf.AIPersonaCore, null)
			};
		}
	}
}
