using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class FoodUtility
	{
		private static HashSet<Thing> filtered = new HashSet<Thing>();

		private static readonly SimpleCurve FoodOptimalityEffectFromMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, -600f),
				true
			},
			{
				new CurvePoint(-10f, -100f),
				true
			},
			{
				new CurvePoint(-5f, -70f),
				true
			},
			{
				new CurvePoint(-1f, -50f),
				true
			},
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(100f, 800f),
				true
			}
		};

		private static List<ThoughtDef> ingestThoughts = new List<ThoughtDef>();

		public static bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false)
		{
			bool flag = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			bool allowDrug = !eater.IsTeetotaler();
			Thing thing = null;
			if (canUseInventory)
			{
				if (flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, null, FoodPreferability.MealAwful, FoodPreferability.MealLavish, 0f, false);
				}
				if (thing != null)
				{
					if (getter.Faction != Faction.OfPlayer)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
			}
			bool allowPlant = getter == eater;
			ThingDef thingDef = default(ThingDef);
			Thing thing2 = FoodUtility.BestFoodSourceOnMap(getter, eater, desperate, out thingDef, FoodPreferability.MealLavish, allowPlant, allowDrug, allowCorpse, true, canRefillDispenser, allowForbidden, allowSociallyImproper, allowHarvest);
			if (thing == null && thing2 == null)
			{
				if (canUseInventory && flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, null, FoodPreferability.DesperateOnly, FoodPreferability.MealLavish, 0f, allowDrug);
					if (thing != null)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				if (thing2 == null && getter == eater && (getter.RaceProps.predator || getter.IsWildMan()))
				{
					Pawn pawn = FoodUtility.BestPawnToHuntForPredator(getter);
					if (pawn != null)
					{
						foodSource = pawn;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				foodSource = null;
				foodDef = null;
				return false;
			}
			if (thing == null && thing2 != null)
			{
				foodSource = thing2;
				foodDef = thingDef;
				return true;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			if (thing2 == null)
			{
				foodSource = thing;
				foodDef = finalIngestibleDef;
				return true;
			}
			float num = FoodUtility.FoodOptimality(eater, thing2, thingDef, (float)(getter.Position - thing2.Position).LengthManhattan, false);
			float num2 = FoodUtility.FoodOptimality(eater, thing, finalIngestibleDef, 0f, false);
			num2 = (float)(num2 - 32.0);
			if (num > num2)
			{
				foodSource = thing2;
				foodDef = thingDef;
				return true;
			}
			foodSource = thing;
			foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
			return true;
		}

		public static ThingDef GetFinalIngestibleDef(Thing foodSource, bool harvest = false)
		{
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			if (building_NutrientPasteDispenser != null)
			{
				return building_NutrientPasteDispenser.DispensableDef;
			}
			Pawn pawn = foodSource as Pawn;
			if (pawn != null)
			{
				return pawn.RaceProps.corpseDef;
			}
			if (harvest)
			{
				Plant plant = foodSource as Plant;
				if (plant != null && plant.HarvestableNow && plant.def.plant.harvestedThingDef.IsIngestible)
				{
					return plant.def.plant.harvestedThingDef;
				}
			}
			return foodSource.def;
		}

		public static Thing BestFoodInInventory(Pawn holder, Pawn eater = null, FoodPreferability minFoodPref = FoodPreferability.NeverForNutrition, FoodPreferability maxFoodPref = FoodPreferability.MealLavish, float minStackNutrition = 0f, bool allowDrug = false)
		{
			if (holder.inventory == null)
			{
				return null;
			}
			if (eater == null)
			{
				eater = holder;
			}
			ThingOwner<Thing> innerContainer = holder.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (thing.def.IsNutritionGivingIngestible && thing.IngestibleNow && eater.RaceProps.CanEverEat(thing) && (int)thing.def.ingestible.preferability >= (int)minFoodPref && (int)thing.def.ingestible.preferability <= (int)maxFoodPref && (allowDrug || !thing.def.IsDrug))
				{
					float num = thing.def.ingestible.nutrition * (float)thing.stackCount;
					if (num >= minStackNutrition)
					{
						return thing;
					}
				}
			}
			return null;
		}

		public static Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false)
		{
			foodDef = null;
			bool getterCanManipulate = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			if (!getterCanManipulate && getter != eater)
			{
				Log.Error(getter + " tried to find food to bring to " + eater + " but " + getter + " is incapable of Manipulation.");
				return null;
			}
			FoodPreferability minPref;
			if (eater.NonHumanlikeOrWildMan())
			{
				minPref = FoodPreferability.NeverForNutrition;
			}
			else if (desperate)
			{
				minPref = FoodPreferability.DesperateOnly;
			}
			else
			{
				minPref = (FoodPreferability)(((int)eater.needs.food.CurCategory <= 2) ? 4 : 6);
			}
			Predicate<Thing> foodValidator = delegate(Thing t)
			{
				if (!allowForbidden && t.IsForbidden(getter))
				{
					return false;
				}
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null)
				{
					if (allowDispenserFull && (int)ThingDefOf.MealNutrientPaste.ingestible.preferability >= (int)minPref && (int)ThingDefOf.MealNutrientPaste.ingestible.preferability <= (int)maxPref && getterCanManipulate && !getter.IsWildMan() && (t.Faction == getter.Faction || t.Faction == getter.HostFaction) && building_NutrientPasteDispenser.powerComp.PowerOn && (allowDispenserEmpty || building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers()) && FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) && t.InteractionCell.Standable(t.Map) && getter.Map.reachability.CanReachNonLocal(getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(getter, Danger.Some, TraverseMode.ByPawn, false)))
					{
						goto IL_025e;
					}
					return false;
				}
				if ((int)t.def.ingestible.preferability < (int)minPref)
				{
					return false;
				}
				if ((int)t.def.ingestible.preferability > (int)maxPref)
				{
					return false;
				}
				if (t.IngestibleNow && t.def.IsNutritionGivingIngestible && (allowCorpse || !(t is Corpse)) && (allowDrug || !t.def.IsDrug) && (desperate || !t.IsNotFresh()) && !t.IsDessicated() && eater.RaceProps.WillAutomaticallyEat(t) && FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) && getter.AnimalAwareOf(t) && getter.CanReserve(t, 1, -1, null, false))
				{
					goto IL_025e;
				}
				return false;
				IL_025e:
				return true;
			};
			ThingRequest thingRequest = ((eater.RaceProps.foodType & (FoodTypeFlags.Plant | FoodTypeFlags.Tree)) == FoodTypeFlags.None || !allowPlant) ? ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree) : ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
			Thing bestThing;
			if (getter.RaceProps.Humanlike)
			{
				Pawn eater2 = eater;
				IntVec3 position = getter.Position;
				List<Thing> searchSet = getter.Map.listerThings.ThingsMatching(thingRequest);
				PathEndMode peMode = PathEndMode.ClosestTouch;
				TraverseParms traverseParams = TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false);
				Predicate<Thing> validator = foodValidator;
				bestThing = FoodUtility.SpawnedFoodSearchInnerScan(eater2, position, searchSet, peMode, traverseParams, 9999f, validator);
				if (allowHarvest && getterCanManipulate)
				{
					Thing thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), PathEndMode.Touch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
					{
						Plant plant = (Plant)x;
						if (!plant.HarvestableNow)
						{
							return false;
						}
						ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
						if (!harvestedThingDef.IsNutritionGivingIngestible)
						{
							return false;
						}
						if (!getter.CanReserve(plant, 1, -1, null, false))
						{
							return false;
						}
						if (!allowForbidden && plant.IsForbidden(getter))
						{
							return false;
						}
						if (bestThing != null && (int)FoodUtility.GetFinalIngestibleDef(bestThing, false).ingestible.preferability >= (int)harvestedThingDef.ingestible.preferability)
						{
							return false;
						}
						return true;
					}, null, 0, 30, false, RegionType.Set_Passable, false);
					if (thing != null)
					{
						bestThing = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
					}
				}
				if (foodDef == null && bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			else
			{
				int searchRegionsMax = 30;
				if (getter.Faction == Faction.OfPlayer)
				{
					searchRegionsMax = 100;
				}
				FoodUtility.filtered.Clear();
				foreach (Thing item in GenRadial.RadialDistinctThingsAround(getter.Position, getter.Map, 2f, true))
				{
					Pawn pawn = item as Pawn;
					if (pawn != null && pawn != getter && pawn.RaceProps.Animal && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest && pawn.CurJob.GetTarget(TargetIndex.A).HasThing)
					{
						FoodUtility.filtered.Add(pawn.CurJob.GetTarget(TargetIndex.A).Thing);
					}
				}
				bool flag = !allowForbidden && ForbidUtility.CaresAboutForbidden(getter, true) && getter.playerSettings != null && getter.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap != null;
				Predicate<Thing> predicate = delegate(Thing t)
				{
					if (!foodValidator(t))
					{
						return false;
					}
					if (FoodUtility.filtered.Contains(t))
					{
						return false;
					}
					if (!(t is Building_NutrientPasteDispenser) && (int)t.def.ingestible.preferability <= 2)
					{
						return false;
					}
					if (t.IsNotFresh())
					{
						return false;
					}
					return true;
				};
				IntVec3 position = getter.Position;
				Map map = getter.Map;
				ThingRequest thingReq = thingRequest;
				PathEndMode peMode = PathEndMode.ClosestTouch;
				TraverseParms traverseParams = TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false);
				Predicate<Thing> validator = predicate;
				bool ignoreEntirelyForbiddenRegions = flag;
				bestThing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, searchRegionsMax, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				FoodUtility.filtered.Clear();
				if (bestThing == null)
				{
					desperate = true;
					position = getter.Position;
					map = getter.Map;
					thingReq = thingRequest;
					peMode = PathEndMode.ClosestTouch;
					traverseParams = TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false);
					validator = foodValidator;
					ignoreEntirelyForbiddenRegions = flag;
					bestThing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, searchRegionsMax, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				}
				if (bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			return bestThing;
		}

		private static bool IsFoodSourceOnMapSociallyProper(Thing t, Pawn getter, Pawn eater, bool allowSociallyImproper)
		{
			if (!allowSociallyImproper)
			{
				bool animalsCare = !getter.RaceProps.Animal;
				if (!t.IsSociallyProper(getter) && !t.IsSociallyProper(eater, eater.IsPrisonerOfColony, animalsCare))
				{
					return false;
				}
			}
			return true;
		}

		public static float FoodOptimality(Pawn eater, Thing foodSource, ThingDef foodDef, float dist, bool takingToInventory = false)
		{
			float num = 300f;
			num -= dist;
			switch (foodDef.ingestible.preferability)
			{
			case FoodPreferability.NeverForNutrition:
				return -9999999f;
			case FoodPreferability.DesperateOnly:
				num = (float)(num - 150.0);
				break;
			case FoodPreferability.DesperateOnlyForHumanlikes:
				if (eater.RaceProps.Humanlike)
				{
					num = (float)(num - 150.0);
				}
				break;
			}
			CompRottable compRottable = foodSource.TryGetComp<CompRottable>();
			if (compRottable != null)
			{
				if (compRottable.Stage == RotStage.Dessicated)
				{
					return -9999999f;
				}
				if (!takingToInventory && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
				{
					num = (float)(num + 12.0);
				}
			}
			if (eater.needs != null && eater.needs.mood != null)
			{
				List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(eater, foodSource, foodDef);
				for (int i = 0; i < list.Count; i++)
				{
					num += FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].stages[0].baseMoodEffect);
				}
			}
			if (foodDef.ingestible != null)
			{
				if (eater.RaceProps.Humanlike)
				{
					num += foodDef.ingestible.optimalityOffsetHumanlikes;
				}
				else if (eater.RaceProps.Animal)
				{
					num += foodDef.ingestible.optimalityOffsetFeedingAnimals;
				}
			}
			return num;
		}

		private static Thing SpawnedFoodSearchInnerScan(Pawn eater, IntVec3 root, List<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null)
		{
			if (searchSet == null)
			{
				return null;
			}
			Pawn pawn = traverseParams.pawn ?? eater;
			int num = 0;
			int num2 = 0;
			Thing result = null;
			float num3 = 0f;
			float num4 = -3.40282347E+38f;
			for (int i = 0; i < searchSet.Count; i++)
			{
				Thing thing = searchSet[i];
				num2++;
				float num5 = (float)(root - thing.Position).LengthManhattan;
				if (!(num5 > maxDistance))
				{
					num3 = FoodUtility.FoodOptimality(eater, thing, FoodUtility.GetFinalIngestibleDef(thing, false), num5, false);
					if (!(num3 < num4) && pawn.Map.reachability.CanReach(root, thing, peMode, traverseParams) && thing.Spawned && (validator == null || validator(thing)))
					{
						result = thing;
						num4 = num3;
						num++;
					}
				}
			}
			return result;
		}

		public static void DebugFoodSearchFromMouse_Update()
		{
			IntVec3 root = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null && pawn.Map == Find.VisibleMap)
			{
				Thing thing = FoodUtility.SpawnedFoodSearchInnerScan(pawn, root, Find.VisibleMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), 9999f, null);
				if (thing != null)
				{
					GenDraw.DrawLineBetween(root.ToVector3Shifted(), thing.Position.ToVector3Shifted());
				}
			}
		}

		public static void DebugFoodSearchFromMouse_OnGUI()
		{
			IntVec3 a = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null && pawn.Map == Find.VisibleMap)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				foreach (Thing item in Find.VisibleMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
				{
					ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(item, false);
					float num = FoodUtility.FoodOptimality(pawn, item, finalIngestibleDef, (a - item.Position).LengthHorizontal, false);
					Vector2 vector = item.DrawPos.MapToUIPosition();
					Rect rect = new Rect((float)(vector.x - 100.0), (float)(vector.y - 100.0), 200f, 200f);
					string text = num.ToString("F0");
					List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, item, finalIngestibleDef);
					for (int i = 0; i < list.Count; i++)
					{
						string text2 = text;
						text = text2 + "\n" + list[i].defName + "(" + FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].stages[0].baseMoodEffect).ToString("F0") + ")";
					}
					Widgets.Label(rect, text);
				}
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		private static Pawn BestPawnToHuntForPredator(Pawn predator)
		{
			if (predator.meleeVerbs.TryGetMeleeVerb() == null)
			{
				return null;
			}
			bool flag = false;
			float summaryHealthPercent = predator.health.summaryHealth.SummaryHealthPercent;
			if (summaryHealthPercent < 0.25)
			{
				flag = true;
			}
			List<Pawn> allPawnsSpawned = predator.Map.mapPawns.AllPawnsSpawned;
			Pawn pawn = null;
			float num = 0f;
			bool tutorialMode = TutorSystem.TutorialMode;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (predator.GetRoom(RegionType.Set_Passable) == pawn2.GetRoom(RegionType.Set_Passable) && predator != pawn2 && (!flag || pawn2.Downed) && FoodUtility.IsAcceptablePreyFor(predator, pawn2) && predator.CanReach(pawn2, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) && !pawn2.IsForbidden(predator) && (!tutorialMode || pawn2.Faction != Faction.OfPlayer))
				{
					float preyScoreFor = FoodUtility.GetPreyScoreFor(predator, pawn2);
					if (preyScoreFor > num || pawn == null)
					{
						num = preyScoreFor;
						pawn = pawn2;
					}
				}
			}
			return pawn;
		}

		public static bool IsAcceptablePreyFor(Pawn predator, Pawn prey)
		{
			if (!prey.RaceProps.canBePredatorPrey)
			{
				return false;
			}
			if (!prey.RaceProps.IsFlesh)
			{
				return false;
			}
			if (prey.BodySize > predator.RaceProps.maxPreyBodySize)
			{
				return false;
			}
			if (!prey.Downed)
			{
				if (prey.kindDef.combatPower > 2.0 * predator.kindDef.combatPower)
				{
					return false;
				}
				float num = prey.kindDef.combatPower * prey.health.summaryHealth.SummaryHealthPercent * prey.ageTracker.CurLifeStage.bodySizeFactor;
				float num2 = predator.kindDef.combatPower * predator.health.summaryHealth.SummaryHealthPercent * predator.ageTracker.CurLifeStage.bodySizeFactor;
				if (num > 0.85000002384185791 * num2)
				{
					return false;
				}
			}
			if (predator.Faction != null && prey.Faction != null && !predator.HostileTo(prey))
			{
				return false;
			}
			if (predator.Faction == Faction.OfPlayer && prey.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (predator.RaceProps.herdAnimal && predator.def == prey.def)
			{
				return false;
			}
			return true;
		}

		public static float GetPreyScoreFor(Pawn predator, Pawn prey)
		{
			float num = prey.kindDef.combatPower / predator.kindDef.combatPower;
			float num2 = prey.health.summaryHealth.SummaryHealthPercent;
			float bodySizeFactor = prey.ageTracker.CurLifeStage.bodySizeFactor;
			float lengthHorizontal = (predator.Position - prey.Position).LengthHorizontal;
			if (prey.Downed)
			{
				num2 = Mathf.Min(num2, 0.2f);
			}
			float num3 = (float)(0.0 - lengthHorizontal - 56.0 * num2 * num2 * num * bodySizeFactor);
			if (prey.RaceProps.Humanlike)
			{
				num3 = (float)(num3 - 35.0);
			}
			return num3;
		}

		public static void DebugDrawPredatorFoodSource()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			Thing thing = default(Thing);
			ThingDef thingDef = default(ThingDef);
			if (pawn != null && FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, false, false, true, false, false))
			{
				GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), thing.Position.ToVector3Shifted());
				if (!(thing is Pawn))
				{
					Pawn pawn2 = FoodUtility.BestPawnToHuntForPredator(pawn);
					if (pawn2 != null)
					{
						GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), pawn2.Position.ToVector3Shifted());
					}
				}
			}
		}

		public static List<ThoughtDef> ThoughtsFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef)
		{
			FoodUtility.ingestThoughts.Clear();
			if (ingester.needs != null && ingester.needs.mood != null)
			{
				if (!ingester.story.traits.HasTrait(TraitDefOf.Ascetic) && foodDef.ingestible.tasteThought != null)
				{
					FoodUtility.ingestThoughts.Add(foodDef.ingestible.tasteThought);
				}
				CompIngredients compIngredients = foodSource.TryGetComp<CompIngredients>();
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
				if (FoodUtility.IsHumanlikeMeat(foodDef) && ingester.RaceProps.Humanlike)
				{
					FoodUtility.ingestThoughts.Add((!ingester.story.traits.HasTrait(TraitDefOf.Cannibal)) ? ThoughtDefOf.AteHumanlikeMeatDirect : ThoughtDefOf.AteHumanlikeMeatDirectCannibal);
				}
				else if (compIngredients != null)
				{
					for (int i = 0; i < compIngredients.ingredients.Count; i++)
					{
						FoodUtility.AddIngestThoughtsFromIngredient(compIngredients.ingredients[i], ingester, FoodUtility.ingestThoughts);
					}
				}
				else if (building_NutrientPasteDispenser != null)
				{
					Thing thing = building_NutrientPasteDispenser.FindFeedInAnyHopper();
					if (thing != null)
					{
						FoodUtility.AddIngestThoughtsFromIngredient(thing.def, ingester, FoodUtility.ingestThoughts);
					}
				}
				if (foodDef.ingestible.specialThoughtDirect != null)
				{
					FoodUtility.ingestThoughts.Add(foodDef.ingestible.specialThoughtDirect);
				}
				if (foodSource.IsNotFresh())
				{
					FoodUtility.ingestThoughts.Add(ThoughtDefOf.AteRottenFood);
				}
				return FoodUtility.ingestThoughts;
			}
			return FoodUtility.ingestThoughts;
		}

		private static void AddIngestThoughtsFromIngredient(ThingDef ingredient, Pawn ingester, List<ThoughtDef> ingestThoughts)
		{
			if (ingredient.ingestible != null)
			{
				if (ingester.RaceProps.Humanlike && FoodUtility.IsHumanlikeMeat(ingredient))
				{
					ingestThoughts.Add((!ingester.story.traits.HasTrait(TraitDefOf.Cannibal)) ? ThoughtDefOf.AteHumanlikeMeatAsIngredient : ThoughtDefOf.AteHumanlikeMeatAsIngredientCannibal);
				}
				else if (ingredient.ingestible.specialThoughtAsIngredient != null)
				{
					ingestThoughts.Add(ingredient.ingestible.specialThoughtAsIngredient);
				}
			}
		}

		public static bool IsHumanlikeMeat(ThingDef def)
		{
			if (def.ingestible.sourceDef != null && def.ingestible.sourceDef.race != null && def.ingestible.sourceDef.race.Humanlike)
			{
				return true;
			}
			return false;
		}

		public static bool IsHumanlikeMeatOrHumanlikeCorpse(Thing thing)
		{
			if (FoodUtility.IsHumanlikeMeat(thing.def))
			{
				return true;
			}
			Corpse corpse = thing as Corpse;
			if (corpse != null && corpse.InnerPawn.RaceProps.Humanlike)
			{
				return true;
			}
			return false;
		}

		public static int WillIngestStackCountOf(Pawn ingester, ThingDef def)
		{
			int num = Mathf.Min(def.ingestible.maxNumToIngestAtOnce, FoodUtility.StackCountForNutrition(def, ingester.needs.food.NutritionWanted));
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}

		public static float GetBodyPartNutrition(Pawn pawn, BodyPartRecord part)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			return (float)(5.1999998092651367 * pawn.BodySize * pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(part));
		}

		public static int StackCountForNutrition(ThingDef def, float nutrition)
		{
			if (nutrition <= 9.9999997473787516E-05)
			{
				return 0;
			}
			return Mathf.Max(Mathf.RoundToInt(nutrition / def.ingestible.nutrition), 1);
		}

		public static bool ShouldBeFedBySomeone(Pawn pawn)
		{
			return FeedPatientUtility.ShouldBeFed(pawn) || WardenFeedUtility.ShouldBeFed(pawn);
		}

		public static void AddFoodPoisoningHediff(Pawn pawn, Thing ingestible)
		{
			pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.FoodPoisoning, pawn, null), null, null);
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message("MessageFoodPoisoning".Translate(pawn.LabelShort, ingestible.LabelCapNoCount).CapitalizeFirst(), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
	}
}
