using RimWorld;

namespace Verse.AI
{
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (TraverseMode)(canDig ? 3 : 0);
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
