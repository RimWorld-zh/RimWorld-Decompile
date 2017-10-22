using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		private static List<Thing> tmpThings = new List<Thing>();

		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Predicate<Thing>)((Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x)), 0, 40);
				bool flag = MentalStateWorker_SadisticRageTantrum.tmpThings.Any();
				MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
				result = flag;
			}
			return result;
		}
	}
}
