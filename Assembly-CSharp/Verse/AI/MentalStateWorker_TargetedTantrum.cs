using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A6A RID: 2666
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x06003B42 RID: 15170 RVA: 0x001F677C File Offset: 0x001F4B7C
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
