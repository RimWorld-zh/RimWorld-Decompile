using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000139 RID: 313
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		// Token: 0x06000661 RID: 1633 RVA: 0x00042900 File Offset: 0x00040D00
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
					if (job2 != null)
					{
						result = job2;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00042960 File Offset: 0x00040D60
		private Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			Job result;
			if (prisoner.Downed || !warden.CanReserve(prisoner, 1, -1, null, false))
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
					result = new Job(JobDefOf.EscortPrisonerToBed, prisoner, building_Bed)
					{
						count = 1
					};
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00042A00 File Offset: 0x00040E00
		private Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			Job result;
			if (!prisoner.Downed || !HealthAIUtility.ShouldSeekMedicalRestUrgent(prisoner) || prisoner.InBed() || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, true, false);
				if (building_Bed != null)
				{
					result = new Job(JobDefOf.TakeWoundedPrisonerToBed, prisoner, building_Bed)
					{
						count = 1
					};
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
