using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000AE RID: 174
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x04000281 RID: 641
		private const int BaseIngestInterval = 1100;

		// Token: 0x06000434 RID: 1076 RVA: 0x0003203C File Offset: 0x0003043C
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00032058 File Offset: 0x00030458
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing thing;
			ThingDef thingDef;
			Thing result;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, true, true, true, true, false, false))
			{
				result = thing;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
