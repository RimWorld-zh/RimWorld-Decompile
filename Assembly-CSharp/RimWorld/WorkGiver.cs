using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver
	{
		public WorkGiverDef def;

		public virtual bool ShouldSkip(Pawn pawn)
		{
			return false;
		}

		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			int num = 0;
			PawnCapacityDef result;
			while (true)
			{
				if (num < this.def.requiredCapacities.Count)
				{
					if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[num]))
					{
						result = this.def.requiredCapacities[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
