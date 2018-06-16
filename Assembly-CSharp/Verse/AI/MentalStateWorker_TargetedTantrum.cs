using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A6A RID: 2666
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x06003B40 RID: 15168 RVA: 0x001F66A8 File Offset: 0x001F4AA8
		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				MentalStateWorker_TargetedTantrum.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TargetedTantrum.tmpThings, null, 300, 40);
				bool flag = MentalStateWorker_TargetedTantrum.tmpThings.Any<Thing>();
				MentalStateWorker_TargetedTantrum.tmpThings.Clear();
				result = flag;
			}
			return result;
		}

		// Token: 0x04002560 RID: 9568
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
