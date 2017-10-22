using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			bool result;
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				result = false;
			}
			else if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				result = WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			else
			{
				result = true;
			}
			return result;
		}

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c = default(IntVec3);
			Building t2 = default(Building);
			return WatchBuildingUtility.TryFindBestWatchCell(t, pawn, base.def.desireSit, out c, out t2) ? new Job(base.def.jobDef, t, c, (Thing)t2) : null;
		}
	}
}
