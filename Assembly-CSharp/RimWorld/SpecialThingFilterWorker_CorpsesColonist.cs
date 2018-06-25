using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000996 RID: 2454
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x06003724 RID: 14116 RVA: 0x001D88CC File Offset: 0x001D6CCC
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
