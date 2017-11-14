using Verse;

namespace RimWorld
{
	public class InspirationWorker
	{
		public InspirationDef def;

		public virtual float CommonalityFor(Pawn pawn)
		{
			return this.def.baseCommonality;
		}

		public virtual bool InspirationCanOccur(Pawn pawn)
		{
			if (!this.def.allowedOnAnimals && pawn.RaceProps.Animal)
			{
				return false;
			}
			if (!this.def.allowedOnNonColonists && !pawn.IsColonist)
			{
				return false;
			}
			if (this.def.requiredNonDisabledStats != null)
			{
				for (int i = 0; i < this.def.requiredNonDisabledStats.Count; i++)
				{
					if (this.def.requiredNonDisabledStats[i].Worker.IsDisabledFor(pawn))
					{
						return false;
					}
				}
			}
			if (this.def.requiredSkills != null)
			{
				for (int j = 0; j < this.def.requiredSkills.Count; j++)
				{
					if (!this.def.requiredSkills[j].PawnSatisfies(pawn))
					{
						return false;
					}
				}
			}
			if (this.def.requiredNonDisabledWorkTypes != null)
			{
				int num = 0;
				while (num < this.def.requiredNonDisabledWorkTypes.Count)
				{
					if (pawn.story != null && !pawn.story.WorkTypeIsDisabled(this.def.requiredNonDisabledWorkTypes[num]))
					{
						num++;
						continue;
					}
					return false;
				}
			}
			if (this.def.requiredCapacities != null)
			{
				for (int k = 0; k < this.def.requiredCapacities.Count; k++)
				{
					if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[k]))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
