using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A5 RID: 165
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		// Token: 0x04000270 RID: 624
		private const float LayRadius = 5f;

		// Token: 0x06000412 RID: 1042 RVA: 0x00030BF8 File Offset: 0x0002EFF8
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			Job result;
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				result = null;
			}
			else
			{
				IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some);
				result = new Job(JobDefOf.LayEgg, c);
			}
			return result;
		}
	}
}
