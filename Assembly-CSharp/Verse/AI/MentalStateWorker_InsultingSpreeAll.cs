using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A64 RID: 2660
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x04002558 RID: 9560
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06003B35 RID: 15157 RVA: 0x001F69C0 File Offset: 0x001F4DC0
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
