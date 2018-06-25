using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B0 RID: 1456
	public static class VirtualPlantsUtility
	{
		// Token: 0x04001097 RID: 4247
		private static readonly FloatRange VirtualPlantNutritionRandomFactor = new FloatRange(0.7f, 1f);

		// Token: 0x06001BDB RID: 7131 RVA: 0x000F00B4 File Offset: 0x000EE4B4
		public static bool CanEverEatVirtualPlants(Pawn p)
		{
			return p.RaceProps.Eats(FoodTypeFlags.Plant);
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000F00D8 File Offset: 0x000EE4D8
		public static bool CanEatVirtualPlantsNow(Pawn p)
		{
			return VirtualPlantsUtility.CanEatVirtualPlants(p, GenTicks.TicksAbs);
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x000F00F8 File Offset: 0x000EE4F8
		public static bool CanEatVirtualPlants(Pawn p, int ticksAbs)
		{
			return p.Tile >= 0 && !p.Dead && p.IsWorldPawn() && VirtualPlantsUtility.CanEverEatVirtualPlants(p) && VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(p.Tile, ticksAbs);
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000F014C File Offset: 0x000EE54C
		public static bool EnvironmentAllowsEatingVirtualPlantsNowAt(int tile)
		{
			return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, GenTicks.TicksAbs);
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000F016C File Offset: 0x000EE56C
		public static bool EnvironmentAllowsEatingVirtualPlantsAt(int tile, int ticksAbs)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			bool result;
			if (!biome.hasVirtualPlants)
			{
				result = false;
			}
			else
			{
				float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
				result = (temperatureFromSeasonAtTile >= 0f);
			}
			return result;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000F01C0 File Offset: 0x000EE5C0
		public static void EatVirtualPlants(Pawn p)
		{
			float num = ThingDefOf.Plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) * VirtualPlantsUtility.VirtualPlantNutritionRandomFactor.RandomInRange;
			p.needs.food.CurLevel += num;
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000F0208 File Offset: 0x000EE608
		public static string GetVirtualPlantsStatusExplanationAt(int tile, int ticksAbs)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ticksAbs == GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsCanGrazeNow".Translate());
			}
			else if (ticksAbs > GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsWillBeAbleToGraze".Translate());
			}
			else
			{
				stringBuilder.Append("AnimalsCanGraze".Translate());
			}
			stringBuilder.Append(": ");
			bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs);
			stringBuilder.Append((!flag) ? "No".Translate() : "Yes".Translate());
			if (flag)
			{
				float? approxDaysUntilPossibleToGraze = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, true);
				if (approxDaysUntilPossibleToGraze != null)
				{
					stringBuilder.Append("\n" + "PossibleToGrazeFor".Translate(new object[]
					{
						approxDaysUntilPossibleToGraze.Value.ToString("0.#")
					}));
				}
				else
				{
					stringBuilder.Append("\n" + "PossibleToGrazeForever".Translate());
				}
			}
			else
			{
				if (!Find.WorldGrid[tile].biome.hasVirtualPlants)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfBiome".Translate(new object[]
					{
						Find.WorldGrid[tile].biome.label
					}));
				}
				float? approxDaysUntilPossibleToGraze2 = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, false);
				if (approxDaysUntilPossibleToGraze2 != null)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfTemp".Translate(new object[]
					{
						approxDaysUntilPossibleToGraze2.Value.ToString("0.#")
					}));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000F03D8 File Offset: 0x000EE7D8
		public static float? GetApproxDaysUntilPossibleToGraze(int tile, int ticksAbs, bool untilNoLongerPossibleToGraze = false)
		{
			float? result;
			if (!untilNoLongerPossibleToGraze && !Find.WorldGrid[tile].biome.hasVirtualPlants)
			{
				result = null;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < Mathf.CeilToInt(133.333344f); i++)
				{
					bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs + (int)(num * 60000f));
					if ((!untilNoLongerPossibleToGraze && flag) || (untilNoLongerPossibleToGraze && !flag))
					{
						return new float?(num);
					}
					num += 0.45f;
				}
				result = null;
			}
			return result;
		}
	}
}
