using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000997 RID: 2455
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x06003726 RID: 14118 RVA: 0x001D8930 File Offset: 0x001D6D30
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
