using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D9 RID: 1241
	public static class SpouseRelationUtility
	{
		// Token: 0x04000CC7 RID: 3271
		public const float ChanceForSpousesToHaveTheSameName = 0.8f;

		// Token: 0x06001611 RID: 5649 RVA: 0x000C3D00 File Offset: 0x000C2100
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

		// Token: 0x06001612 RID: 5650 RVA: 0x000C3D40 File Offset: 0x000C2140
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
