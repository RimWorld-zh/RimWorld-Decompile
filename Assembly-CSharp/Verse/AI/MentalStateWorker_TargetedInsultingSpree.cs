using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A67 RID: 2663
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x06003B39 RID: 15161 RVA: 0x001F65FC File Offset: 0x001F49FC
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

		// Token: 0x0400255D RID: 9565
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
