using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D7 RID: 1239
	public static class SpouseRelationUtility
	{
		// Token: 0x04000CC7 RID: 3271
		public const float ChanceForSpousesToHaveTheSameName = 0.8f;

		// Token: 0x0600160D RID: 5645 RVA: 0x000C3BB0 File Offset: 0x000C1FB0
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

		// Token: 0x0600160E RID: 5646 RVA: 0x000C3BF0 File Offset: 0x000C1FF0
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
