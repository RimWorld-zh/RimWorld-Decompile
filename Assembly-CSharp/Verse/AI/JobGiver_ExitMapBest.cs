using System;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		public JobGiver_ExitMapBest()
		{
		}

		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
