using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanTendUtility
	{
		private static List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();

		private const int TendIntervalTicks = 1250;

		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		public static void CheckTend(Caravan caravan)
		{
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn = caravan.pawns[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn, null, caravan))
				{
					if (pawn.IsHashIntervalTick(1250))
					{
						CaravanTendUtility.TryTendToAnyPawn(caravan);
					}
				}
			}
		}

		public static void TryTendToAnyPawn(Caravan caravan)
		{
			CaravanTendUtility.FindPawnsNeedingTend(caravan, CaravanTendUtility.tmpPawnsNeedingTreatment);
			if (CaravanTendUtility.tmpPawnsNeedingTreatment.Any<Pawn>())
			{
				CaravanTendUtility.tmpPawnsNeedingTreatment.SortByDescending((Pawn x) => CaravanTendUtility.GetTendPriority(x));
				Pawn patient = null;
				Pawn pawn = null;
				for (int i = 0; i < CaravanTendUtility.tmpPawnsNeedingTreatment.Count; i++)
				{
					patient = CaravanTendUtility.tmpPawnsNeedingTreatment[i];
					pawn = CaravanTendUtility.FindBestDoctorFor(caravan, patient);
					if (pawn != null)
					{
						break;
					}
				}
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

		private static Pawn FindBestDoctorFor(Caravan caravan, Pawn patient)
		{
			float num = 0f;
			Pawn pawn = null;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn2, patient, caravan))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.MedicalTendQuality, true);
					if (statValue > num || pawn == null)
					{
						num = statValue;
						pawn = pawn2;
					}
				}
			}
			return pawn;
		}

		private static bool IsValidDoctorFor(Pawn doctor, Pawn patient, Caravan caravan)
		{
			return doctor.RaceProps.Humanlike && caravan.IsOwner(doctor) && (doctor != patient || (doctor.IsColonist && doctor.playerSettings.selfTend)) && !doctor.Downed && !doctor.InMentalState && (doctor.story == null || !doctor.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor));
		}

		private static float GetTendPriority(Pawn patient)
		{
			int num = HealthUtility.TicksUntilDeathDueToBloodLoss(patient);
			float result;
			if (num < 15000)
			{
				if (patient.RaceProps.Humanlike)
				{
					result = GenMath.LerpDouble(0f, 15000f, 5f, 4f, (float)num);
				}
				else
				{
					result = GenMath.LerpDouble(0f, 15000f, 4f, 3f, (float)num);
				}
			}
			else
			{
				for (int i = 0; i < patient.health.hediffSet.hediffs.Count; i++)
				{
					Hediff hediff = patient.health.hediffSet.hediffs[i];
					HediffStage curStage = hediff.CurStage;
					if ((curStage != null && curStage.lifeThreatening) || hediff.def.lethalSeverity >= 0f)
					{
						if (hediff.TendableNow(false))
						{
							if (patient.RaceProps.Humanlike)
							{
								return 2.5f;
							}
							return 2f;
						}
					}
				}
				if (patient.health.hediffSet.BleedRateTotal >= 0.0001f)
				{
					if (patient.RaceProps.Humanlike)
					{
						result = 1.5f;
					}
					else
					{
						result = 1f;
					}
				}
				else if (patient.RaceProps.Humanlike)
				{
					result = 0.5f;
				}
				else
				{
					result = 0f;
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CaravanTendUtility()
		{
		}

		[CompilerGenerated]
		private static float <TryTendToAnyPawn>m__0(Pawn x)
		{
			return CaravanTendUtility.GetTendPriority(x);
		}
	}
}
