using System;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		public PlaceWorker_DeepDrill()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			AcceptanceReport result;
			if (DeepDrillUtility.GetNextResource(loc, map) == null)
			{
				result = "MustPlaceOnDrillable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
