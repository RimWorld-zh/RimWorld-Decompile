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
			bool result;
			if (!this.def.allowedOnAnimals && pawn.RaceProps.Animal)
			{
				result = false;
			}
			else if (!this.def.allowedOnNonColonists && !pawn.IsColonist)
			{
				result = false;
			}
			else
			{
				if (this.def.requiredNonDisabledStats != null)
				{
					for (int i = 0; i < this.def.requiredNonDisabledStats.Count; i++)
					{
						if (this.def.requiredNonDisabledStats[i].Worker.IsDisabledFor(pawn))
							goto IL_0084;
					}
				}
				if (this.def.requiredSkills != null)
				{
					for (int j = 0; j < this.def.requiredSkills.Count; j++)
					{
						if (!this.def.requiredSkills[j].PawnSatisfies(pawn))
							goto IL_00dc;
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
						goto IL_0144;
					}
				}
				if (this.def.requiredCapacities != null)
				{
					for (int k = 0; k < this.def.requiredCapacities.Count; k++)
					{
						if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[k]))
							goto IL_01a8;
					}
				}
				result = true;
			}
			goto IL_01d5;
			IL_00dc:
			result = false;
			goto IL_01d5;
			IL_01a8:
			result = false;
			goto IL_01d5;
			IL_01d5:
			return result;
			IL_0084:
			result = false;
			goto IL_01d5;
			IL_0144:
			result = false;
			goto IL_01d5;
		}
	}
}
