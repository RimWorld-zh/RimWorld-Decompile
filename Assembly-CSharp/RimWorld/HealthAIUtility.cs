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
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || pawn.health.hediffSet.HasTendedImmunizableNotImmuneHediff();
		}

		public static bool ShouldBeTendedNowUrgent(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNow(pawn) && HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 15000;
		}

		public static bool ShouldBeTendedNow(Pawn pawn)
		{
			return pawn.playerSettings != null && HealthAIUtility.ShouldEverReceiveMedicalCare(pawn) && pawn.health.HasHediffsNeedingTendByColony(false);
		}

		public static bool ShouldEverReceiveMedicalCare(Pawn pawn)
		{
			return (byte)((((pawn.playerSettings == null) ? MedicalCareCategory.NoMeds : pawn.playerSettings.medCare) != 0) ? ((pawn.guest == null || pawn.guest.interactionMode != PrisonerInteractionModeDefOf.Execution) ? ((pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) == null) ? 1 : 0) : 0) : 0) != 0;
		}

		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow;
		}

		public static Thing FindBestMedicine(Pawn healer, Pawn patient)
		{
			Thing result;
			if (patient.playerSettings == null || (int)patient.playerSettings.medCare <= 1)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> predicate = (Predicate<Thing>)((Thing m) => (byte)((!m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 1, -1, null, false)) ? 1 : 0) != 0);
				Func<Thing, float> priorityGetter = (Func<Thing, float>)((Thing t) => t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
				IntVec3 position = patient.Position;
				Map map = patient.Map;
				List<Thing> searchSet = patient.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine);
				PathEndMode peMode = PathEndMode.ClosestTouch;
				TraverseParms traverseParams = TraverseParms.For(healer, Danger.Deadly, TraverseMode.ByPawn, false);
				Predicate<Thing> validator = predicate;
				result = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, priorityGetter);
			}
			return result;
		}
	}
}
