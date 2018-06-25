using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000276 RID: 630
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x04000555 RID: 1365
		public List<float> valuesPerLevel = new List<float>();

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00062298 File Offset: 0x00060698
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
