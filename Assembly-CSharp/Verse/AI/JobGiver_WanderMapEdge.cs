using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD5 RID: 2773
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06003D8A RID: 15754 RVA: 0x002063DF File Offset: 0x002047DF
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00206404 File Offset: 0x00204804
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
