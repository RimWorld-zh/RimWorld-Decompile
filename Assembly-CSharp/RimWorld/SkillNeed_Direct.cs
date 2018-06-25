using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000276 RID: 630
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x04000557 RID: 1367
		public List<float> valuesPerLevel = new List<float>();

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00062294 File Offset: 0x00060694
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
