using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class SkillNeed_BaseBonus : SkillNeed
	{
		private float baseFactor = 0.5f;

		private float bonusFactor = 0.05f;

		public override float FactorFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(base.skill).Level;
			return this.FactorAt(level);
		}

		private float FactorAt(int level)
		{
			return this.baseFactor + this.bonusFactor * (float)level;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			for (int i = 1; i <= 20; i++)
			{
				float factor = this.FactorAt(i);
				if (factor <= 0.0)
				{
					yield return "SkillNeed yields factor < 0 at skill level " + i;
				}
			}
		}
	}
}
