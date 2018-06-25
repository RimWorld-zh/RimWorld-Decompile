using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000352 RID: 850
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x04000902 RID: 2306
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04000903 RID: 2307
		private const float ChanceToRedressWorldPawn = 0.2f;

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0007C1C8 File Offset: 0x0007A5C8
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

		// Token: 0x06000EAA RID: 3754 RVA: 0x0007C258 File Offset: 0x0007A658
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
	}
}
