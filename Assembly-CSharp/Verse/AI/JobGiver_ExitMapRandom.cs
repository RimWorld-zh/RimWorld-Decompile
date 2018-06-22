using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ABF RID: 2751
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x00205804 File Offset: 0x00203C04
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
