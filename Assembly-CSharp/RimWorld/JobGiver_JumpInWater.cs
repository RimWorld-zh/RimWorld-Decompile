using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		private const float ActivateChance = 1f;

		private readonly IntRange MaxDistance = new IntRange(10, 16);

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (Rand.Value < 1.0)
			{
				IntVec3 position = pawn.Position;
				Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire);
				Map map = pawn.Map;
				int randomInRange = this.MaxDistance.RandomInRange;
				IntVec3 c = default(IntVec3);
				if (RCellFinder.TryFindRandomCellNearWith(position, validator, map, out c, 5, randomInRange))
				{
					result = new Job(JobDefOf.Goto, c);
					goto IL_008d;
				}
			}
			result = null;
			goto IL_008d;
			IL_008d:
			return result;
		}
	}
}
