using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E4 RID: 228
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x00036F38 File Offset: 0x00035338
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 1f)
			{
				IntVec3 position = pawn.Position;
				Predicate<IntVec3> validator = (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire;
				Map map = pawn.Map;
				IntVec3 c;
				ref IntVec3 result = ref c;
				int randomInRange = this.MaxDistance.RandomInRange;
				if (RCellFinder.TryFindRandomCellNearWith(position, validator, map, out result, 5, randomInRange))
				{
					return new Job(JobDefOf.Goto, c);
				}
			}
			return null;
		}

		// Token: 0x040002B9 RID: 697
		private const float ActivateChance = 1f;

		// Token: 0x040002BA RID: 698
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
