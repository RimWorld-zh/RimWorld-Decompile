using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000273 RID: 627
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x06000ACD RID: 2765 RVA: 0x00061DBC File Offset: 0x000601BC
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
				result = this.ValueAtLevel(level);
			}
			return result;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00061E08 File Offset: 0x00060208
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00061E30 File Offset: 0x00060230
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return error;
			}
			for (int i = 1; i <= 20; i++)
			{
				float factor = this.ValueAtLevel(i);
				if (factor <= 0f)
				{
					yield return "SkillNeed yields factor < 0 at skill level " + i;
				}
			}
			yield break;
		}

		// Token: 0x04000555 RID: 1365
		private float baseValue = 0.5f;

		// Token: 0x04000556 RID: 1366
		private float bonusPerLevel = 0.05f;
	}
}
