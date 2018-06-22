using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C67 RID: 3175
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x060045D1 RID: 17873 RVA: 0x0024D9A4 File Offset: 0x0024BDA4
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

		// Token: 0x060045D2 RID: 17874 RVA: 0x0024DA00 File Offset: 0x0024BE00
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}
	}
}
