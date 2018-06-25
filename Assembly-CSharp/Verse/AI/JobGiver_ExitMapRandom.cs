using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC2 RID: 2754
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06003D4A RID: 15690 RVA: 0x00205C10 File Offset: 0x00204010
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = (!canDig) ? TraverseMode.ByPawn : TraverseMode.PassAllDestroyableThings;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
