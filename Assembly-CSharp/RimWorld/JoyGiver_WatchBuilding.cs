using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		public JoyGiver_WatchBuilding()
		{
		}

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
			IntVec3 c;
			Building t2;
			Job result;
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, this.def.desireSit, out c, out t2))
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, t, c, t2);
			}
			return result;
		}
	}
}
