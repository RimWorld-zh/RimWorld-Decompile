using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000358 RID: 856
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x04000915 RID: 2325
		private const float RelationWithColonistWeight = 75f;

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0007CF60 File Offset: 0x0007B360
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
