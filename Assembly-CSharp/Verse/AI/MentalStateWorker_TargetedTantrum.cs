using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A68 RID: 2664
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x0400255C RID: 9564
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B41 RID: 15169 RVA: 0x001F6BA4 File Offset: 0x001F4FA4
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
