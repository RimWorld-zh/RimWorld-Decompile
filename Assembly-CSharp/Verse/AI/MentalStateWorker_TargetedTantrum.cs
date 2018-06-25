using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A69 RID: 2665
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x0400256C RID: 9580
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B42 RID: 15170 RVA: 0x001F6ED0 File Offset: 0x001F52D0
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
	}
}
