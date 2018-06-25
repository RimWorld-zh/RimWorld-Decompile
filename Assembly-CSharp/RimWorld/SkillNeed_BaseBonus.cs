using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000275 RID: 629
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x04000555 RID: 1365
		private float baseValue = 0.5f;

		// Token: 0x04000556 RID: 1366
		private float bonusPerLevel = 0.05f;

		// Token: 0x06000ACE RID: 2766 RVA: 0x00061F64 File Offset: 0x00060364
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

		// Token: 0x06000ACF RID: 2767 RVA: 0x00061FB0 File Offset: 0x000603B0
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00061FD8 File Offset: 0x000603D8
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
	}
}
