using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x06003729 RID: 14121 RVA: 0x001D8320 File Offset: 0x001D6720
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
