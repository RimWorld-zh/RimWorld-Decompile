using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 != null && pawn2 != pawn && !pawn2.InBed() && pawn2.RaceProps.IsFlesh && HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2))
			{
				LocalTargetInfo target = (Thing)pawn2;
				if (pawn.CanReserve(target, 1, -1, null, forced) && (!pawn2.InMentalState || !pawn2.MentalStateDef.IsAggro))
				{
					if (!pawn2.Downed)
					{
						if (pawn2.IsColonist)
						{
							result = false;
							goto IL_011d;
						}
						if (!pawn2.IsPrisonerOfColony && pawn2.Faction != Faction.OfPlayer)
						{
							result = false;
							goto IL_011d;
						}
						if (pawn2.guest != null && pawn2.guest.Released)
						{
							result = false;
							goto IL_011d;
						}
					}
					Building_Bed building_Bed = base.FindBed(pawn, pawn2);
					result = ((byte)((building_Bed != null && pawn2.CanReserve((Thing)building_Bed, building_Bed.SleepingSlotsCount, -1, null, false)) ? 1 : 0) != 0);
					goto IL_011d;
				}
			}
			result = false;
			goto IL_011d;
			IL_011d:
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Building_Bed t2 = base.FindBed(pawn, pawn2);
			Job job = new Job(JobDefOf.TakeToBedToOperate, (Thing)pawn2, (Thing)t2);
			job.count = 1;
			return job;
		}
	}
}
