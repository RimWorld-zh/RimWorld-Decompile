using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E1 RID: 1505
	public static class CaravanTendUtility
	{
		// Token: 0x040011A3 RID: 4515
		private static List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();

		// Token: 0x06001DCB RID: 7627 RVA: 0x00100D80 File Offset: 0x000FF180
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

		// Token: 0x06001DCC RID: 7628 RVA: 0x00100E18 File Offset: 0x000FF218
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

		// Token: 0x06001DCD RID: 7629 RVA: 0x00100E90 File Offset: 0x000FF290
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
	}
}
