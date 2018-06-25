using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A67 RID: 2663
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x0400256A RID: 9578
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B3C RID: 15164 RVA: 0x001F6DB0 File Offset: 0x001F51B0
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
