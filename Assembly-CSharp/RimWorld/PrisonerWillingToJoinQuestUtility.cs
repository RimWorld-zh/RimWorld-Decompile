using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000356 RID: 854
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x04000912 RID: 2322
		private const float RelationWithColonistWeight = 75f;

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0007CE08 File Offset: 0x0007B208
		public static Pawn GeneratePrisoner(int tile, Faction hostFaction)
		{
			PawnKindDef slave = PawnKindDefOf.Slave;
			PawnGenerationRequest request = new PawnGenerationRequest(slave, hostFaction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 75f, false, true, true, false, false, true, true, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.guest.SetGuestStatus(hostFaction, true);
			return pawn;
		}
	}
}
