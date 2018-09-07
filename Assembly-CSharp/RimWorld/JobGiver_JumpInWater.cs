using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		private const float ActivateChance = 1f;

		private readonly IntRange MaxDistance = new IntRange(10, 16);

		public JobGiver_JumpInWater()
		{
		}

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

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 pos)
			{
				return pos.GetTerrain(this.pawn.Map).extinguishesFire;
			}
		}
	}
}
