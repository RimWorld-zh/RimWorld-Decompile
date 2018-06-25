using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6C RID: 2668
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06003B4A RID: 15178 RVA: 0x001F7044 File Offset: 0x001F5444
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
