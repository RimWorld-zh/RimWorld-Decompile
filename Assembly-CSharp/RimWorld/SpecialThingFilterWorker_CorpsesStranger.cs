using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000995 RID: 2453
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x06003722 RID: 14114 RVA: 0x001D851C File Offset: 0x001D691C
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
