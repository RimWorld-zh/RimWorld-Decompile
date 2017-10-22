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
			Job result;
			if (duty == null)
			{
				result = null;
			}
			else
			{
				float curLevelPercentage = pawn.needs.food.CurLevelPercentage;
				if ((double)curLevelPercentage > 0.9)
				{
					result = null;
				}
				else
				{
					IntVec3 cell = duty.focus.Cell;
					Thing thing = this.FindFood(pawn, cell);
					if (thing == null)
					{
						result = null;
					}
					else
					{
						Job job = new Job(JobDefOf.Ingest, thing);
						job.count = FoodUtility.WillIngestStackCountOf(pawn, thing.def);
						result = job;
					}
				}
			}
			return result;
		}

		private Thing FindFood(Pawn pawn, IntVec3 partySpot)
		{
			Predicate<Thing> validator = (Predicate<Thing>)((Thing x) => (byte)(x.IngestibleNow ? (x.def.IsNutritionGivingIngestible ? (PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map) ? ((!x.def.IsDrug) ? (((int)x.def.ingestible.preferability > 4) ? (pawn.RaceProps.WillAutomaticallyEat(x) ? ((!x.IsForbidden(pawn)) ? (x.IsSociallyProper(pawn) ? (pawn.CanReserve(x, 1, -1, null, false) ? 1 : 0) : 0) : 0) : 0) : 0) : 0) : 0) : 0) : 0) != 0);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false);
		}
	}
}
