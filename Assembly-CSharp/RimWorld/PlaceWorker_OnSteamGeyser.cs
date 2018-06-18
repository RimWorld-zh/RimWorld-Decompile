using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x060045C8 RID: 17864 RVA: 0x0024C5D4 File Offset: 0x0024A9D4
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

		// Token: 0x060045C9 RID: 17865 RVA: 0x0024C630 File Offset: 0x0024AA30
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
