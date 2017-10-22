using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class HealthAIUtility
	{
		public static bool ShouldSeekMedicalRestUrgent(Pawn pawn)
		{
			return pawn.Downed || pawn.health.HasHediffsNeedingTend(false) || HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn);
		}

		public static bool ShouldSeekMedicalRest(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || HealthAIUtility.HasTendedImmunizableNonInjuryNonMissingPartHediff(pawn);
		}

		public static bool ShouldBeTendedNowUrgent(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldBeTendedNow(pawn))
			{
				return false;
			}
			return HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 15000;
		}

		public static bool ShouldBeTendedNow(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return false;
			}
			if (!HealthAIUtility.ShouldEverReceiveMedicalCare(pawn))
			{
				return false;
			}
			return pawn.health.HasHediffsNeedingTendByColony(false);
		}

		public static bool ShouldEverReceiveMedicalCare(Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.medCare == MedicalCareCategory.NoCare)
			{
				return false;
			}
			if (pawn.guest != null && pawn.guest.interactionMode == PrisonerInteractionModeDefOf.Execution)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) != null)
			{
				return false;
			}
			return true;
		}

		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow;
		}

		public static bool HasTendedImmunizableNonInjuryNonMissingPartHediff(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (!(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && hediffs[i].Visible && hediffs[i].IsTended() && hediffs[i].def.PossibleToDevelopImmunityNaturally())
				{
					return true;
				}
			}
			return false;
		}

		public static Thing FindBestMedicine(Pawn healer, Pawn patient)
		{
			if (patient.playerSettings != null && (int)patient.playerSettings.medCare > 1)
			{
				Predicate<Thing> predicate = (Predicate<Thing>)delegate(Thing m)
				{
					if (!m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 1, -1, null, false))
					{
						return true;
					}
					return false;
				};
				Func<Thing, float> priorityGetter = (Func<Thing, float>)((Thing t) => t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
				Predicate<Thing> validator = predicate;
				return GenClosest.ClosestThing_Global_Reachable(patient.Position, patient.Map, patient.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine), PathEndMode.ClosestTouch, TraverseParms.For(healer, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, priorityGetter);
			}
			return null;
		}
	}
}
