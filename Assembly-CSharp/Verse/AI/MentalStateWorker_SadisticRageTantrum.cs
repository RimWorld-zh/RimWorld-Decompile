using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A6B RID: 2667
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x06003B43 RID: 15171 RVA: 0x001F6720 File Offset: 0x001F4B20
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

		// Token: 0x04002561 RID: 9569
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
