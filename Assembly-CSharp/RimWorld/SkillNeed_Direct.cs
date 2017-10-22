using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class SkillNeed_Direct : SkillNeed
	{
		public List<float> valuesPerLevel = new List<float>();

		public override float ValueFor(Pawn pawn)
		{
			float result;
			if (pawn.skills == null)
			{
				result = 1f;
			}
			else
			{
				int level = pawn.skills.GetSkill(base.skill).Level;
				result = (float)((this.valuesPerLevel.Count <= level) ? ((this.valuesPerLevel.Count <= 0) ? 1.0 : this.valuesPerLevel[this.valuesPerLevel.Count - 1]) : this.valuesPerLevel[level]);
			}
			return result;
		}
	}
}
