using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
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

		public static bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false)
		{
			Profiler.BeginSample("TryFindBestFoodSourceFor");
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
						Profiler.EndSample();
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
					{
						Profiler.EndSample();
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
			}
			ThingDef thingDef;
			ref ThingDef foodDef2 = ref thingDef;
			bool allowPlant = getter == eater;
			Thing thing2 = FoodUtility.BestFoodSourceOnMap(getter, eater, desperate, out foodDef2, FoodPreferability.MealLavish, allowPlant, allowDrug, allowCorpse, true, canRefillDispenser, allowForbidden, allowSociallyImproper, allowHarvest, forceScanWholeMap);
			bool result;
			if (thing != null || thing2 != null)
			{
				if (thing == null && thing2 != null)
				{
					Profiler.EndSample();
					foodSource = thing2;
					foodDef = thingDef;
					result = true;
				}
				else
				{
					ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
					if (thing2 == null)
					{
						Profiler.EndSample();
						foodSource = thing;
						foodDef = finalIngestibleDef;
						result = true;
					}
					else
					{
						float num = FoodUtility.FoodOptimality(eater, thing2, thingDef, (float)(getter.Position - thing2.Position).LengthManhattan, false);
						float num2 = FoodUtility.FoodOptimality(eater, thing, finalIngestibleDef, 0f, false);
						num2 -= 32f;
						if (num > num2)
						{
							Profiler.EndSample();
							foodSource = thing2;
							foodDef = thingDef;
							result = true;
						}
						else
						{
							Profiler.EndSample();
							foodSource = thing;
							foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
							result = true;
						}
					}
				}
			}
			else
			{
				if (canUseInventory && flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, null, FoodPreferability.DesperateOnly, FoodPreferability.MealLavish, 0f, allowDrug);
					if (thing != null)
					{
						Profiler.EndSample();
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
						Profiler.EndSample();
						foodSource = pawn;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				Profiler.EndSample();
				foodSource = null;
				foodDef = null;
				result = false;
			}
			return result;
		}

		public static ThingDef GetFinalIngestibleDef(Thing foodSource, bool harvest = false)
		{
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			ThingDef result;
			if (building_NutrientPasteDispenser != null)
			{
				result = building_NutrientPasteDispenser.DispensableDef;
			}
			else
			{
				Pawn pawn = foodSource as Pawn;
				if (pawn != null)
				{
					result = pawn.RaceProps.corpseDef;
				}
				else
				{
					if (harvest)
					{
						Plant plant = foodSource as Plant;
						if (plant != null && plant.HarvestableNow && plant.def.plant.harvestedThingDef.IsIngestible)
						{
							return plant.def.plant.harvestedThingDef;
						}
					}
					result = foodSource.def;
				}
			}
			return result;
		}

		public static Thing BestFoodInInventory(Pawn holder, Pawn eater = null, FoodPreferability minFoodPref = FoodPreferability.NeverForNutrition, FoodPreferability maxFoodPref = FoodPreferability.MealLavish, float minStackNutrition = 0f, bool allowDrug = false)
		{
			Thing result;
			if (holder.inventory == null)
			{
				result = null;
			}
			else
			{
				if (eater == null)
				{
					eater = holder;
				}
				ThingOwner<Thing> innerContainer = holder.inventory.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Thing thing = innerContainer[i];
					if (thing.def.IsNutritionGivingIngestible && thing.IngestibleNow && eater.RaceProps.CanEverEat(thing) && thing.def.ingestible.preferability >= minFoodPref && thing.def.ingestible.preferability <= maxFoodPref && (allowDrug || !thing.def.IsDrug))
					{
						float num = thing.GetStatValue(StatDefOf.Nutrition, true) * (float)thing.stackCount;
						if (num >= minStackNutrition)
						{
							return thing;
						}
					}
				}
				result = null;
			}
			return result;
		}

		public static Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false)
		{
			FoodUtility.<BestFoodSourceOnMap>c__AnonStorey0 <BestFoodSourceOnMap>c__AnonStorey = new FoodUtility.<BestFoodSourceOnMap>c__AnonStorey0();
			<BestFoodSourceOnMap>c__AnonStorey.allowDispenserFull = allowDispenserFull;
			<BestFoodSourceOnMap>c__AnonStorey.maxPref = maxPref;
			<BestFoodSourceOnMap>c__AnonStorey.eater = eater;
			<BestFoodSourceOnMap>c__AnonStorey.getter = getter;
			<BestFoodSourceOnMap>c__AnonStorey.allowForbidden = allowForbidden;
			<BestFoodSourceOnMap>c__AnonStorey.allowDispenserEmpty = allowDispenserEmpty;
			<BestFoodSourceOnMap>c__AnonStorey.allowSociallyImproper = allowSociallyImproper;
			<BestFoodSourceOnMap>c__AnonStorey.allowCorpse = allowCorpse;
			<BestFoodSourceOnMap>c__AnonStorey.allowDrug = allowDrug;
			<BestFoodSourceOnMap>c__AnonStorey.desperate = desperate;
			<BestFoodSourceOnMap>c__AnonStorey.forceScanWholeMap = forceScanWholeMap;
			foodDef = null;
			Profiler.BeginSample("BestFoodInWorldFor getter=" + <BestFoodSourceOnMap>c__AnonStorey.getter.LabelCap + " eater=" + <BestFoodSourceOnMap>c__AnonStorey.eater.LabelCap);
			<BestFoodSourceOnMap>c__AnonStorey.getterCanManipulate = (<BestFoodSourceOnMap>c__AnonStorey.getter.RaceProps.ToolUser && <BestFoodSourceOnMap>c__AnonStorey.getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
			Thing result;
			if (!<BestFoodSourceOnMap>c__AnonStorey.getterCanManipulate && <BestFoodSourceOnMap>c__AnonStorey.getter != <BestFoodSourceOnMap>c__AnonStorey.eater)
			{
				Log.Error(string.Concat(new object[]
				{
					<BestFoodSourceOnMap>c__AnonStorey.getter,
					" tried to find food to bring to ",
					<BestFoodSourceOnMap>c__AnonStorey.eater,
					" but ",
					<BestFoodSourceOnMap>c__AnonStorey.getter,
					" is incapable of Manipulation."
				}), false);
				Profiler.EndSample();
				result = null;
			}
			else
			{
				if (<BestFoodSourceOnMap>c__AnonStorey.eater.NonHumanlikeOrWildMan())
				{
					<BestFoodSourceOnMap>c__AnonStorey.minPref = FoodPreferability.NeverForNutrition;
				}
				else if (<BestFoodSourceOnMap>c__AnonStorey.desperate)
				{
					<BestFoodSourceOnMap>c__AnonStorey.minPref = FoodPreferability.DesperateOnly;
				}
				else
				{
					<BestFoodSourceOnMap>c__AnonStorey.minPref = ((<BestFoodSourceOnMap>c__AnonStorey.eater.needs.food.CurCategory < HungerCategory.UrgentlyHungry) ? FoodPreferability.MealAwful : FoodPreferability.RawBad);
				}
				<BestFoodSourceOnMap>c__AnonStorey.foodValidator = delegate(Thing t)
				{
					Profiler.BeginSample("foodValidator");
					Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
					if (building_NutrientPasteDispenser != null)
					{
						if (!<BestFoodSourceOnMap>c__AnonStorey.allowDispenserFull || !<BestFoodSourceOnMap>c__AnonStorey.getterCanManipulate || ThingDefOf.MealNutrientPaste.ingestible.preferability < <BestFoodSourceOnMap>c__AnonStorey.minPref || ThingDefOf.MealNutrientPaste.ingestible.preferability > <BestFoodSourceOnMap>c__AnonStorey.maxPref || !<BestFoodSourceOnMap>c__AnonStorey.eater.RaceProps.CanEverEat(ThingDefOf.MealNutrientPaste) || (t.Faction != <BestFoodSourceOnMap>c__AnonStorey.getter.Faction && t.Faction != <BestFoodSourceOnMap>c__AnonStorey.getter.HostFaction) || (!<BestFoodSourceOnMap>c__AnonStorey.allowForbidden && t.IsForbidden(<BestFoodSourceOnMap>c__AnonStorey.getter)) || (!building_NutrientPasteDispenser.powerComp.PowerOn || (!<BestFoodSourceOnMap>c__AnonStorey.allowDispenserEmpty && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())) || !t.InteractionCell.Standable(t.Map) || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, <BestFoodSourceOnMap>c__AnonStorey.getter, <BestFoodSourceOnMap>c__AnonStorey.eater, <BestFoodSourceOnMap>c__AnonStorey.allowSociallyImproper) || <BestFoodSourceOnMap>c__AnonStorey.getter.IsWildMan() || !<BestFoodSourceOnMap>c__AnonStorey.getter.Map.reachability.CanReachNonLocal(<BestFoodSourceOnMap>c__AnonStorey.getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(<BestFoodSourceOnMap>c__AnonStorey.getter, Danger.Some, TraverseMode.ByPawn, false)))
						{
							Profiler.EndSample();
							return false;
						}
					}
					else if (t.def.ingestible.preferability < <BestFoodSourceOnMap>c__AnonStorey.minPref || t.def.ingestible.preferability > <BestFoodSourceOnMap>c__AnonStorey.maxPref || !<BestFoodSourceOnMap>c__AnonStorey.eater.RaceProps.WillAutomaticallyEat(t) || !t.def.IsNutritionGivingIngestible || !t.IngestibleNow || (!<BestFoodSourceOnMap>c__AnonStorey.allowCorpse && t is Corpse) || (!<BestFoodSourceOnMap>c__AnonStorey.allowDrug && t.def.IsDrug) || (!<BestFoodSourceOnMap>c__AnonStorey.allowForbidden && t.IsForbidden(<BestFoodSourceOnMap>c__AnonStorey.getter)) || (!<BestFoodSourceOnMap>c__AnonStorey.desperate && t.IsNotFresh()) || (t.IsDessicated() || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, <BestFoodSourceOnMap>c__AnonStorey.getter, <BestFoodSourceOnMap>c__AnonStorey.eater, <BestFoodSourceOnMap>c__AnonStorey.allowSociallyImproper) || (!<BestFoodSourceOnMap>c__AnonStorey.getter.AnimalAwareOf(t) && !<BestFoodSourceOnMap>c__AnonStorey.forceScanWholeMap)) || !<BestFoodSourceOnMap>c__AnonStorey.getter.CanReserve(t, 1, -1, null, false))
					{
						Profiler.EndSample();
						return false;
					}
					Profiler.EndSample();
					return true;
				};
				ThingRequest thingRequest;
				if ((<BestFoodSourceOnMap>c__AnonStorey.eater.RaceProps.foodType & (FoodTypeFlags.Plant | FoodTypeFlags.Tree)) != FoodTypeFlags.None && allowPlant)
				{
					thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
				}
				else
				{
					thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
				}
				if (<BestFoodSourceOnMap>c__AnonStorey.getter.RaceProps.Humanlike)
				{
					FoodUtility.<BestFoodSourceOnMap>c__AnonStorey0 <BestFoodSourceOnMap>c__AnonStorey2 = <BestFoodSourceOnMap>c__AnonStorey;
					Pawn eater2 = <BestFoodSourceOnMap>c__AnonStorey.eater;
					IntVec3 position = <BestFoodSourceOnMap>c__AnonStorey.getter.Position;
					List<Thing> searchSet = <BestFoodSourceOnMap>c__AnonStorey.getter.Map.listerThings.ThingsMatching(thingRequest);
					PathEndMode peMode = PathEndMode.ClosestTouch;
					TraverseParms traverseParams = TraverseParms.For(<BestFoodSourceOnMap>c__AnonStorey.getter, Danger.Deadly, TraverseMode.ByPawn, false);
					Predicate<Thing> validator = <BestFoodSourceOnMap>c__AnonStorey.foodValidator;
					<BestFoodSourceOnMap>c__AnonStorey2.bestThing = FoodUtility.SpawnedFoodSearchInnerScan(eater2, position, searchSet, peMode, traverseParams, 9999f, validator);
					if (allowHarvest && <BestFoodSourceOnMap>c__AnonStorey.getterCanManipulate)
					{
						int searchRegionsMax;
						if (<BestFoodSourceOnMap>c__AnonStorey.forceScanWholeMap && <BestFoodSourceOnMap>c__AnonStorey.bestThing == null)
						{
							searchRegionsMax = -1;
						}
						else
						{
							searchRegionsMax = 30;
						}
						Thing thing = GenClosest.ClosestThingReachable(<BestFoodSourceOnMap>c__AnonStorey.getter.Position, <BestFoodSourceOnMap>c__AnonStorey.getter.Map, ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), PathEndMode.Touch, TraverseParms.For(<BestFoodSourceOnMap>c__AnonStorey.getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
						{
							Plant plant = (Plant)x;
							bool result2;
							if (!plant.HarvestableNow)
							{
								result2 = false;
							}
							else
							{
								ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
								result2 = (harvestedThingDef.IsNutritionGivingIngestible && <BestFoodSourceOnMap>c__AnonStorey.getter.CanReserve(plant, 1, -1, null, false) && (<BestFoodSourceOnMap>c__AnonStorey.allowForbidden || !plant.IsForbidden(<BestFoodSourceOnMap>c__AnonStorey.getter)) && (<BestFoodSourceOnMap>c__AnonStorey.bestThing == null || FoodUtility.GetFinalIngestibleDef(<BestFoodSourceOnMap>c__AnonStorey.bestThing, false).ingestible.preferability < harvestedThingDef.ingestible.preferability));
							}
							return result2;
						}, null, 0, searchRegionsMax, false, RegionType.Set_Passable, false);
						if (thing != null)
						{
							<BestFoodSourceOnMap>c__AnonStorey.bestThing = thing;
							foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
						}
					}
					if (foodDef == null && <BestFoodSourceOnMap>c__AnonStorey.bestThing != null)
					{
						foodDef = FoodUtility.GetFinalIngestibleDef(<BestFoodSourceOnMap>c__AnonStorey.bestThing, false);
					}
				}
				else
				{
					int searchRegionsMax2;
					if (<BestFoodSourceOnMap>c__AnonStorey.forceScanWholeMap)
					{
						searchRegionsMax2 = -1;
					}
					else if (<BestFoodSourceOnMap>c__AnonStorey.getter.Faction == Faction.OfPlayer)
					{
						searchRegionsMax2 = 100;
					}
					else
					{
						searchRegionsMax2 = 30;
					}
					FoodUtility.filtered.Clear();
					foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(<BestFoodSourceOnMap>c__AnonStorey.getter.Position, <BestFoodSourceOnMap>c__AnonStorey.getter.Map, 2f, true))
					{
						Pawn pawn = thing2 as Pawn;
						if (pawn != null && pawn != <BestFoodSourceOnMap>c__AnonStorey.getter && pawn.RaceProps.Animal && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest && pawn.CurJob.GetTarget(TargetIndex.A).HasThing)
						{
							FoodUtility.filtered.Add(pawn.CurJob.GetTarget(TargetIndex.A).Thing);
						}
					}
					bool flag = !<BestFoodSourceOnMap>c__AnonStorey.allowForbidden && ForbidUtility.CaresAboutForbidden(<BestFoodSourceOnMap>c__AnonStorey.getter, true) && <BestFoodSourceOnMap>c__AnonStorey.getter.playerSettings != null && <BestFoodSourceOnMap>c__AnonStorey.getter.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap != null;
					Predicate<Thing> predicate = (Thing t) => <BestFoodSourceOnMap>c__AnonStorey.foodValidator(t) && !FoodUtility.filtered.Contains(t) && (t is Building_NutrientPasteDispenser || t.def.ingestible.preferability > FoodPreferability.DesperateOnly) && !t.IsNotFresh();
					FoodUtility.<BestFoodSourceOnMap>c__AnonStorey0 <BestFoodSourceOnMap>c__AnonStorey3 = <BestFoodSourceOnMap>c__AnonStorey;
					IntVec3 position = <BestFoodSourceOnMap>c__AnonStorey.getter.Position;
					Map map = <BestFoodSourceOnMap>c__AnonStorey.getter.Map;
					ThingRequest thingReq = thingRequest;
					PathEndMode peMode = PathEndMode.ClosestTouch;
					TraverseParms traverseParams = TraverseParms.For(<BestFoodSourceOnMap>c__AnonStorey.getter, Danger.Deadly, TraverseMode.ByPawn, false);
					Predicate<Thing> validator = predicate;
					bool ignoreEntirelyForbiddenRegions = flag;
					<BestFoodSourceOnMap>c__AnonStorey3.bestThing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, searchRegionsMax2, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
					FoodUtility.filtered.Clear();
					if (<BestFoodSourceOnMap>c__AnonStorey.bestThing == null)
					{
						<BestFoodSourceOnMap>c__AnonStorey.desperate = true;
						FoodUtility.<BestFoodSourceOnMap>c__AnonStorey0 <BestFoodSourceOnMap>c__AnonStorey4 = <BestFoodSourceOnMap>c__AnonStorey;
						position = <BestFoodSourceOnMap>c__AnonStorey.getter.Position;
						map = <BestFoodSourceOnMap>c__AnonStorey.getter.Map;
						thingReq = thingRequest;
						peMode = PathEndMode.ClosestTouch;
						traverseParams = TraverseParms.For(<BestFoodSourceOnMap>c__AnonStorey.getter, Danger.Deadly, TraverseMode.ByPawn, false);
						validator = <BestFoodSourceOnMap>c__AnonStorey.foodValidator;
						ignoreEntirelyForbiddenRegions = flag;
						<BestFoodSourceOnMap>c__AnonStorey4.bestThing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, searchRegionsMax2, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
					}
					if (<BestFoodSourceOnMap>c__AnonStorey.bestThing != null)
					{
						foodDef = FoodUtility.GetFinalIngestibleDef(<BestFoodSourceOnMap>c__AnonStorey.bestThing, false);
					}
				}
				Profiler.EndSample();
				result = <BestFoodSourceOnMap>c__AnonStorey.bestThing;
			}
			return result;
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
			FoodPreferability preferability = foodDef.ingestible.preferability;
			float result;
			if (preferability != FoodPreferability.NeverForNutrition)
			{
				if (preferability != FoodPreferability.DesperateOnly)
				{
					if (preferability == FoodPreferability.DesperateOnlyForHumanlikes)
					{
						if (eater.RaceProps.Humanlike)
						{
							num -= 150f;
						}
					}
				}
				else
				{
					num -= 150f;
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
						num += 12f;
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
				result = num;
			}
			else
			{
				result = -9999999f;
			}
			return result;
		}

		private static Thing SpawnedFoodSearchInnerScan(Pawn eater, IntVec3 root, List<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null)
		{
			Profiler.BeginSample("SpawnedFoodSearchInnerScan");
			Thing result;
			if (searchSet == null)
			{
				Profiler.EndSample();
				result = null;
			}
			else
			{
				Pawn pawn = traverseParams.pawn ?? eater;
				int num = 0;
				int num2 = 0;
				Thing thing = null;
				float num3 = float.MinValue;
				for (int i = 0; i < searchSet.Count; i++)
				{
					Thing thing2 = searchSet[i];
					num2++;
					float num4 = (float)(root - thing2.Position).LengthManhattan;
					if (num4 <= maxDistance)
					{
						float num5 = FoodUtility.FoodOptimality(eater, thing2, FoodUtility.GetFinalIngestibleDef(thing2, false), num4, false);
						if (num5 >= num3)
						{
							if (pawn.Map.reachability.CanReach(root, thing2, peMode, traverseParams))
							{
								if (thing2.Spawned)
								{
									if (validator == null || validator(thing2))
									{
										thing = thing2;
										num3 = num5;
										num++;
									}
								}
							}
						}
					}
				}
				Profiler.BeginSample(string.Concat(new object[]
				{
					"changedCount: ",
					num,
					" scanCount: ",
					num2
				}));
				Profiler.EndSample();
				Profiler.EndSample();
				result = thing;
			}
			return result;
		}

		public static void DebugFoodSearchFromMouse_Update()
		{
			IntVec3 root = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null)
			{
				if (pawn.Map == Find.CurrentMap)
				{
					Thing thing = FoodUtility.SpawnedFoodSearchInnerScan(pawn, root, Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), 9999f, null);
					if (thing != null)
					{
						GenDraw.DrawLineBetween(root.ToVector3Shifted(), thing.Position.ToVector3Shifted());
					}
				}
			}
		}

		public static void DebugFoodSearchFromMouse_OnGUI()
		{
			IntVec3 a = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null)
			{
				if (pawn.Map == Find.CurrentMap)
				{
					Text.Anchor = TextAnchor.MiddleCenter;
					Text.Font = GameFont.Tiny;
					foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
					{
						ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
						float num = FoodUtility.FoodOptimality(pawn, thing, finalIngestibleDef, (a - thing.Position).LengthHorizontal, false);
						Vector2 vector = thing.DrawPos.MapToUIPosition();
						Rect rect = new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f);
						string text = num.ToString("F0");
						List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, thing, finalIngestibleDef);
						for (int i = 0; i < list.Count; i++)
						{
							string text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								"\n",
								list[i].defName,
								"(",
								FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].stages[0].baseMoodEffect).ToString("F0"),
								")"
							});
						}
						Widgets.Label(rect, text);
					}
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
		}

		private static Pawn BestPawnToHuntForPredator(Pawn predator)
		{
			Pawn result;
			if (predator.meleeVerbs.TryGetMeleeVerb(null) == null)
			{
				result = null;
			}
			else
			{
				bool flag = false;
				float summaryHealthPercent = predator.health.summaryHealth.SummaryHealthPercent;
				if (summaryHealthPercent < 0.25f)
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
					if (predator.GetRoom(RegionType.Set_Passable) == pawn2.GetRoom(RegionType.Set_Passable))
					{
						if (predator != pawn2)
						{
							if (!flag || pawn2.Downed)
							{
								if (FoodUtility.IsAcceptablePreyFor(predator, pawn2))
								{
									if (predator.CanReach(pawn2, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
									{
										if (!pawn2.IsForbidden(predator))
										{
											if (!tutorialMode || pawn2.Faction != Faction.OfPlayer)
											{
												float preyScoreFor = FoodUtility.GetPreyScoreFor(predator, pawn2);
												if (preyScoreFor > num || pawn == null)
												{
													num = preyScoreFor;
													pawn = pawn2;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				result = pawn;
			}
			return result;
		}

		public static bool IsAcceptablePreyFor(Pawn predator, Pawn prey)
		{
			bool result;
			if (!prey.RaceProps.canBePredatorPrey)
			{
				result = false;
			}
			else if (!prey.RaceProps.IsFlesh)
			{
				result = false;
			}
			else if (!Find.Storyteller.difficulty.predatorsHuntHumanlikes && prey.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (prey.BodySize > predator.RaceProps.maxPreyBodySize)
			{
				result = false;
			}
			else
			{
				if (!prey.Downed)
				{
					if (prey.kindDef.combatPower > 2f * predator.kindDef.combatPower)
					{
						return false;
					}
					float num = prey.kindDef.combatPower * prey.health.summaryHealth.SummaryHealthPercent * prey.ageTracker.CurLifeStage.bodySizeFactor;
					float num2 = predator.kindDef.combatPower * predator.health.summaryHealth.SummaryHealthPercent * predator.ageTracker.CurLifeStage.bodySizeFactor;
					if (num > 0.85f * num2)
					{
						return false;
					}
				}
				result = ((predator.Faction == null || prey.Faction == null || predator.HostileTo(prey)) && (predator.Faction != Faction.OfPlayer || prey.Faction != Faction.OfPlayer) && (!predator.RaceProps.herdAnimal || predator.def != prey.def));
			}
			return result;
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
			float num3 = -lengthHorizontal - 56f * num2 * num2 * num * bodySizeFactor;
			if (prey.RaceProps.Humanlike)
			{
				num3 -= 35f;
			}
			return num3;
		}

		public static void DebugDrawPredatorFoodSource()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null)
			{
				Thing thing;
				ThingDef thingDef;
				if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, false, false, true, false, false, false))
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
		}

		public static List<ThoughtDef> ThoughtsFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef)
		{
			FoodUtility.ingestThoughts.Clear();
			List<ThoughtDef> result;
			if (ingester.needs == null || ingester.needs.mood == null)
			{
				result = FoodUtility.ingestThoughts;
			}
			else
			{
				if (!ingester.story.traits.HasTrait(TraitDefOf.Ascetic))
				{
					if (foodDef.ingestible.tasteThought != null)
					{
						FoodUtility.ingestThoughts.Add(foodDef.ingestible.tasteThought);
					}
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
				result = FoodUtility.ingestThoughts;
			}
			return result;
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
			return def.ingestible.sourceDef != null && def.ingestible.sourceDef.race != null && def.ingestible.sourceDef.race.Humanlike;
		}

		public static bool IsHumanlikeMeatOrHumanlikeCorpse(Thing thing)
		{
			bool result;
			if (FoodUtility.IsHumanlikeMeat(thing.def))
			{
				result = true;
			}
			else
			{
				Corpse corpse = thing as Corpse;
				result = (corpse != null && corpse.InnerPawn.RaceProps.Humanlike);
			}
			return result;
		}

		public static int WillIngestStackCountOf(Pawn ingester, ThingDef def, float singleFoodNutrition)
		{
			int num = Mathf.Min(def.ingestible.maxNumToIngestAtOnce, FoodUtility.StackCountForNutrition(ingester.needs.food.NutritionWanted, singleFoodNutrition));
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}

		public static float GetBodyPartNutrition(Corpse corpse, BodyPartRecord part)
		{
			return FoodUtility.GetBodyPartNutrition(corpse.GetStatValue(StatDefOf.Nutrition, true), corpse.InnerPawn, part);
		}

		public static float GetBodyPartNutrition(float currentCorpseNutrition, Pawn pawn, BodyPartRecord part)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			float coverageOfNotMissingNaturalParts = hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
			float result;
			if (coverageOfNotMissingNaturalParts <= 0f)
			{
				result = 0f;
			}
			else
			{
				float coverageOfNotMissingNaturalParts2 = hediffSet.GetCoverageOfNotMissingNaturalParts(part);
				float num = coverageOfNotMissingNaturalParts2 / coverageOfNotMissingNaturalParts;
				result = currentCorpseNutrition * num;
			}
			return result;
		}

		public static int StackCountForNutrition(float wantedNutrition, float singleFoodNutrition)
		{
			int result;
			if (wantedNutrition <= 0.0001f)
			{
				result = 0;
			}
			else
			{
				result = Mathf.Max(Mathf.RoundToInt(wantedNutrition / singleFoodNutrition), 1);
			}
			return result;
		}

		public static bool ShouldBeFedBySomeone(Pawn pawn)
		{
			return FeedPatientUtility.ShouldBeFed(pawn) || WardenFeedUtility.ShouldBeFed(pawn);
		}

		public static void AddFoodPoisoningHediff(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
		{
			pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.FoodPoisoning, pawn, null), null, null, null);
			if (PawnUtility.ShouldSendNotificationAbout(pawn) && MessagesRepeatAvoider.MessageShowAllowed("MessageFoodPoisoning-" + pawn.thingIDNumber, 0.1f))
			{
				string text = "MessageFoodPoisoning".Translate(new object[]
				{
					pawn.LabelShort,
					ingestible.LabelCapNoCount,
					cause.ToStringHuman().CapitalizeFirst()
				}).CapitalizeFirst();
				Messages.Message(text, pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		public static bool Starving(this Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.Starving;
		}

		public static float GetNutrition(Thing foodSource, ThingDef foodDef)
		{
			float result;
			if (foodSource == null || foodDef == null)
			{
				result = 0f;
			}
			else if (foodSource.def == foodDef)
			{
				result = foodSource.GetStatValue(StatDefOf.Nutrition, true);
			}
			else
			{
				result = foodDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FoodUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <BestFoodSourceOnMap>c__AnonStorey0
		{
			internal bool allowDispenserFull;

			internal bool getterCanManipulate;

			internal FoodPreferability minPref;

			internal FoodPreferability maxPref;

			internal Pawn eater;

			internal Pawn getter;

			internal bool allowForbidden;

			internal bool allowDispenserEmpty;

			internal bool allowSociallyImproper;

			internal bool allowCorpse;

			internal bool allowDrug;

			internal bool desperate;

			internal bool forceScanWholeMap;

			internal Thing bestThing;

			internal Predicate<Thing> foodValidator;

			public <BestFoodSourceOnMap>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				Profiler.BeginSample("foodValidator");
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null)
				{
					if (!this.allowDispenserFull || !this.getterCanManipulate || ThingDefOf.MealNutrientPaste.ingestible.preferability < this.minPref || ThingDefOf.MealNutrientPaste.ingestible.preferability > this.maxPref || !this.eater.RaceProps.CanEverEat(ThingDefOf.MealNutrientPaste) || (t.Faction != this.getter.Faction && t.Faction != this.getter.HostFaction) || (!this.allowForbidden && t.IsForbidden(this.getter)) || (!building_NutrientPasteDispenser.powerComp.PowerOn || (!this.allowDispenserEmpty && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())) || !t.InteractionCell.Standable(t.Map) || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, this.getter, this.eater, this.allowSociallyImproper) || this.getter.IsWildMan() || !this.getter.Map.reachability.CanReachNonLocal(this.getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(this.getter, Danger.Some, TraverseMode.ByPawn, false)))
					{
						Profiler.EndSample();
						return false;
					}
				}
				else if (t.def.ingestible.preferability < this.minPref || t.def.ingestible.preferability > this.maxPref || !this.eater.RaceProps.WillAutomaticallyEat(t) || !t.def.IsNutritionGivingIngestible || !t.IngestibleNow || (!this.allowCorpse && t is Corpse) || (!this.allowDrug && t.def.IsDrug) || (!this.allowForbidden && t.IsForbidden(this.getter)) || (!this.desperate && t.IsNotFresh()) || (t.IsDessicated() || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, this.getter, this.eater, this.allowSociallyImproper) || (!this.getter.AnimalAwareOf(t) && !this.forceScanWholeMap)) || !this.getter.CanReserve(t, 1, -1, null, false))
				{
					Profiler.EndSample();
					return false;
				}
				Profiler.EndSample();
				return true;
			}

			internal bool <>m__1(Thing x)
			{
				Plant plant = (Plant)x;
				bool result;
				if (!plant.HarvestableNow)
				{
					result = false;
				}
				else
				{
					ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
					result = (harvestedThingDef.IsNutritionGivingIngestible && this.getter.CanReserve(plant, 1, -1, null, false) && (this.allowForbidden || !plant.IsForbidden(this.getter)) && (this.bestThing == null || FoodUtility.GetFinalIngestibleDef(this.bestThing, false).ingestible.preferability < harvestedThingDef.ingestible.preferability));
				}
				return result;
			}

			internal bool <>m__2(Thing t)
			{
				return this.foodValidator(t) && !FoodUtility.filtered.Contains(t) && (t is Building_NutrientPasteDispenser || t.def.ingestible.preferability > FoodPreferability.DesperateOnly) && !t.IsNotFresh();
			}
		}
	}
}
