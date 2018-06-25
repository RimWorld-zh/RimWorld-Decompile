using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC1 RID: 2753
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06003D4A RID: 15690 RVA: 0x00205930 File Offset: 0x00203D30
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
