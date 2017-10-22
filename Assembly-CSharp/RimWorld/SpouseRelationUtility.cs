using Verse;

namespace RimWorld
{
	public static class SpouseRelationUtility
	{
		public const float ChanceForSpousesToHaveTheSameName = 0.8f;

		public static Pawn GetSpouse(this Pawn pawn)
		{
			return pawn.RaceProps.IsFlesh ? pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null) : null;
		}

		public static Pawn GetSpouseOppositeGender(this Pawn pawn)
		{
			Pawn spouse = pawn.GetSpouse();
			Pawn result;
			if (spouse == null)
			{
				result = null;
			}
			else
			{
				if (pawn.gender == Gender.Male && spouse.gender == Gender.Female)
				{
					goto IL_0045;
				}
				if (pawn.gender == Gender.Female && spouse.gender == Gender.Male)
					goto IL_0045;
				result = null;
			}
			goto IL_0054;
			IL_0054:
			return result;
			IL_0045:
			result = spouse;
			goto IL_0054;
		}
	}
}
