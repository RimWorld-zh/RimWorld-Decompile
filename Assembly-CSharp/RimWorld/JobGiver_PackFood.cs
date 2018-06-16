using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E7 RID: 231
	public class JobGiver_PackFood : ThinkNode_JobGiver
	{
		// Token: 0x060004FA RID: 1274 RVA: 0x000376F8 File Offset: 0x00035AF8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.inventory == null)
			{
				result = null;
			}
			else
			{
				float invNutrition = this.GetInventoryPackableFoodNutrition(pawn);
				if (invNutrition > 0.4f)
				{
					result = null;
				}
				else if (pawn.Map.resourceCounter.TotalHumanEdibleNutrition < (float)pawn.Map.mapPawns.ColonistsSpawnedCount * 1.5f)
				{
					result = null;
				}
				else
				{
					Thing thing = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 20f, delegate(Thing t)
					{
						bool result2;
						if (!this.IsGoodPackableFoodFor(t, pawn) || t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, false) || !t.IsSociallyProper(pawn))
						{
							result2 = false;
						}
						else
						{
							float num3 = invNutrition + t.GetStatValue(StatDefOf.Nutrition, true) * (float)t.stackCount;
							if (num3 < 0.8f)
							{
								result2 = false;
							}
							else
							{
								List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, t, FoodUtility.GetFinalIngestibleDef(t, false));
								for (int i = 0; i < list.Count; i++)
								{
									if (list[i].stages[0].baseMoodEffect < 0f)
									{
										return false;
									}
								}
								result2 = true;
							}
						}
						return result2;
					}, (Thing x) => FoodUtility.FoodOptimality(pawn, x, FoodUtility.GetFinalIngestibleDef(x, false), 0f, false), 24, 30);
					if (thing == null)
					{
						result = null;
					}
					else
					{
						float num = pawn.needs.food.MaxLevel - invNutrition;
						int num2 = Mathf.FloorToInt(num / thing.GetStatValue(StatDefOf.Nutrition, true));
						num2 = Mathf.Min(num2, thing.stackCount);
						num2 = Mathf.Max(num2, 1);
						result = new Job(JobDefOf.TakeInventory, thing)
						{
							count = num2
						};
					}
				}
			}
			return result;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00037868 File Offset: 0x00035C68
		private float GetInventoryPackableFoodNutrition(Pawn pawn)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			float num = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.IsGoodPackableFoodFor(innerContainer[i], pawn))
				{
					num += innerContainer[i].GetStatValue(StatDefOf.Nutrition, true) * (float)innerContainer[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000378E0 File Offset: 0x00035CE0
		private bool IsGoodPackableFoodFor(Thing food, Pawn forPawn)
		{
			return food.def.IsNutritionGivingIngestible && food.def.EverHaulable && food.def.ingestible.preferability >= FoodPreferability.MealAwful && forPawn.RaceProps.CanEverEat(food);
		}

		// Token: 0x040002C3 RID: 707
		private const float MaxInvNutritionToConsiderLookingForFood = 0.4f;

		// Token: 0x040002C4 RID: 708
		private const float MinFinalInvNutritionToPickUp = 0.8f;

		// Token: 0x040002C5 RID: 709
		private const float MinNutritionPerColonistToDo = 1.5f;

		// Token: 0x040002C6 RID: 710
		public const FoodPreferability MinFoodPreferability = FoodPreferability.MealAwful;
	}
}
