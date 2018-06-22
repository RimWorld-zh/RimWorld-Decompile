using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000994 RID: 2452
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x06003720 RID: 14112 RVA: 0x001D84B8 File Offset: 0x001D68B8
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
