using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A62 RID: 2658
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x04002557 RID: 9559
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06003B31 RID: 15153 RVA: 0x001F6894 File Offset: 0x001F4C94
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
