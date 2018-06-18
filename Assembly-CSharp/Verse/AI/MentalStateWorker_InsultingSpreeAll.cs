using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A66 RID: 2662
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x06003B36 RID: 15158 RVA: 0x001F6598 File Offset: 0x001F4998
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

		// Token: 0x0400255C RID: 9564
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
