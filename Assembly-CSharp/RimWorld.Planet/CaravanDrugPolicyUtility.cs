using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanDrugPolicyUtility
	{
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		private static void TryTakeScheduledDrugs(Pawn pawn, Caravan caravan)
		{
			if (pawn.drugs != null)
			{
				DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
				for (int i = 0; i < currentPolicy.Count; i++)
				{
					Thing drug = default(Thing);
					Pawn drugOwner = default(Pawn);
					if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug) && CaravanInventoryUtility.TryGetThingOfDef(caravan, currentPolicy[i].drug, out drug, out drugOwner))
					{
						CaravanPawnsNeedsUtility.IngestDrug(pawn, drug, drugOwner, caravan);
					}
				}
			}
		}
	}
}
