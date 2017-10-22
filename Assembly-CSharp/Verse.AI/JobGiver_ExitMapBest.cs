using RimWorld;

namespace Verse.AI
{
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (TraverseMode)(canDig ? 3 : 0);
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
