using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A69 RID: 2665
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x0400255D RID: 9565
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003B44 RID: 15172 RVA: 0x001F6C1C File Offset: 0x001F501C
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
