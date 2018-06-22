using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC0 RID: 2752
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06003D48 RID: 15688 RVA: 0x00205838 File Offset: 0x00203C38
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
