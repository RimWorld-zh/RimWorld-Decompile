#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
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

		public static bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false)
		{
			Profiler.BeginSample("TryFindBestFoodSourceFor");
			bool flag = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			bool allowDrug = !eater.IsTeetotaler();
			Thing thing = null;
			bool result;
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
						result = true;
						goto IL_028e;
					}
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
					{
						Profiler.EndSample();
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						result = true;
						goto IL_028e;
					}
				}
			}
			bool allowPlant = getter == eater;
			ThingDef thingDef = default(ThingDef);
			Thing thing2 = FoodUtility.BestFoodSourceOnMap(getter, eater, desperate, out thingDef, FoodPreferability.MealLavish, allowPlant, allowDrug, allowCorpse, true, canRefillDispenser, allowForbidden, allowSociallyImproper, allowHarvest);
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
						num2 = (float)(num2 - 32.0);
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
						result = true;
						goto IL_028e;
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
						result = true;
						goto IL_028e;
					}
				}
				Profiler.EndSample();
				foodSource = null;
				foodDef = null;
				result = false;
			}
			goto IL_028e;
			IL_028e:
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
							result = plant.def.plant.harvestedThingDef;
							goto IL_0095;
						}
					}
					result = foodSource.def;
				}
			}
			goto IL_0095;
			IL_0095:
			return result;
		}

		public static Thing BestFoodInInventory(Pawn holder, Pawn eater = null, FoodPreferability minFoodPref = FoodPreferability.NeverForNutrition, FoodPreferability maxFoodPref = FoodPreferability.MealLavish, float minStackNutrition = 0f, bool allowDrug = false)
		{
			Thing result;
			Thing thing;
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
					thing = innerContainer[i];
					if (thing.def.IsNutritionGivingIngestible && thing.IngestibleNow && eater.RaceProps.CanEverEat(thing) && (int)thing.def.ingestible.preferability >= (int)minFoodPref && (int)thing.def.ingestible.preferability <= (int)maxFoodPref && (allowDrug || !thing.def.IsDrug))
					{
						float num = thing.def.ingestible.nutrition * (float)thing.stackCount;
						if (num >= minStackNutrition)
							goto IL_00cb;
					}
				}
				result = null;
			}
			goto IL_00eb;
			IL_00cb:
			result = thing;
			goto IL_00eb;
			IL_00eb:
			return result;
		}

		public static Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false)
		{
			foodDef = null;
			Profiler.BeginSample("BestFoodInWorldFor getter=" + getter.LabelCap + " eater=" + eater.LabelCap);
			bool getterCanManipulate = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			Thing result;
			if (!getterCanManipulate && getter != eater)
			{
				Log.Error(getter + " tried to find food to bring to " + eater + " but " + getter + " is incapable of Manipulation.");
				Profiler.EndSample();
				result = null;
			}
			else
			{
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
				Predicate<Thing> foodValidator = (Predicate<Thing>)delegate(Thing t)
				{
					Profiler.BeginSample("foodValidator");
					bool result3;
					if (!allowForbidden && t.IsForbidden(getter))
					{
						Profiler.EndSample();
						result3 = false;
					}
					else
					{
						Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
						if (building_NutrientPasteDispenser != null)
						{
							if (allowDispenserFull && (int)ThingDefOf.MealNutrientPaste.ingestible.preferability >= (int)minPref && (int)ThingDefOf.MealNutrientPaste.ingestible.preferability <= (int)maxPref && getterCanManipulate && !getter.IsWildMan() && (t.Faction == getter.Faction || t.Faction == getter.HostFaction) && building_NutrientPasteDispenser.powerComp.PowerOn && (allowDispenserEmpty || building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers()) && FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) && t.InteractionCell.Standable(t.Map) && getter.Map.reachability.CanReachNonLocal(getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(getter, Danger.Some, TraverseMode.ByPawn, false)))
							{
								goto IL_02a4;
							}
							Profiler.EndSample();
							result3 = false;
						}
						else if ((int)t.def.ingestible.preferability < (int)minPref)
						{
							Profiler.EndSample();
							result3 = false;
						}
						else if ((int)t.def.ingestible.preferability > (int)maxPref)
						{
							Profiler.EndSample();
							result3 = false;
						}
						else
						{
							if (t.IngestibleNow && t.def.IsNutritionGivingIngestible && (allowCorpse || !(t is Corpse)) && (allowDrug || !t.def.IsDrug) && (desperate || !t.IsNotFresh()) && !t.IsDessicated() && eater.RaceProps.WillAutomaticallyEat(t) && FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) && getter.AnimalAwareOf(t) && getter.CanReserve(t, 1, -1, null, false))
							{
								goto IL_02a4;
							}
							Profiler.EndSample();
							result3 = false;
						}
					}
					goto IL_02b0;
					IL_02b0:
					return result3;
					IL_02a4:
					Profiler.EndSample();
					result3 = true;
					goto IL_02b0;
				};
				ThingRequest thingRequest = (((int)eater.RaceProps.foodType & 192) == 0 || !allowPlant) ? ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree) : ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
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
						Thing thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), PathEndMode.Touch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)delegate(Thing x)
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
								result2 = ((byte)(harvestedThingDef.IsNutritionGivingIngestible ? (getter.CanReserve((Thing)plant, 1, -1, null, false) ? ((allowForbidden || !plant.IsForbidden(getter)) ? ((bestThing == null || (int)FoodUtility.GetFinalIngestibleDef(bestThing, false).ingestible.preferability < (int)harvestedThingDef.ingestible.preferability) ? 1 : 0) : 0) : 0) : 0) != 0);
							}
							return result2;
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
					Predicate<Thing> predicate = (Predicate<Thing>)((Thing t) => (byte)(foodValidator(t) ? ((!FoodUtility.filtered.Contains(t)) ? ((t is Building_NutrientPasteDispenser || (int)t.def.ingestible.preferability > 2) ? ((!t.IsNotFresh()) ? 1 : 0) : 0) : 0) : 0) != 0);
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
				Profiler.EndSample();
				result = bestThing;
			}
			return result;
		}

		private static bool IsFoodSourceOnMapSociallyProper(Thing t, Pawn getter, Pawn eater, bool allowSociallyImproper)
		{
			bool result;
			if (!allowSociallyImproper)
			{
				bool animalsCare = !getter.RaceProps.Animal;
				if (!t.IsSociallyProper(getter) && !t.IsSociallyProper(eater, eater.IsPrisonerOfColony, animalsCare))
				{
					result = false;
					goto IL_0045;
				}
			}
			result = true;
			goto IL_0045;
			IL_0045:
			return result;
		}

		public static float FoodOptimality(Pawn eater, Thing foodSource, ThingDef foodDef, float dist, bool takingToInventory = false)
		{
			float num = 300f;
			num -= dist;
			float result;
			switch (foodDef.ingestible.preferability)
			{
			case FoodPreferability.NeverForNutrition:
			{
				result = -9999999f;
				goto IL_0178;
			}
			case FoodPreferability.DesperateOnly:
			{
				num = (float)(num - 150.0);
				break;
			}
			case FoodPreferability.DesperateOnlyForHumanlikes:
			{
				if (eater.RaceProps.Humanlike)
				{
					num = (float)(num - 150.0);
				}
				break;
			}
			}
			CompRottable compRottable = foodSource.TryGetComp<CompRottable>();
			if (compRottable != null)
			{
				if (compRottable.Stage == RotStage.Dessicated)
				{
					result = -9999999f;
					goto IL_0178;
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
			result = num;
			goto IL_0178;
			IL_0178:
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
				float num3 = 0f;
				float num4 = -3.40282347E+38f;
				for (int i = 0; i < searchSet.Count; i++)
				{
					Thing thing2 = searchSet[i];
					num2++;
					float num5 = (float)(root - thing2.Position).LengthManhattan;
					if (!(num5 > maxDistance))
					{
						num3 = FoodUtility.FoodOptimality(eater, thing2, FoodUtility.GetFinalIngestibleDef(thing2, false), num5, false);
						if (!(num3 < num4) && pawn.Map.reachability.CanReach(root, thing2, peMode, traverseParams) && thing2.Spawned && ((object)validator == null || validator(thing2)))
						{
							thing = thing2;
							num4 = num3;
							num++;
						}
					}
				}
				Profiler.BeginSample("changedCount: " + num + " scanCount: " + num2);
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
			Pawn result;
			if (predator.meleeVerbs.TryGetMeleeVerb() == null)
			{
				result = null;
			}
			else
			{
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
					if (predator.GetRoom(RegionType.Set_Passable) == pawn2.GetRoom(RegionType.Set_Passable) && predator != pawn2 && (!flag || pawn2.Downed) && FoodUtility.IsAcceptablePreyFor(predator, pawn2) && predator.CanReach((Thing)pawn2, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) && !pawn2.IsForbidden(predator) && (!tutorialMode || pawn2.Faction != Faction.OfPlayer))
					{
						float preyScoreFor = FoodUtility.GetPreyScoreFor(predator, pawn2);
						if (preyScoreFor > num || pawn == null)
						{
							num = preyScoreFor;
							pawn = pawn2;
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
			else if (prey.BodySize > predator.RaceProps.maxPreyBodySize)
			{
				result = false;
			}
			else
			{
				if (!prey.Downed)
				{
					if (prey.kindDef.combatPower > 2.0 * predator.kindDef.combatPower)
					{
						result = false;
						goto IL_0170;
					}
					float num = prey.kindDef.combatPower * prey.health.summaryHealth.SummaryHealthPercent * prey.ageTracker.CurLifeStage.bodySizeFactor;
					float num2 = predator.kindDef.combatPower * predator.health.summaryHealth.SummaryHealthPercent * predator.ageTracker.CurLifeStage.bodySizeFactor;
					if (num > 0.85000002384185791 * num2)
					{
						result = false;
						goto IL_0170;
					}
				}
				result = ((byte)((predator.Faction == null || prey.Faction == null || predator.HostileTo(prey)) ? ((predator.Faction != Faction.OfPlayer || prey.Faction != Faction.OfPlayer) ? ((!predator.RaceProps.herdAnimal || predator.def != prey.def) ? 1 : 0) : 0) : 0) != 0);
			}
			goto IL_0170;
			IL_0170:
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
			List<ThoughtDef> result;
			if (ingester.needs == null || ingester.needs.mood == null)
			{
				result = FoodUtility.ingestThoughts;
			}
			else
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
			return (byte)((def.ingestible.sourceDef != null && def.ingestible.sourceDef.race != null && def.ingestible.sourceDef.race.Humanlike) ? 1 : 0) != 0;
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
				result = ((byte)((corpse != null && corpse.InnerPawn.RaceProps.Humanlike) ? 1 : 0) != 0);
			}
			return result;
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
			return (float)(pawn.RaceProps.IsFlesh ? (5.1999998092651367 * pawn.BodySize * pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(part)) : 0.0);
		}

		public static int StackCountForNutrition(ThingDef def, float nutrition)
		{
			return (!(nutrition <= 9.9999997473787516E-05)) ? Mathf.Max(Mathf.RoundToInt(nutrition / def.ingestible.nutrition), 1) : 0;
		}

		public static bool ShouldBeFedBySomeone(Pawn pawn)
		{
			return FeedPatientUtility.ShouldBeFed(pawn) || WardenFeedUtility.ShouldBeFed(pawn);
		}

		public static void AddFoodPoisoningHediff(Pawn pawn, Thing ingestible)
		{
			pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.FoodPoisoning, pawn, null), null, default(DamageInfo?));
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message("MessageFoodPoisoning".Translate(pawn.LabelShort, ingestible.LabelCapNoCount).CapitalizeFirst(), (Thing)pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
	}
}
