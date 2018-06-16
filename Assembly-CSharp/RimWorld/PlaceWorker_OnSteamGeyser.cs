using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6B RID: 3179
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x060045CA RID: 17866 RVA: 0x0024C5FC File Offset: 0x0024A9FC
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

		// Token: 0x060045CB RID: 17867 RVA: 0x0024C658 File Offset: 0x0024AA58
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
