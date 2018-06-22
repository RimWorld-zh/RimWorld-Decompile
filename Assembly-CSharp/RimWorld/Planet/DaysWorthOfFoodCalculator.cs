using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F0 RID: 1520
	public static class DaysWorthOfFoodCalculator
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x001052A0 File Offset: 0x001036A0
		private static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<ThingDefCount> extraFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3500, bool assumeCaravanMoving = true)
		{
			float result;
			if (!DaysWorthOfFoodCalculator.AnyFoodEatingPawn(pawns))
			{
				result = 600f;
			}
			else
			{
				if (!assumeCaravanMoving)
				{
					path = null;
				}
				DaysWorthOfFoodCalculator.tmpFood.Clear();
				if (extraFood != null)
				{
					int i = 0;
					int count = extraFood.Count;
					while (i < count)
					{
						ThingDefCount item = extraFood[i];
						if (item.ThingDef.IsNutritionGivingIngestible && item.Count > 0)
						{
							DaysWorthOfFoodCalculator.tmpFood.Add(item);
						}
						i++;
					}
				}
				int j = 0;
				int count2 = pawns.Count;
				while (j < count2)
				{
					Pawn pawn = pawns[j];
					if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
					{
						ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
						int k = 0;
						int count3 = innerContainer.Count;
						while (k < count3)
						{
							Thing thing = innerContainer[k];
							if (thing.def.IsNutritionGivingIngestible)
							{
								DaysWorthOfFoodCalculator.tmpFood.Add(new ThingDefCount(thing.def, thing.stackCount));
							}
							k++;
						}
					}
					j++;
				}
				DaysWorthOfFoodCalculator.tmpFood2.Clear();
				DaysWorthOfFoodCalculator.tmpFood2.AddRange(DaysWorthOfFoodCalculator.tmpFood);
				DaysWorthOfFoodCalculator.tmpFood.Clear();
				int l = 0;
				int count4 = DaysWorthOfFoodCalculator.tmpFood2.Count;
				while (l < count4)
				{
					ThingDefCount item2 = DaysWorthOfFoodCalculator.tmpFood2[l];
					bool flag = false;
					int m = 0;
					int count5 = DaysWorthOfFoodCalculator.tmpFood.Count;
					while (m < count5)
					{
						ThingDefCount thingDefCount = DaysWorthOfFoodCalculator.tmpFood[m];
						if (thingDefCount.ThingDef == item2.ThingDef)
						{
							DaysWorthOfFoodCalculator.tmpFood[m] = thingDefCount.WithCount(thingDefCount.Count + item2.Count);
							flag = true;
							break;
						}
						m++;
					}
					if (!flag)
					{
						DaysWorthOfFoodCalculator.tmpFood.Add(item2);
					}
					l++;
				}
				DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn.Clear();
				int n = 0;
				int count6 = pawns.Count;
				while (n < count6)
				{
					DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn.Add(0f);
					n++;
				}
				int ticksAbs = Find.TickManager.TicksAbs;
				DaysWorthOfFoodCalculator.tmpTicksToArrive.Clear();
				if (path != null && path.Found)
				{
					CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(tile, path.LastNode, path, nextTileCostLeft, caravanTicksPerMove, ticksAbs, DaysWorthOfFoodCalculator.tmpTicksToArrive);
				}
				DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Clear();
				DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Clear();
				DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Clear();
				int num = 0;
				int count7 = pawns.Count;
				while (num < count7)
				{
					Pawn pawn2 = pawns[num];
					Need_Food food = pawn2.needs.food;
					DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Add(food.NutritionBetweenHungryAndFed);
					DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Add(food.TicksUntilHungryWhenFed);
					DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Add(food.MaxLevel);
					num++;
				}
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				bool flag2 = false;
				WorldGrid worldGrid = Find.WorldGrid;
				bool flag3;
				do
				{
					flag3 = false;
					int num5 = ticksAbs + (int)(num3 * 60000f);
					int num6 = (path == null) ? tile : CaravanArrivalTimeEstimator.TileIllBeInAt(num5, DaysWorthOfFoodCalculator.tmpTicksToArrive, ticksAbs);
					bool flag4 = CaravanRestUtility.WouldBeRestingAt(num6, (long)num5);
					float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(assumeCaravanMoving && !flag4, flag4, null);
					float num7 = 1f / progressPerTick;
					bool flag5 = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(num6, num5);
					float num8 = num3 - num2;
					if (num8 > 0f)
					{
						num4 += num8 * 60000f;
						if (num4 >= num7)
						{
							BiomeDef biome = worldGrid[num6].biome;
							int num9 = Mathf.RoundToInt(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, biome, faction, null));
							ThingDef foragedFood = biome.foragedFood;
							while (num4 >= num7)
							{
								num4 -= num7;
								if (num9 > 0)
								{
									bool flag6 = false;
									for (int num10 = DaysWorthOfFoodCalculator.tmpFood.Count - 1; num10 >= 0; num10--)
									{
										ThingDefCount thingDefCount2 = DaysWorthOfFoodCalculator.tmpFood[num10];
										if (thingDefCount2.ThingDef == foragedFood)
										{
											DaysWorthOfFoodCalculator.tmpFood[num10] = thingDefCount2.WithCount(thingDefCount2.Count + num9);
											flag6 = true;
											break;
										}
									}
									if (!flag6)
									{
										DaysWorthOfFoodCalculator.tmpFood.Add(new ThingDefCount(foragedFood, num9));
									}
								}
							}
						}
					}
					num2 = num3;
					int num11 = 0;
					int count8 = pawns.Count;
					while (num11 < count8)
					{
						Pawn pawn3 = pawns[num11];
						if (pawn3.RaceProps.EatsFood)
						{
							if (flag5 && VirtualPlantsUtility.CanEverEatVirtualPlants(pawn3))
							{
								if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] < num3)
								{
									DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] = num3;
								}
								else
								{
									List<float> list;
									int index;
									(list = DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn)[index = num11] = list[index] + 0.45f;
								}
								flag3 = true;
							}
							else
							{
								float num12 = DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed[num11];
								int num13 = DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed[num11];
								do
								{
									int num14 = DaysWorthOfFoodCalculator.BestEverEdibleFoodIndexFor(pawn3, DaysWorthOfFoodCalculator.tmpFood);
									if (num14 < 0)
									{
										goto Block_27;
									}
									ThingDefCount thingDefCount3 = DaysWorthOfFoodCalculator.tmpFood[num14];
									float num15 = Mathf.Min(thingDefCount3.ThingDef.ingestible.CachedNutrition, num12);
									float num16 = num15 / num12 * (float)num13 / 60000f;
									int num17 = Mathf.Min(Mathf.CeilToInt(Mathf.Min(0.2f, DaysWorthOfFoodCalculator.cachedMaxFoodLevel[num11]) / num15), thingDefCount3.Count);
									List<float> list;
									int index2;
									(list = DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn)[index2 = num11] = list[index2] + num16 * (float)num17;
									DaysWorthOfFoodCalculator.tmpFood[num14] = thingDefCount3.WithCount(thingDefCount3.Count - num17);
									flag3 = true;
								}
								while (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] < num3);
								goto IL_633;
								Block_27:
								if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11] < num3)
								{
									flag2 = true;
								}
							}
							IL_633:
							if (flag2)
							{
								break;
							}
							num3 = Mathf.Max(num3, DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num11]);
						}
						num11++;
					}
				}
				while (flag3 && !flag2 && num3 <= 601f);
				float num18 = 600f;
				int num19 = 0;
				int count9 = pawns.Count;
				while (num19 < count9)
				{
					if (pawns[num19].RaceProps.EatsFood)
					{
						num18 = Mathf.Min(num18, DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num19]);
					}
					num19++;
				}
				result = num18;
			}
			return result;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00105998 File Offset: 0x00103D98
		public static float ApproxDaysWorthOfFood(Caravan caravan)
		{
			return DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(caravan.PawnsListForReading, null, caravan.Tile, IgnorePawnsInventoryMode.DontIgnore, caravan.Faction, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, caravan.pather.Moving && !caravan.pather.Paused);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00105A04 File Offset: 0x00103E04
		public static float ApproxDaysWorthOfFood(List<TransferableOneWay> transferables, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3500)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (pawn.RaceProps.EatsFood)
							{
								DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
							}
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(transferableOneWay.ThingDef, transferableOneWay.CountToTransfer));
					}
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, path, nextTileCostLeft, caravanTicksPerMove, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x00105B10 File Offset: 0x00103F10
		public static float ApproxDaysWorthOfFoodLeftAfterTransfer(List<TransferableOneWay> transferables, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3500)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (pawn.RaceProps.EatsFood)
							{
								DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
							}
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(transferableOneWay.ThingDef, transferableOneWay.MaxCount - transferableOneWay.CountToTransfer));
					}
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, path, nextTileCostLeft, caravanTicksPerMove, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x00105C30 File Offset: 0x00104030
		public static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<Thing> potentiallyFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.RaceProps.EatsFood)
				{
					DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
				}
			}
			for (int j = 0; j < potentiallyFood.Count; j++)
			{
				DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(potentiallyFood[j].def, potentiallyFood[j].stackCount));
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3500, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x00105D14 File Offset: 0x00104114
		public static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<ThingCount> potentiallyFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			for (int i = 0; i < potentiallyFood.Count; i++)
			{
				if (potentiallyFood[i].Count > 0)
				{
					DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(potentiallyFood[i].Thing.def, potentiallyFood[i].Count));
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(pawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3500, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			return result;
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00105DC4 File Offset: 0x001041C4
		public static float ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, DaysWorthOfFoodCalculator.tmpThingCounts);
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			for (int i = DaysWorthOfFoodCalculator.tmpThingCounts.Count - 1; i >= 0; i--)
			{
				if (DaysWorthOfFoodCalculator.tmpThingCounts[i].Count > 0)
				{
					Pawn pawn = DaysWorthOfFoodCalculator.tmpThingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.EatsFood)
						{
							DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(DaysWorthOfFoodCalculator.tmpThingCounts[i].Thing.def, DaysWorthOfFoodCalculator.tmpThingCounts[i].Count));
					}
				}
			}
			DaysWorthOfFoodCalculator.tmpThingCounts.Clear();
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3500, true);
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			return result;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x00105F08 File Offset: 0x00104308
		private static bool AnyFoodEatingPawn(List<Pawn> pawns)
		{
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				if (pawns[i].RaceProps.EatsFood)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x00105F58 File Offset: 0x00104358
		private static int BestEverEdibleFoodIndexFor(Pawn pawn, List<ThingDefCount> food)
		{
			int num = -1;
			float num2 = 0f;
			int i = 0;
			int count = food.Count;
			while (i < count)
			{
				if (food[i].Count > 0)
				{
					ThingDef thingDef = food[i].ThingDef;
					if (CaravanPawnsNeedsUtility.CanEverEatForNutrition(thingDef, pawn))
					{
						float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thingDef, pawn, thingDef.ingestible.CachedNutrition);
						if (num < 0 || foodScore > num2)
						{
							num = i;
							num2 = foodScore;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x040011F3 RID: 4595
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040011F4 RID: 4596
		private static List<ThingDefCount> tmpThingDefCounts = new List<ThingDefCount>();

		// Token: 0x040011F5 RID: 4597
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040011F6 RID: 4598
		public const float InfiniteDaysWorthOfFood = 600f;

		// Token: 0x040011F7 RID: 4599
		private static List<float> tmpDaysWorthOfFoodForPawn = new List<float>();

		// Token: 0x040011F8 RID: 4600
		private static List<ThingDefCount> tmpFood = new List<ThingDefCount>();

		// Token: 0x040011F9 RID: 4601
		private static List<ThingDefCount> tmpFood2 = new List<ThingDefCount>();

		// Token: 0x040011FA RID: 4602
		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();

		// Token: 0x040011FB RID: 4603
		private static List<float> cachedNutritionBetweenHungryAndFed = new List<float>();

		// Token: 0x040011FC RID: 4604
		private static List<int> cachedTicksUntilHungryWhenFed = new List<int>();

		// Token: 0x040011FD RID: 4605
		private static List<float> cachedMaxFoodLevel = new List<float>();
	}
}
