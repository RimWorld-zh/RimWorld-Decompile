using RimWorld;

namespace Verse.AI
{
	public class JobGiver_ExitMapNearDutyTarget : JobGiver_ExitMap
	{
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (TraverseMode)(canDig ? 3 : 0);
			IntVec3 near = pawn.DutyLocation();
			float num = pawn.mindState.duty.radius;
			if (num <= 0.0)
			{
				num = 12f;
			}
			return RCellFinder.TryFindExitSpotNear(pawn, near, num, out spot, mode);
		}
	}
}
