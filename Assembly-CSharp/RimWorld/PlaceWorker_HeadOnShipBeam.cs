using Verse;

namespace RimWorld
{
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
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
				result = ((edifice != null && edifice.def == ThingDefOf.Ship_Beam) ? true : "MustPlaceHeadOnShipBeam".Translate());
			}
			return result;
		}
	}
}
