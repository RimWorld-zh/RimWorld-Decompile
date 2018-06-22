using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A69 RID: 2665
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06003B45 RID: 15173 RVA: 0x001F6BEC File Offset: 0x001F4FEC
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
