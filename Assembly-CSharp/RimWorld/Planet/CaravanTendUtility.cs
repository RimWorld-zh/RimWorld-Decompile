using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E5 RID: 1509
	public static class CaravanTendUtility
	{
		// Token: 0x06001DD4 RID: 7636 RVA: 0x00100D2C File Offset: 0x000FF12C
		public static void TryTendToRandomPawn(Caravan caravan)
		{
			CaravanTendUtility.FindPawnsNeedingTend(caravan, CaravanTendUtility.tmpPawnsNeedingTreatment);
			if (CaravanTendUtility.tmpPawnsNeedingTreatment.Any<Pawn>())
			{
				Pawn patient = CaravanTendUtility.tmpPawnsNeedingTreatment.RandomElement<Pawn>();
				Pawn pawn = CaravanTendUtility.FindBestDoctor(caravan, patient);
				if (pawn != null)
				{
					Medicine medicine = null;
					Pawn pawn2 = null;
					CaravanInventoryUtility.TryGetBestMedicine(caravan, patient, out medicine, out pawn2);
					TendUtility.DoTend(pawn, patient, medicine);
					if (medicine != null && medicine.Destroyed && pawn2 != null)
					{
						pawn2.inventory.innerContainer.Remove(medicine);
					}
					CaravanTendUtility.tmpPawnsNeedingTreatment.Clear();
				}
			}
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x00100DC4 File Offset: 0x000FF1C4
		private static void FindPawnsNeedingTend(Caravan caravan, List<Pawn> outPawnsNeedingTend)
		{
			outPawnsNeedingTend.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (pawn.playerSettings == null || pawn.playerSettings.medCare > MedicalCareCategory.NoCare)
				{
					if (pawn.health.HasHediffsNeedingTend(false))
					{
						outPawnsNeedingTend.Add(pawn);
					}
				}
			}
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x00100E3C File Offset: 0x000FF23C
		private static Pawn FindBestDoctor(Caravan caravan, Pawn patient)
		{
			float num = 0f;
			Pawn pawn = null;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (CaravanUtility.IsOwner(pawn2, caravan.Faction))
				{
					if (pawn2 != patient || (pawn2.IsColonist && pawn2.playerSettings.selfTend))
					{
						if (!pawn2.Downed && !pawn2.InMentalState)
						{
							if (pawn2.story == null || !pawn2.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
							{
								float statValue = pawn2.GetStatValue(StatDefOf.MedicalTendQuality, true);
								if (statValue > num || pawn == null)
								{
									num = statValue;
									pawn = pawn2;
								}
							}
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x040011A6 RID: 4518
		private static List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();
	}
}
