using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_DeliverFood : WorkGiver_Warden
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
				Pawn pawn2 = (Pawn)t;
				Thing thing = default(Thing);
				ThingDef def = default(ThingDef);
				if (!pawn2.guest.CanBeBroughtFood)
				{
					result = null;
				}
				else if (!pawn2.Position.IsInPrisonCell(pawn2.Map))
				{
					result = null;
				}
				else if (pawn2.needs.food.CurLevelPercentage >= pawn2.needs.food.PercentageThreshHungry + 0.019999999552965164)
				{
					result = null;
				}
				else if (WardenFeedUtility.ShouldBeFed(pawn2))
				{
					result = null;
				}
				else if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out def, false, true, false, false, false, false))
				{
					result = null;
				}
				else if (thing.GetRoom(RegionType.Set_Passable) == pawn2.GetRoom(RegionType.Set_Passable))
				{
					result = null;
				}
				else if (WorkGiver_Warden_DeliverFood.FoodAvailableInRoomTo(pawn2))
				{
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.DeliverFood, thing, (Thing)pawn2);
					job.count = FoodUtility.WillIngestStackCountOf(pawn2, def);
					job.targetC = RCellFinder.SpotToChewStandingNear(pawn2, thing);
					result = job;
				}
			}
			return result;
		}

		private static bool FoodAvailableInRoomTo(Pawn prisoner)
		{
			bool result;
			if (prisoner.carryTracker.CarriedThing != null && WorkGiver_Warden_DeliverFood.NutritionAvailableForFrom(prisoner, prisoner.carryTracker.CarriedThing) > 0.0)
			{
				result = true;
			}
			else
			{
				float num = 0f;
				float num2 = 0f;
				Room room = prisoner.GetRoom(RegionType.Set_Passable);
				if (room == null)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < room.RegionCount; i++)
					{
						Region region = room.Regions[i];
						List<Thing> list = region.ListerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
						for (int j = 0; j < list.Count; j++)
						{
							Thing thing = list[j];
							if (!thing.def.IsIngestible || (int)thing.def.ingestible.preferability > 3)
							{
								num2 += WorkGiver_Warden_DeliverFood.NutritionAvailableForFrom(prisoner, thing);
							}
						}
						List<Thing> list2 = region.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
						for (int k = 0; k < list2.Count; k++)
						{
							Pawn pawn = list2[k] as Pawn;
							if (pawn.IsPrisonerOfColony && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry + 0.019999999552965164 && (pawn.carryTracker.CarriedThing == null || !pawn.RaceProps.WillAutomaticallyEat(pawn.carryTracker.CarriedThing)))
							{
								num += pawn.needs.food.NutritionWanted;
							}
						}
					}
					result = ((byte)((num2 + 0.5 >= num) ? 1 : 0) != 0);
				}
			}
			return result;
		}

		private static float NutritionAvailableForFrom(Pawn p, Thing foodSource)
		{
			float result;
			if (foodSource.def.IsNutritionGivingIngestible && p.RaceProps.WillAutomaticallyEat(foodSource))
			{
				result = foodSource.def.ingestible.nutrition * (float)foodSource.stackCount;
			}
			else
			{
				if (p.RaceProps.ToolUser && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
					if (building_NutrientPasteDispenser != null && building_NutrientPasteDispenser.CanDispenseNow)
					{
						result = 99999f;
						goto IL_009a;
					}
				}
				result = 0f;
			}
			goto IL_009a;
			IL_009a:
			return result;
		}
	}
}
