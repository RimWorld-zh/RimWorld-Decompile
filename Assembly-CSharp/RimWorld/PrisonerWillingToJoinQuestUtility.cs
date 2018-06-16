using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000356 RID: 854
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x06000EC6 RID: 3782 RVA: 0x0007CC1C File Offset: 0x0007B01C
		public static Pawn GeneratePrisoner(int tile, Faction hostFaction)
		{
			PawnKindDef slave = PawnKindDefOf.Slave;
			PawnGenerationRequest request = new PawnGenerationRequest(slave, hostFaction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 75f, false, true, true, false, false, true, true, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.guest.SetGuestStatus(hostFaction, true);
			return pawn;
		}

		// Token: 0x04000910 RID: 2320
		private const float RelationWithColonistWeight = 75f;
	}
}
