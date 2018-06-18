using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC4 RID: 2756
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06003D4D RID: 15693 RVA: 0x00205514 File Offset: 0x00203914
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
