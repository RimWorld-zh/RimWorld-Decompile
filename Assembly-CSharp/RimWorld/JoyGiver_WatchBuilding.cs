using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				return false;
			}
			if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				return WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			return true;
		}

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c = default(IntVec3);
			Building t2 = default(Building);
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, base.def.desireSit, out c, out t2))
			{
				return null;
			}
			return new Job(base.def.jobDef, t, c, (Thing)t2);
		}
	}
}
