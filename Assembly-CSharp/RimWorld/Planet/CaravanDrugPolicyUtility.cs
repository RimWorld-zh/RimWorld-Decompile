using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D5 RID: 1493
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x06001D66 RID: 7526 RVA: 0x000FCD30 File Offset: 0x000FB130
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000FCD6C File Offset: 0x000FB16C
		private static void TryTakeScheduledDrugs(Pawn pawn, Caravan caravan)
		{
			if (pawn.drugs != null)
			{
				DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
				for (int i = 0; i < currentPolicy.Count; i++)
				{
					if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug))
					{
						Thing drug;
						Pawn drugOwner;
						if (CaravanInventoryUtility.TryGetThingOfDef(caravan, currentPolicy[i].drug, out drug, out drugOwner))
						{
							CaravanPawnsNeedsUtility.IngestDrug(pawn, drug, drugOwner, caravan);
						}
					}
				}
			}
		}
	}
}
