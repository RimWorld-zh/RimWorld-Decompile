using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD3 RID: 2771
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06003D86 RID: 15750 RVA: 0x002062B3 File Offset: 0x002046B3
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x002062D8 File Offset: 0x002046D8
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
