using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC3 RID: 2755
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06003D49 RID: 15689 RVA: 0x0020540C File Offset: 0x0020380C
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
