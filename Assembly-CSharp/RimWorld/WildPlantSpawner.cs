using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000454 RID: 1108
	public class WildPlantSpawner : IExposable
	{
		// Token: 0x04000BB4 RID: 2996
		private Map map;

		// Token: 0x04000BB5 RID: 2997
		private int cycleIndex;

		// Token: 0x04000BB6 RID: 2998
		private float calculatedWholeMapNumDesiredPlants;

		// Token: 0x04000BB7 RID: 2999
		private float calculatedWholeMapNumDesiredPlantsTmp;

		// Token: 0x04000BB8 RID: 3000
		private int calculatedWholeMapNumNonZeroFertilityCells;

		// Token: 0x04000BB9 RID: 3001
		private int calculatedWholeMapNumNonZeroFertilityCellsTmp;

		// Token: 0x04000BBA RID: 3002
		private bool hasWholeMapNumDesiredPlantsCalculated;

		// Token: 0x04000BBB RID: 3003
		private float? cachedCavePlantsCommonalitiesSum;

		// Token: 0x04000BBC RID: 3004
		private static List<ThingDef> allCavePlants = new List<ThingDef>();

		// Token: 0x04000BBD RID: 3005
		private static List<ThingDef> tmpPossiblePlants = new List<ThingDef>();

		// Token: 0x04000BBE RID: 3006
		private static List<KeyValuePair<ThingDef, float>> tmpPossiblePlantsWithWeight = new List<KeyValuePair<ThingDef, float>>();

		// Token: 0x04000BBF RID: 3007
		private static Dictionary<ThingDef, float> distanceSqToNearbyClusters = new Dictionary<ThingDef, float>();

		// Token: 0x04000BC0 RID: 3008
		private static Dictionary<ThingDef, List<float>> nearbyClusters = new Dictionary<ThingDef, List<float>>();

		// Token: 0x04000BC1 RID: 3009
		private static List<KeyValuePair<ThingDef, List<float>>> nearbyClustersList = new List<KeyValuePair<ThingDef, List<float>>>();

		// Token: 0x04000BC2 RID: 3010
		private const float CavePlantsDensityFactor = 0.5f;

		// Token: 0x04000BC3 RID: 3011
		private const int PlantSaturationScanRadius = 20;

		// Token: 0x04000BC4 RID: 3012
		private const float MapFractionCheckPerTick = 0.0001f;

		// Token: 0x04000BC5 RID: 3013
		private const float ChanceToRegrow = 0.012f;

		// Token: 0x04000BC6 RID: 3014
		private const float CavePlantChanceToRegrow = 0.0001f;

		// Token: 0x04000BC7 RID: 3015
		private const float BaseLowerOrderScanRadius = 7f;

		// Token: 0x04000BC8 RID: 3016
		private const float LowerOrderScanRadiusWildClusterRadiusFactor = 1.5f;

		// Token: 0x04000BC9 RID: 3017
		private const float MinDesiredLowerOrderPlantsToConsiderSkipping = 4f;

		// Token: 0x04000BCA RID: 3018
		private const float MinLowerOrderPlantsPct = 0.57f;

		// Token: 0x04000BCB RID: 3019
		private const float LocalPlantProportionsMaxScanRadius = 25f;

		// Token: 0x04000BCC RID: 3020
		private const float MaxLocalProportionsBias = 7f;

		// Token: 0x04000BCD RID: 3021
		private const float CavePlantRegrowDays = 130f;

		// Token: 0x04000BCE RID: 3022
		private static readonly SimpleCurve GlobalPctSelectionWeightBias = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.25f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			}
		};

		// Token: 0x04000BCF RID: 3023
		private static List<ThingDef> tmpPlantDefsLowerOrder = new List<ThingDef>();

		// Token: 0x06001349 RID: 4937 RVA: 0x000A5CA5 File Offset: 0x000A40A5
		public WildPlantSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x000A5CB8 File Offset: 0x000A40B8
		public float CurrentPlantDensity
		{
			get
			{
				return this.map.Biome.plantDensity * this.map.gameConditionManager.AggregatePlantDensityFactor(this.map);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x000A5CF4 File Offset: 0x000A40F4
		public float CurrentWholeMapNumDesiredPlants
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				float currentPlantDensity = this.CurrentPlantDensity;
				float num = 0f;
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					num += this.GetDesiredPlantsCountAt(iterator.Current, iterator.Current, currentPlantDensity);
					iterator.MoveNext();
				}
				return num;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x000A5D60 File Offset: 0x000A4160
		public int CurrentWholeMapNumNonZeroFertilityCells
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				int num = 0;
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.GetTerrain(this.map).fertility > 0f)
					{
						num++;
					}
					iterator.MoveNext();
				}
				return num;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x000A5DCC File Offset: 0x000A41CC
		public float CavePlantsCommonalitiesSum
		{
			get
			{
				float? num = this.cachedCavePlantsCommonalitiesSum;
				if (num == null)
				{
					this.cachedCavePlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
					{
						float? num2 = this.cachedCavePlantsCommonalitiesSum;
						this.cachedCavePlantsCommonalitiesSum = ((num2 == null) ? null : new float?(num2.GetValueOrDefault() + this.GetCommonalityOfPlant(WildPlantSpawner.allCavePlants[i])));
					}
				}
				return this.cachedCavePlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x000A5E75 File Offset: 0x000A4275
		public static void ResetStaticData()
		{
			WildPlantSpawner.allCavePlants.Clear();
			WildPlantSpawner.allCavePlants.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant
			select x);
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x000A5EB4 File Offset: 0x000A42B4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlants, "calculatedWholeMapNumDesiredPlants", 0f, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlantsTmp, "calculatedWholeMapNumDesiredPlantsTmp", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasWholeMapNumDesiredPlantsCalculated, "hasWholeMapNumDesiredPlantsCalculated", true, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCells, "calculatedWholeMapNumNonZeroFertilityCells", 0, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCellsTmp, "calculatedWholeMapNumNonZeroFertilityCellsTmp", 0, false);
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x000A5F38 File Offset: 0x000A4338
		public void WildPlantSpawnerTick()
		{
			if (DebugSettings.fastEcology || DebugSettings.fastEcologyRegrowRateOnly)
			{
				for (int i = 0; i < 2000; i++)
				{
					this.WildPlantSpawnerTickInternal();
				}
			}
			else
			{
				this.WildPlantSpawnerTickInternal();
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x000A5F88 File Offset: 0x000A4388
		private void WildPlantSpawnerTickInternal()
		{
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.0001f);
			float currentPlantDensity = this.CurrentPlantDensity;
			if (!this.hasWholeMapNumDesiredPlantsCalculated)
			{
				this.calculatedWholeMapNumDesiredPlants = this.CurrentWholeMapNumDesiredPlants;
				this.calculatedWholeMapNumNonZeroFertilityCells = this.CurrentWholeMapNumNonZeroFertilityCells;
				this.hasWholeMapNumDesiredPlantsCalculated = true;
			}
			int num2 = Mathf.CeilToInt(10000f);
			float chance = this.calculatedWholeMapNumDesiredPlants / (float)this.calculatedWholeMapNumNonZeroFertilityCells;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.calculatedWholeMapNumDesiredPlants = this.calculatedWholeMapNumDesiredPlantsTmp;
					this.calculatedWholeMapNumDesiredPlantsTmp = 0f;
					this.calculatedWholeMapNumNonZeroFertilityCells = this.calculatedWholeMapNumNonZeroFertilityCellsTmp;
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp = 0;
					this.cycleIndex = 0;
				}
				IntVec3 intVec = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.calculatedWholeMapNumDesiredPlantsTmp += this.GetDesiredPlantsCountAt(intVec, intVec, currentPlantDensity);
				if (intVec.GetTerrain(this.map).fertility > 0f)
				{
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp++;
				}
				float mtb = (!this.GoodRoofForCavePlant(intVec)) ? this.map.Biome.wildPlantRegrowDays : 130f;
				if (Rand.Chance(chance) && Rand.MTBEventOccurs(mtb, 60000f, (float)num2) && this.CanRegrowAt(intVec))
				{
					this.CheckSpawnWildPlantAt(intVec, currentPlantDensity, this.calculatedWholeMapNumDesiredPlants, false);
				}
				this.cycleIndex++;
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x000A6128 File Offset: 0x000A4528
		public bool CheckSpawnWildPlantAt(IntVec3 c, float plantDensity, float wholeMapNumDesiredPlants, bool setRandomGrowth = false)
		{
			bool result;
			if (plantDensity <= 0f || c.GetPlant(this.map) != null || c.GetCover(this.map) != null || c.GetEdifice(this.map) != null || this.map.fertilityGrid.FertilityAt(c) <= 0f || !GenPlant.SnowAllowsPlanting(c, this.map))
			{
				result = false;
			}
			else
			{
				bool cavePlants = this.GoodRoofForCavePlant(c);
				if (this.SaturatedAt(c, plantDensity, cavePlants, wholeMapNumDesiredPlants))
				{
					result = false;
				}
				else
				{
					this.CalculatePlantsWhichCanGrowAt(c, WildPlantSpawner.tmpPossiblePlants, cavePlants, plantDensity);
					if (!WildPlantSpawner.tmpPossiblePlants.Any<ThingDef>())
					{
						result = false;
					}
					else
					{
						this.CalculateDistancesToNearbyClusters(c);
						WildPlantSpawner.tmpPossiblePlantsWithWeight.Clear();
						for (int i = 0; i < WildPlantSpawner.tmpPossiblePlants.Count; i++)
						{
							float value = this.PlantChoiceWeight(WildPlantSpawner.tmpPossiblePlants[i], c, WildPlantSpawner.distanceSqToNearbyClusters, wholeMapNumDesiredPlants, plantDensity);
							WildPlantSpawner.tmpPossiblePlantsWithWeight.Add(new KeyValuePair<ThingDef, float>(WildPlantSpawner.tmpPossiblePlants[i], value));
						}
						KeyValuePair<ThingDef, float> keyValuePair;
						if (!WildPlantSpawner.tmpPossiblePlantsWithWeight.TryRandomElementByWeight((KeyValuePair<ThingDef, float> x) => x.Value, out keyValuePair))
						{
							result = false;
						}
						else
						{
							Plant plant = (Plant)ThingMaker.MakeThing(keyValuePair.Key, null);
							if (setRandomGrowth)
							{
								plant.Growth = Rand.Range(0.07f, 1f);
								if (plant.def.plant.LimitedLifespan)
								{
									plant.Age = Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 50, 0));
								}
							}
							GenSpawn.Spawn(plant, c, this.map, WipeMode.Vanish);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x000A630C File Offset: 0x000A470C
		private float PlantChoiceWeight(ThingDef plantDef, IntVec3 c, Dictionary<ThingDef, float> distanceSqToNearbyClusters, float wholeMapNumDesiredPlants, float plantDensity)
		{
			float commonalityOfPlant = this.GetCommonalityOfPlant(plantDef);
			float commonalityPctOfPlant = this.GetCommonalityPctOfPlant(plantDef);
			float num = commonalityOfPlant;
			float result;
			if (num <= 0f)
			{
				result = num;
			}
			else
			{
				float num2 = 0.5f;
				if ((float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count > wholeMapNumDesiredPlants / 2f && !plantDef.plant.cavePlant)
				{
					num2 = (float)this.map.listerThings.ThingsOfDef(plantDef).Count / (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count / commonalityPctOfPlant;
					num *= WildPlantSpawner.GlobalPctSelectionWeightBias.Evaluate(num2);
				}
				if (plantDef.plant.GrowsInClusters && num2 < 1.1f)
				{
					float num3 = (!plantDef.plant.cavePlant) ? this.map.Biome.PlantCommonalitiesSum : this.CavePlantsCommonalitiesSum;
					float x = commonalityOfPlant * plantDef.plant.wildClusterWeight / (num3 - commonalityOfPlant + commonalityOfPlant * plantDef.plant.wildClusterWeight);
					float num4 = 1f / (3.14159274f * (float)plantDef.plant.wildClusterRadius * (float)plantDef.plant.wildClusterRadius);
					num4 = GenMath.LerpDoubleClamped(commonalityPctOfPlant, 1f, 1f, num4, x);
					float f;
					if (distanceSqToNearbyClusters.TryGetValue(plantDef, out f))
					{
						float x2 = Mathf.Sqrt(f);
						num *= GenMath.LerpDoubleClamped((float)plantDef.plant.wildClusterRadius * 0.9f, (float)plantDef.plant.wildClusterRadius * 1.1f, plantDef.plant.wildClusterWeight, num4, x2);
					}
					else
					{
						num *= num4;
					}
				}
				if (plantDef.plant.wildEqualLocalDistribution)
				{
					float f2 = wholeMapNumDesiredPlants * commonalityPctOfPlant;
					float num5 = (float)Mathf.Max(this.map.Size.x, this.map.Size.z) / Mathf.Sqrt(f2);
					float num6 = num5 * 2f;
					if (plantDef.plant.GrowsInClusters)
					{
						num6 = Mathf.Max(num6, (float)plantDef.plant.wildClusterRadius * 1.6f);
					}
					num6 = Mathf.Max(num6, 7f);
					if (num6 <= 25f)
					{
						num *= this.LocalPlantProportionsWeightFactor(c, commonalityPctOfPlant, plantDensity, num6, plantDef);
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x000A6580 File Offset: 0x000A4980
		private float LocalPlantProportionsWeightFactor(IntVec3 c, float commonalityPct, float plantDensity, float radiusToScan, ThingDef plantDef)
		{
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			int numPlantsThisDef = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.Plant).Count;
				numPlantsThisDef += reg.ListerThings.ThingsOfDef(plantDef).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			float num = numDesiredPlantsLocally * commonalityPct;
			float result;
			if (num < 2f)
			{
				result = 1f;
			}
			else if ((float)numPlants <= numDesiredPlantsLocally * 0.5f)
			{
				result = 1f;
			}
			else
			{
				float t = (float)numPlantsThisDef / (float)numPlants / commonalityPct;
				result = Mathf.Lerp(7f, 1f, t);
			}
			return result;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000A666C File Offset: 0x000A4A6C
		private void CalculatePlantsWhichCanGrowAt(IntVec3 c, List<ThingDef> outPlants, bool cavePlants, float plantDensity)
		{
			outPlants.Clear();
			if (cavePlants)
			{
				for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
				{
					if (WildPlantSpawner.allCavePlants[i].CanEverPlantAt(c, this.map))
					{
						outPlants.Add(WildPlantSpawner.allCavePlants[i]);
					}
				}
			}
			else
			{
				List<ThingDef> allWildPlants = this.map.Biome.AllWildPlants;
				for (int j = 0; j < allWildPlants.Count; j++)
				{
					ThingDef thingDef = allWildPlants[j];
					if (thingDef.CanEverPlantAt(c, this.map))
					{
						if (thingDef.plant.wildOrder != this.map.Biome.LowestWildAndCavePlantOrder)
						{
							float num = 7f;
							if (thingDef.plant.GrowsInClusters)
							{
								num = Math.Max(num, (float)thingDef.plant.wildClusterRadius * 1.5f);
							}
							if (!this.EnoughLowerOrderPlantsNearby(c, plantDensity, num, thingDef))
							{
								goto IL_109;
							}
						}
						outPlants.Add(thingDef);
					}
					IL_109:;
				}
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x000A6794 File Offset: 0x000A4B94
		private bool EnoughLowerOrderPlantsNearby(IntVec3 c, float plantDensity, float radiusToScan, ThingDef plantDef)
		{
			float num = 0f;
			WildPlantSpawner.tmpPlantDefsLowerOrder.Clear();
			List<ThingDef> allWildPlants = this.map.Biome.AllWildPlants;
			for (int i = 0; i < allWildPlants.Count; i++)
			{
				if (allWildPlants[i].plant.wildOrder < plantDef.plant.wildOrder)
				{
					num += this.GetCommonalityPctOfPlant(allWildPlants[i]);
					WildPlantSpawner.tmpPlantDefsLowerOrder.Add(allWildPlants[i]);
				}
			}
			float numDesiredPlantsLocally = 0f;
			int numPlantsLowerOrder = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				for (int j = 0; j < WildPlantSpawner.tmpPlantDefsLowerOrder.Count; j++)
				{
					numPlantsLowerOrder += reg.ListerThings.ThingsOfDef(WildPlantSpawner.tmpPlantDefsLowerOrder[j]).Count;
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			float num2 = numDesiredPlantsLocally * num;
			return num2 < 4f || (float)numPlantsLowerOrder / num2 >= 0.57f;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x000A68C0 File Offset: 0x000A4CC0
		private bool SaturatedAt(IntVec3 c, float plantDensity, bool cavePlants, float wholeMapNumDesiredPlants)
		{
			int num = GenRadial.NumCellsInRadius(20f);
			float num2 = wholeMapNumDesiredPlants * ((float)num / (float)this.map.Area);
			bool result;
			if (num2 <= 4f || !this.map.Biome.wildPlantsCareAboutLocalFertility)
			{
				result = ((float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count >= wholeMapNumDesiredPlants);
			}
			else
			{
				float numDesiredPlantsLocally = 0f;
				int numPlants = 0;
				RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), 20f), delegate(Region reg)
				{
					numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
					numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.Plant).Count;
					return false;
				}, 999999, RegionType.Set_Passable);
				result = ((float)numPlants >= numDesiredPlantsLocally);
			}
			return result;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000A69A8 File Offset: 0x000A4DA8
		private void CalculateDistancesToNearbyClusters(IntVec3 c)
		{
			WildPlantSpawner.nearbyClusters.Clear();
			WildPlantSpawner.nearbyClustersList.Clear();
			int num = GenRadial.NumCellsInRadius((float)(this.map.Biome.MaxWildAndCavePlantsClusterRadius * 2));
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map))
				{
					List<Thing> list = this.map.thingGrid.ThingsListAtFast(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.category == ThingCategory.Plant && thing.def.plant.GrowsInClusters)
						{
							float item = (float)intVec.DistanceToSquared(c);
							List<float> list2;
							if (!WildPlantSpawner.nearbyClusters.TryGetValue(thing.def, out list2))
							{
								list2 = SimplePool<List<float>>.Get();
								WildPlantSpawner.nearbyClusters.Add(thing.def, list2);
								WildPlantSpawner.nearbyClustersList.Add(new KeyValuePair<ThingDef, List<float>>(thing.def, list2));
							}
							list2.Add(item);
						}
					}
				}
			}
			WildPlantSpawner.distanceSqToNearbyClusters.Clear();
			for (int k = 0; k < WildPlantSpawner.nearbyClustersList.Count; k++)
			{
				List<float> value = WildPlantSpawner.nearbyClustersList[k].Value;
				value.Sort();
				WildPlantSpawner.distanceSqToNearbyClusters.Add(WildPlantSpawner.nearbyClustersList[k].Key, value[value.Count / 2]);
				value.Clear();
				SimplePool<List<float>>.Return(value);
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000A6B70 File Offset: 0x000A4F70
		private bool CanRegrowAt(IntVec3 c)
		{
			return c.GetTemperature(this.map) > 0f && (!c.Roofed(this.map) || this.GoodRoofForCavePlant(c));
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x000A6BBC File Offset: 0x000A4FBC
		private bool GoodRoofForCavePlant(IntVec3 c)
		{
			RoofDef roof = c.GetRoof(this.map);
			return roof != null && roof.isNatural;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x000A6BF0 File Offset: 0x000A4FF0
		private float GetCommonalityOfPlant(ThingDef plant)
		{
			return (!plant.plant.cavePlant) ? this.map.Biome.CommonalityOfPlant(plant) : plant.plant.cavePlantWeight;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000A6C38 File Offset: 0x000A5038
		private float GetCommonalityPctOfPlant(ThingDef plant)
		{
			return (!plant.plant.cavePlant) ? this.map.Biome.CommonalityPctOfPlant(plant) : (this.GetCommonalityOfPlant(plant) / this.CavePlantsCommonalitiesSum);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000A6C84 File Offset: 0x000A5084
		public float GetBaseDesiredPlantsCountAt(IntVec3 c)
		{
			float num = c.GetTerrain(this.map).fertility;
			if (this.GoodRoofForCavePlant(c))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000A6CC0 File Offset: 0x000A50C0
		public float GetDesiredPlantsCountAt(IntVec3 c, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(this.GetBaseDesiredPlantsCountAt(c) * plantDensity * forCell.GetTerrain(this.map).fertility, 1f);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000A6CFC File Offset: 0x000A50FC
		public float GetDesiredPlantsCountIn(Region reg, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(reg.GetBaseDesiredPlantsCount(true) * plantDensity * forCell.GetTerrain(this.map).fertility, (float)reg.CellCount);
		}
	}
}
