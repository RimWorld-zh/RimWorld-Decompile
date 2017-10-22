using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenPlantReproduction
	{
		private const float CavePlantsDensity = 0.5f;

		public static Plant TryReproduceFrom(IntVec3 source, ThingDef plantDef, SeedTargFindMode mode, Map map)
		{
			IntVec3 dest = default(IntVec3);
			return GenPlantReproduction.TryFindReproductionDestination(source, plantDef, mode, map, out dest) ? GenPlantReproduction.TryReproduceInto(dest, plantDef, map) : null;
		}

		public static Plant TryReproduceInto(IntVec3 dest, ThingDef plantDef, Map map)
		{
			return plantDef.CanEverPlantAt(dest, map) ? (GenPlant.SnowAllowsPlanting(dest, map) ? ((Plant)GenSpawn.Spawn(plantDef, dest, map)) : null) : null;
		}

		public static bool TryFindReproductionDestination(IntVec3 source, ThingDef plantDef, SeedTargFindMode mode, Map map, out IntVec3 foundCell)
		{
			float radius = -1f;
			switch (mode)
			{
			case SeedTargFindMode.Reproduce:
			{
				radius = plantDef.plant.reproduceRadius;
				break;
			}
			case SeedTargFindMode.MapGenCluster:
			{
				radius = plantDef.plant.WildClusterRadiusActual;
				break;
			}
			case SeedTargFindMode.MapEdge:
			{
				radius = 40f;
				break;
			}
			case SeedTargFindMode.Cave:
			{
				radius = plantDef.plant.WildClusterRadiusActual;
				break;
			}
			}
			int num = 0;
			int num2 = 0;
			float num3 = 0f;
			CellRect cellRect = CellRect.CenteredOn(source, Mathf.RoundToInt(radius));
			cellRect.ClipInsideMap(map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					Plant plant = c2.GetPlant(map);
					if (plant != null && (mode != SeedTargFindMode.Cave || plant.def.plant.cavePlant))
					{
						num++;
						if (plant.def == plantDef)
						{
							num2++;
						}
					}
					num3 += c2.GetTerrain(map).fertility;
				}
			}
			float num4 = (float)((mode != SeedTargFindMode.Cave) ? map.Biome.plantDensity : 0.5);
			float num5 = num3 * num4;
			bool flag = (float)num > num5;
			bool result;
			if ((float)num > num5 * 1.25)
			{
				foundCell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				if (mode != 0 && mode != SeedTargFindMode.Cave)
				{
					BiomeDef curBiome = map.Biome;
					float num6 = curBiome.AllWildPlants.Sum((Func<ThingDef, float>)((ThingDef pd) => curBiome.CommonalityOfPlant(pd)));
					float num7 = curBiome.CommonalityOfPlant(plantDef) / num6;
					float num8 = curBiome.CommonalityOfPlant(plantDef) * plantDef.plant.wildCommonalityMaxFraction / num6;
					float num9 = num5 * num8;
					if ((float)num2 > num9)
					{
						foundCell = IntVec3.Invalid;
						result = false;
						goto IL_02fa;
					}
					float num10 = num5 * num7;
					bool flag2 = (float)num2 < num10 * 0.5;
					if (flag && !flag2)
					{
						foundCell = IntVec3.Invalid;
						result = false;
						goto IL_02fa;
					}
				}
				Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 c) => (byte)(plantDef.CanEverPlantAt(c, map) ? ((!plantDef.plant.cavePlant || GenPlantReproduction.GoodRoofForCavePlantReproduction(c, map)) ? (GenPlant.SnowAllowsPlanting(c, map) ? (source.InHorDistOf(c, radius) ? (GenSight.LineOfSight(source, c, map, true, null, 0, 0) ? 1 : 0) : 0) : 0) : 0) : 0) != 0);
				result = CellFinder.TryFindRandomCellNear(source, map, Mathf.CeilToInt(radius), validator, out foundCell);
			}
			goto IL_02fa;
			IL_02fa:
			return result;
		}

		public static bool GoodRoofForCavePlantReproduction(IntVec3 c, Map map)
		{
			RoofDef roof = c.GetRoof(map);
			return roof != null && roof.isNatural;
		}
	}
}
