using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6B RID: 2667
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06003B49 RID: 15177 RVA: 0x001F6D18 File Offset: 0x001F5118
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
