using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x06003727 RID: 14119 RVA: 0x001D824C File Offset: 0x001D664C
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
