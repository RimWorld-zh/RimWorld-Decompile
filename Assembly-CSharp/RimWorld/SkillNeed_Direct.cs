using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000274 RID: 628
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x06000AD2 RID: 2770 RVA: 0x000620EC File Offset: 0x000604EC
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

		// Token: 0x04000557 RID: 1367
		public List<float> valuesPerLevel = new List<float>();
	}
}
