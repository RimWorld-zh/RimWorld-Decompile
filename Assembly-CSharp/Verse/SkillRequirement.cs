using RimWorld;

namespace Verse
{
	public class SkillRequirement
	{
		public SkillDef skill;

		public int minLevel;

		public bool PawnSatisfies(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return false;
			}
			return pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}
	}
}
