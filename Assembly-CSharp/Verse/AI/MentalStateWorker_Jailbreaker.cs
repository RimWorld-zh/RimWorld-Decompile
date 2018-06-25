using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		public MentalStateWorker_Jailbreaker()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
