using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A65 RID: 2661
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x04002559 RID: 9561
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06003B38 RID: 15160 RVA: 0x001F6A24 File Offset: 0x001F4E24
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
