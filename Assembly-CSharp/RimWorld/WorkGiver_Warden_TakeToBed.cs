using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				result = null;
			}
			else
			{
				Pawn prisoner = (Pawn)t;
				Job job = this.TakeDownedToBedJob(prisoner, pawn);
				if (job != null)
				{
					result = job;
				}
				else
				{
					Job job2 = this.TakeToPreferredBedJob(prisoner, pawn);
					result = ((job2 == null) ? null : job2);
				}
			}
			return result;
		}

		private Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			Job result;
			if (prisoner.Downed || !warden.CanReserve((Thing)prisoner, 1, -1, null, false))
			{
				result = null;
			}
			else if (RestUtility.FindBedFor(prisoner, prisoner, true, true, false) != null)
			{
				result = null;
			}
			else
			{
				Room room = prisoner.GetRoom(RegionType.Set_Passable);
				Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, false, false);
				if (building_Bed != null && building_Bed.GetRoom(RegionType.Set_Passable) != room)
				{
					Job job = new Job(JobDefOf.EscortPrisonerToBed, (Thing)prisoner, (Thing)building_Bed);
					job.count = 1;
					result = job;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		private Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			Job result;
			if (!prisoner.Downed || !HealthAIUtility.ShouldSeekMedicalRestUrgent(prisoner) || prisoner.InBed() || !warden.CanReserve((Thing)prisoner, 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, true, false);
				if (building_Bed != null)
				{
					Job job = new Job(JobDefOf.TakeWoundedPrisonerToBed, (Thing)prisoner, (Thing)building_Bed);
					job.count = 1;
					result = job;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
	}
}
