using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DB RID: 1243
	public static class SpouseRelationUtility
	{
		// Token: 0x06001616 RID: 5654 RVA: 0x000C3BC0 File Offset: 0x000C1FC0
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

		// Token: 0x06001617 RID: 5655 RVA: 0x000C3C00 File Offset: 0x000C2000
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

		// Token: 0x04000CCA RID: 3274
		public const float ChanceForSpousesToHaveTheSameName = 0.8f;
	}
}
