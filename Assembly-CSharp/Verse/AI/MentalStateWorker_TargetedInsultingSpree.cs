using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A63 RID: 2659
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x04002558 RID: 9560
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06003B34 RID: 15156 RVA: 0x001F68F8 File Offset: 0x001F4CF8
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
