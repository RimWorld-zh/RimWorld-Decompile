using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000C9 RID: 201
	public class JobGiver_EatInPartyArea : ThinkNode_JobGiver
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x0003470C File Offset: 0x00032B0C
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
						result = new Job(JobDefOf.Ingest, thing)
						{
							count = FoodUtility.WillIngestStackCountOf(pawn, thing.def, thing.def.GetStatValueAbstract(StatDefOf.Nutrition, null))
						};
					}
				}
			}
			return result;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000347C4 File Offset: 0x00032BC4
		private Thing FindFood(Pawn pawn, IntVec3 partySpot)
		{
			Predicate<Thing> validator = (Thing x) => x.IngestibleNow && x.def.IsNutritionGivingIngestible && PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map) && !x.def.IsDrug && x.def.ingestible.preferability > FoodPreferability.RawBad && pawn.RaceProps.WillAutomaticallyEat(x) && !x.IsForbidden(pawn) && x.IsSociallyProper(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false);
		}
	}
}
