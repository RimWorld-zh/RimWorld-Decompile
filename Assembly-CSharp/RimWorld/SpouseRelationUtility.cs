using System;
using Verse;

namespace RimWorld
{
	public static class SpouseRelationUtility
	{
		public const float ChanceForSpousesToHaveTheSameName = 0.8f;

		public static Pawn GetSpouse(this Pawn pawn)
		{
			Pawn result;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = null;
			}
			else
			{
				result = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
			}
			return result;
		}

		public static Pawn GetSpouseOppositeGender(this Pawn pawn)
		{
			Pawn spouse = pawn.GetSpouse();
			Pawn result;
			if (spouse == null)
			{
				result = null;
			}
			else if ((pawn.gender == Gender.Male && spouse.gender == Gender.Female) || (pawn.gender == Gender.Female && spouse.gender == Gender.Male))
			{
				result = spouse;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
