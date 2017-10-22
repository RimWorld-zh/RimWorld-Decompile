using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class SkillNeed_BaseBonus : SkillNeed
	{
		private float baseValue = 0.5f;

		private float bonusPerLevel = 0.05f;

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
				result = this.ValueAtLevel(level);
			}
			return result;
		}

		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string error = enumerator.Current;
					yield return error;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			int i = 1;
			while (true)
			{
				if (i <= 20)
				{
					float factor = this.ValueAtLevel(i);
					if (!(factor <= 0.0))
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return "SkillNeed yields factor < 0 at skill level " + i;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0140:
			/*Error near IL_0141: Unexpected return in MoveNext()*/;
		}
	}
}
