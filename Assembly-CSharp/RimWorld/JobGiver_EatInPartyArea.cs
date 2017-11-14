using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_EatInPartyArea : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			float curLevelPercentage = pawn.needs.food.CurLevelPercentage;
			if ((double)curLevelPercentage > 0.9)
			{
				return null;
			}
			IntVec3 cell = duty.focus.Cell;
			Thing thing = this.FindFood(pawn, cell);
			if (thing == null)
			{
				return null;
			}
			Job job = new Job(JobDefOf.Ingest, thing);
			job.count = FoodUtility.WillIngestStackCountOf(pawn, thing.def);
			return job;
		}

		private Thing FindFood(Pawn pawn, IntVec3 partySpot)
		{
			Predicate<Thing> validator = delegate(Thing x)
			{
				if (!x.IngestibleNow)
				{
					return false;
				}
				if (!x.def.IsNutritionGivingIngestible)
				{
					return false;
				}
				if (!PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map))
				{
					return false;
				}
				if (x.def.IsDrug)
				{
					return false;
				}
				if ((int)x.def.ingestible.preferability <= 4)
				{
					return false;
				}
				if (!pawn.RaceProps.WillAutomaticallyEat(x))
				{
					return false;
				}
				if (x.IsForbidden(pawn))
				{
					return false;
				}
				if (!x.IsSociallyProper(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(x, 1, -1, null, false))
				{
					return false;
				}
				return true;
			};
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false);
		}
	}
}
