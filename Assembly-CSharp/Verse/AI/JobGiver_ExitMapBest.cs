using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC2 RID: 2754
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06003D4C RID: 15692 RVA: 0x00205964 File Offset: 0x00203D64
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
