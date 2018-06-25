using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000354 RID: 852
	public class IncidentWorker_QuestItemStashAICore : IncidentWorker_QuestItemStash
	{
		// Token: 0x06000EB4 RID: 3764 RVA: 0x0007C6D4 File Offset: 0x0007AAD4
		protected override List<Thing> GenerateItems(Faction siteFaction)
		{
			return new List<Thing>
			{
				ThingMaker.MakeThing(ThingDefOf.AIPersonaCore, null)
			};
		}
	}
}
