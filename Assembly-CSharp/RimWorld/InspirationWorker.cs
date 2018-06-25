using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A8 RID: 680
	public class InspirationWorker
	{
		// Token: 0x0400064C RID: 1612
		public InspirationDef def;

		// Token: 0x06000B60 RID: 2912 RVA: 0x00066B18 File Offset: 0x00064F18
		public virtual float CommonalityFor(Pawn pawn)
		{
			return this.def.baseCommonality;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00066B38 File Offset: 0x00064F38
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
				if (!this.def.requiredAnySkill.NullOrEmpty<SkillRequirement>())
				{
					bool flag = false;
					for (int k = 0; k < this.def.requiredAnySkill.Count; k++)
					{
						if (this.def.requiredAnySkill[k].PawnSatisfies(pawn))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (this.def.requiredNonDisabledWorkTypes != null)
				{
					for (int l = 0; l < this.def.requiredNonDisabledWorkTypes.Count; l++)
					{
						if (pawn.story == null || pawn.story.WorkTypeIsDisabled(this.def.requiredNonDisabledWorkTypes[l]))
						{
							return false;
						}
					}
				}
				if (!this.def.requiredAnyNonDisabledWorkType.NullOrEmpty<WorkTypeDef>())
				{
					bool flag2 = false;
					for (int m = 0; m < this.def.requiredAnyNonDisabledWorkType.Count; m++)
					{
						if (pawn.story != null && !pawn.story.WorkTypeIsDisabled(this.def.requiredAnyNonDisabledWorkType[m]))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						return false;
					}
				}
				if (this.def.requiredCapacities != null)
				{
					for (int n = 0; n < this.def.requiredCapacities.Count; n++)
					{
						if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[n]))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}
	}
}
