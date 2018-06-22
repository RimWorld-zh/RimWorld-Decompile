using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000350 RID: 848
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x0007C078 File Offset: 0x0007A478
		public static Pawn GenerateRefugee(int tile)
		{
			PawnKindDef spaceRefugee = PawnKindDefOf.SpaceRefugee;
			Faction randomFactionForRefugee = DownedRefugeeQuestUtility.GetRandomFactionForRefugee();
			PawnGenerationRequest request = new PawnGenerationRequest(spaceRefugee, randomFactionForRefugee, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, new float?(0.2f), null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			HealthUtility.DamageUntilDowned(pawn);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn);
			return pawn;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0007C108 File Offset: 0x0007A508
		public static Faction GetRandomFactionForRefugee()
		{
			if (Rand.Chance(0.6f))
			{
				Faction result;
				if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, true, false, TechLevel.Undefined))
				{
					return result;
				}
			}
			return null;
		}

		// Token: 0x04000902 RID: 2306
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04000903 RID: 2307
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
