using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A66 RID: 2662
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x06003B3D RID: 15165 RVA: 0x001F6A78 File Offset: 0x001F4E78
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

		// Token: 0x0400255B RID: 9563
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
