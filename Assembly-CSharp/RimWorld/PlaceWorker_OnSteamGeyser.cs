using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x060045D4 RID: 17876 RVA: 0x0024DA80 File Offset: 0x0024BE80
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			Thing thing = map.thingGrid.ThingAt(loc, ThingDefOf.SteamGeyser);
			AcceptanceReport result;
			if (thing == null || thing.Position != loc)
			{
				result = "MustPlaceOnSteamGeyser".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x0024DADC File Offset: 0x0024BEDC
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
