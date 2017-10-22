using Verse;

namespace RimWorld
{
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			return (!map.roofGrid.Roofed(loc)) ? true : new AcceptanceReport("MustPlaceUnroofed".Translate());
		}
	}
}
