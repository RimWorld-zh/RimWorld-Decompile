using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A66 RID: 2662
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x0400255A RID: 9562
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B3B RID: 15163 RVA: 0x001F6A84 File Offset: 0x001F4E84
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
	}
}
