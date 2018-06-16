using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD7 RID: 2775
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06003D89 RID: 15753 RVA: 0x00205EBB File Offset: 0x002042BB
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x00205EE0 File Offset: 0x002042E0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			IntVec3 intVec;
			IntVec3 result;
			if (RCellFinder.TryFindBestExitSpot(pawn, out intVec, TraverseMode.ByPawn))
			{
				result = intVec;
			}
			else
			{
				result = pawn.Position;
			}
			return result;
		}
	}
}
