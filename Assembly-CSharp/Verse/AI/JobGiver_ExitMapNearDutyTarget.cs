using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC1 RID: 2753
	public class JobGiver_ExitMapNearDutyTarget : JobGiver_ExitMap
	{
		// Token: 0x06003D4A RID: 15690 RVA: 0x0020586C File Offset: 0x00203C6C
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			IntVec3 near = pawn.DutyLocation();
			float num = pawn.mindState.duty.radius;
			if (num <= 0f)
			{
				num = 12f;
			}
			return RCellFinder.TryFindExitSpotNear(pawn, near, num, out spot, mode);
		}
	}
}
