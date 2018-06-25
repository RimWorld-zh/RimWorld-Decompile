using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD6 RID: 2774
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06003D8A RID: 15754 RVA: 0x002066BF File Offset: 0x00204ABF
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x002066E4 File Offset: 0x00204AE4
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
