using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x060045D4 RID: 17876 RVA: 0x0024DD60 File Offset: 0x0024C160
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

		// Token: 0x060045D5 RID: 17877 RVA: 0x0024DDBC File Offset: 0x0024C1BC
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
