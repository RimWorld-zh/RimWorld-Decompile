using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000134 RID: 308
	public class WorkGiver_Warden_DeliverFood : WorkGiver_Warden
	{
		// Token: 0x06000653 RID: 1619 RVA: 0x00042268 File Offset: 0x00040668
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
				Thing thing;
				ThingDef thingDef;
				if (!pawn2.guest.CanBeBroughtFood)
				{
					result = null;
				}
				else if (!pawn2.Position.IsInPrisonCell(pawn2.Map))
				{
					result = null;
				}
				else if (pawn2.needs.food.CurLevelPercentage >= pawn2.needs.food.PercentageThreshHungry + 0.02f)
				{
					result = null;
				}
				else if (WardenFeedUtility.ShouldBeFed(pawn2))
				{
					result = null;
				}
				else if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, false, false, false, false))
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
					float nutrition = FoodUtility.GetNutrition(thing, thingDef);
					result = new Job(JobDefOf.DeliverFood, thing, pawn2)
					{
						count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition),
						targetC = RCellFinder.SpotToChewStandingNear(pawn2, thing)
					};
				}
			}
			return result;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000423B4 File Offset: 0x000407B4
		private static bool FoodAvailableInRoomTo(Pawn prisoner)
		{
			bool result;
			if (prisoner.carryTracker.CarriedThing != null && WorkGiver_Warden_DeliverFood.NutritionAvailableForFrom(prisoner, prisoner.carryTracker.CarriedThing) > 0f)
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
							if (!thing.def.IsIngestible || thing.def.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes)
							{
								num2 += WorkGiver_Warden_DeliverFood.NutritionAvailableForFrom(prisoner, thing);
							}
						}
						List<Thing> list2 = region.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
						for (int k = 0; k < list2.Count; k++)
						{
							Pawn pawn = list2[k] as Pawn;
							if (pawn.IsPrisonerOfColony && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry + 0.02f && (pawn.carryTracker.CarriedThing == null || !pawn.RaceProps.WillAutomaticallyEat(pawn.carryTracker.CarriedThing)))
							{
								num += pawn.needs.food.NutritionWanted;
							}
						}
					}
					result = (num2 + 0.5f >= num);
				}
			}
			return result;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00042588 File Offset: 0x00040988
		private static float NutritionAvailableForFrom(Pawn p, Thing foodSource)
		{
			float result;
			if (foodSource.def.IsNutritionGivingIngestible && p.RaceProps.WillAutomaticallyEat(foodSource))
			{
				result = foodSource.GetStatValue(StatDefOf.Nutrition, true) * (float)foodSource.stackCount;
			}
			else
			{
				if (p.RaceProps.ToolUser && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
					if (building_NutrientPasteDispenser != null && building_NutrientPasteDispenser.CanDispenseNow)
					{
						return 99999f;
					}
				}
				result = 0f;
			}
			return result;
		}
	}
}
