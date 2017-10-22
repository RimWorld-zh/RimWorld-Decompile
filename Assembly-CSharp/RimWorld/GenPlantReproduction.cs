using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenPlantReproduction
	{
		public static Plant TryReproduceFrom(IntVec3 source, ThingDef plantDef, SeedTargFindMode mode, Map map)
		{
			IntVec3 dest = default(IntVec3);
			if (!GenPlantReproduction.TryFindReproductionDestination(source, plantDef, mode, map, out dest))
			{
				return null;
			}
			return GenPlantReproduction.TryReproduceInto(dest, plantDef, map);
		}

		public static Plant TryReproduceInto(IntVec3 dest, ThingDef plantDef, Map map)
		{
			if (!plantDef.CanEverPlantAt(dest, map))
			{
				return null;
			}
			if (!GenPlant.SnowAllowsPlanting(dest, map))
			{
				return null;
			}
			return (Plant)GenSpawn.Spawn(plantDef, dest, map);
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
					if (plant != null)
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
			float num4 = num3 * map.Biome.plantDensity;
			bool flag = (float)num > num4;
			if ((float)num > num4 * 1.25)
			{
				foundCell = IntVec3.Invalid;
				return false;
			}
			if (mode != 0)
			{
				BiomeDef curBiome = map.Biome;
				float num5 = curBiome.AllWildPlants.Sum((Func<ThingDef, float>)((ThingDef pd) => curBiome.CommonalityOfPlant(pd)));
				float num6 = curBiome.CommonalityOfPlant(plantDef) / num5;
				float num7 = curBiome.CommonalityOfPlant(plantDef) * plantDef.plant.wildCommonalityMaxFraction / num5;
				float num8 = num4 * num7;
				if ((float)num2 > num8)
				{
					foundCell = IntVec3.Invalid;
					return false;
				}
				float num9 = num4 * num6;
				bool flag2 = (float)num2 < num9 * 0.5;
				if (flag && !flag2)
				{
					foundCell = IntVec3.Invalid;
					return false;
				}
			}
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				if (!plantDef.CanEverPlantAt(c, map))
				{
					return false;
				}
				if (!GenPlant.SnowAllowsPlanting(c, map))
				{
					return false;
				}
				if (!source.InHorDistOf(c, radius))
				{
					return false;
				}
				if (!GenSight.LineOfSight(source, c, map, true, null, 0, 0))
				{
					return false;
				}
				return true;
			};
			return CellFinder.TryFindRandomCellNear(source, map, Mathf.CeilToInt(radius), validator, out foundCell);
		}
	}
}
