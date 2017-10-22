using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		private static List<Pawn> candidates = new List<Pawn>();

		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_InsultingSpreeAll.candidates, true);
				bool flag = MentalStateWorker_InsultingSpreeAll.candidates.Count >= 2;
				MentalStateWorker_InsultingSpreeAll.candidates.Clear();
				result = flag;
			}
			return result;
		}
	}
}
