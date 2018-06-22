using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A64 RID: 2660
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x06003B37 RID: 15159 RVA: 0x001F6958 File Offset: 0x001F4D58
		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				MentalStateWorker_TantrumAll.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
				bool flag = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
				MentalStateWorker_TantrumAll.tmpThings.Clear();
				result = flag;
			}
			return result;
		}

		// Token: 0x04002559 RID: 9561
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
