using System;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		public PlaceWorker_HeadOnShipBeam()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			IntVec3 c = loc + rot.FacingCell * -1;
			AcceptanceReport result;
			if (!c.InBounds(map))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				if (edifice == null || edifice.def != ThingDefOf.Ship_Beam)
				{
					result = "MustPlaceHeadOnShipBeam".Translate();
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}
}
