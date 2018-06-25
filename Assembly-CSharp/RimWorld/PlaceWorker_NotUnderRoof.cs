using System;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		public PlaceWorker_NotUnderRoof()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			AcceptanceReport result;
			if (map.roofGrid.Roofed(loc))
			{
				result = new AcceptanceReport("MustPlaceUnroofed".Translate());
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
