using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CFB RID: 3323
	public static class HealthAIUtility
	{
		// Token: 0x06004934 RID: 18740 RVA: 0x00267ACC File Offset: 0x00265ECC
		public static bool ShouldSeekMedicalRestUrgent(Pawn pawn)
		{
			return pawn.Downed || pawn.health.HasHediffsNeedingTend(false) || HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn);
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00267B08 File Offset: 0x00265F08
		public static bool ShouldSeekMedicalRest(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || pawn.health.hediffSet.HasTendedImmunizableNotImmuneHediff();
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00267B50 File Offset: 0x00265F50
		public static bool ShouldBeTendedNowByPlayerUrgent(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNowByPlayer(pawn) && HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 45000;
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00267B80 File Offset: 0x00265F80
		public static bool ShouldBeTendedNowByPlayer(Pawn pawn)
		{
			return pawn.playerSettings != null && HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(pawn) && pawn.health.HasHediffsNeedingTendByPlayer(false);
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x00267BC8 File Offset: 0x00265FC8
		public static bool ShouldEverReceiveMedicalCareFromPlayer(Pawn pawn)
		{
			return (pawn.playerSettings == null || pawn.playerSettings.medCare != MedicalCareCategory.NoCare) && (pawn.guest == null || pawn.guest.interactionMode != PrisonerInteractionModeDefOf.Execution) && pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) == null;
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x00267C4C File Offset: 0x0026604C
		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow;
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00267C74 File Offset: 0x00266074
		public static Thing FindBestMedicine(Pawn healer, Pawn patient)
		{
			Thing result;
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				result = null;
			}
			else if (Medicine.GetMedicineCountToFullyHeal(patient) <= 0)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> predicate = (Thing m) => !m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 10, 1, null, false);
				Func<Thing, float> priorityGetter = (Thing t) => t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
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
