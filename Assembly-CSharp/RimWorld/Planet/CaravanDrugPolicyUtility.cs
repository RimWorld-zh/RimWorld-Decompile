using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D9 RID: 1497
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x06001D6F RID: 7535 RVA: 0x000FCCDC File Offset: 0x000FB0DC
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000FCD18 File Offset: 0x000FB118
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
