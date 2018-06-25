using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_PatientGoToBedTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		public WorkGiver_PatientGoToBedTreatment()
		{
		}

		public override Job NonScanJob(Pawn pawn)
		{
			Job result;
			if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
			{
				result = null;
			}
			else if (!this.AnyAvailableDoctorFor(pawn))
			{
				result = null;
			}
			else
			{
				result = base.NonScanJob(pawn);
			}
			return result;
		}

		private bool AnyAvailableDoctorFor(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			bool result;
			if (mapHeld == null || pawn.Faction == null)
			{
				result = false;
			}
			else
			{
				List<Pawn> list = mapHeld.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn2 = list[i];
					if (pawn2 != pawn && pawn2.RaceProps.Humanlike && !pawn2.Downed && pawn2.Awake() && !pawn2.InBed() && !pawn2.InMentalState && !pawn2.IsPrisoner && pawn2.workSettings != null && pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Doctor) && pawn2.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn2.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
