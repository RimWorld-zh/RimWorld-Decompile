using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD7 RID: 2775
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06003D8B RID: 15755 RVA: 0x00205F8F File Offset: 0x0020438F
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x00205FB4 File Offset: 0x002043B4
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
