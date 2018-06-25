using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		private static List<Pawn> candidates = new List<Pawn>();

		public MentalStateWorker_TargetedInsultingSpree()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_TargetedInsultingSpree.candidates, false);
				bool flag = MentalStateWorker_TargetedInsultingSpree.candidates.Any<Pawn>();
				MentalStateWorker_TargetedInsultingSpree.candidates.Clear();
				result = flag;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MentalStateWorker_TargetedInsultingSpree()
		{
		}
	}
}
