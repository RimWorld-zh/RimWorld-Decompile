using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A66 RID: 2662
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x04002569 RID: 9577
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06003B39 RID: 15161 RVA: 0x001F6D50 File Offset: 0x001F5150
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
	}
}
