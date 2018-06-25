using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC3 RID: 2755
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06003D4C RID: 15692 RVA: 0x00205C44 File Offset: 0x00204044
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
