using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000354 RID: 852
	public class IncidentWorker_QuestItemStashAICore : IncidentWorker_QuestItemStash
	{
		// Token: 0x06000EB3 RID: 3763 RVA: 0x0007C6DC File Offset: 0x0007AADC
		protected override List<Thing> GenerateItems(Faction siteFaction)
		{
			return new List<Thing>
			{
				ThingMaker.MakeThing(ThingDefOf.AIPersonaCore, null)
			};
		}
	}
}
