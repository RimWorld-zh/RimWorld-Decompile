using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn prisoner = (Pawn)t;
			Job job = this.TakeDownedToBedJob(prisoner, pawn);
			if (job != null)
			{
				return job;
			}
			Job job2 = this.TakeToPreferredBedJob(prisoner, pawn);
			if (job2 != null)
			{
				return job2;
			}
			return null;
		}

		private Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Downed && warden.CanReserve((Thing)prisoner, 1, -1, null, false))
			{
				if (RestUtility.FindBedFor(prisoner, prisoner, true, true, false) != null)
				{
					return null;
				}
				Room room = prisoner.GetRoom(RegionType.Set_Passable);
				Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, false, false);
				if (building_Bed != null && building_Bed.GetRoom(RegionType.Set_Passable) != room)
				{
					Job job = new Job(JobDefOf.EscortPrisonerToBed, (Thing)prisoner, (Thing)building_Bed);
					job.count = 1;
					return job;
				}
				return null;
			}
			return null;
		}

		private Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			if (prisoner.Downed && HealthAIUtility.ShouldSeekMedicalRestUrgent(prisoner) && !prisoner.InBed() && warden.CanReserve((Thing)prisoner, 1, -1, null, false))
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, true, false);
				if (building_Bed != null)
				{
					Job job = new Job(JobDefOf.TakeWoundedPrisonerToBed, (Thing)prisoner, (Thing)building_Bed);
					job.count = 1;
					return job;
				}
				return null;
			}
			return null;
		}
	}
}
