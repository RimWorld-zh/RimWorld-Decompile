using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class SkillNeed_Direct : SkillNeed
	{
		public List<float> valuesPerLevel = new List<float>();

		public SkillNeed_Direct()
		{
		}

		public override float ValueFor(Pawn pawn)
		{
			float result;
			if (pawn.skills == null)
			{
				result = 1f;
			}
			else
			{
				int level = pawn.skills.GetSkill(this.skill).Level;
				if (this.valuesPerLevel.Count > level)
				{
					result = this.valuesPerLevel[level];
				}
				else if (this.valuesPerLevel.Count > 0)
				{
					result = this.valuesPerLevel[this.valuesPerLevel.Count - 1];
				}
				else
				{
					result = 1f;
				}
			}
			return result;
		}
	}
}
