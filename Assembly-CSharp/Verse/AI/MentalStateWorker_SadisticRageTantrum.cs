using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A67 RID: 2663
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x0400255C RID: 9564
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B40 RID: 15168 RVA: 0x001F6AF0 File Offset: 0x001F4EF0
		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x), 0, 40);
				bool flag = MentalStateWorker_SadisticRageTantrum.tmpThings.Any<Thing>();
				MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
				result = flag;
			}
			return result;
		}
	}
}
