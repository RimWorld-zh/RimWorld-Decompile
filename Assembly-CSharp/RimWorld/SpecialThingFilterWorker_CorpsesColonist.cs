using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000998 RID: 2456
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x06003727 RID: 14119 RVA: 0x001D82BC File Offset: 0x001D66BC
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
