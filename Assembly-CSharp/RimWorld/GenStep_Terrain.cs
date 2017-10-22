using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_Terrain : GenStep
	{
		private static bool debug_WarnedMissingTerrain;

		public override void Generate(Map map)
		{
			BeachMaker.Init(map);
			RiverMaker river = this.GenerateRiver(map);
			List<IntVec3> list = new List<IntVec3>();
			MapGenFloatGrid mapGenFloatGrid = MapGenerator.FloatGridNamed("Elevation", map);
			MapGenFloatGrid mapGenFloatGrid2 = MapGenerator.FloatGridNamed("Fertility", map);
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 allCell in map.AllCells)
			{
				Building edifice = allCell.GetEdifice(map);
				TerrainDef terrainDef = null;
				terrainDef = ((edifice == null || edifice.def.Fillage != FillCategory.Full) ? this.TerrainFrom(allCell, map, mapGenFloatGrid[allCell], mapGenFloatGrid2[allCell], river, false) : this.TerrainFrom(allCell, map, mapGenFloatGrid[allCell], mapGenFloatGrid2[allCell], river, true));
				if ((terrainDef == TerrainDefOf.WaterMovingShallow || terrainDef == TerrainDefOf.WaterMovingDeep) && edifice != null)
				{
					list.Add(edifice.Position);
					edifice.Destroy(DestroyMode.Vanish);
				}
				terrainGrid.SetTerrain(allCell, terrainDef);
			}
			RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
			BeachMaker.Cleanup();
			List<TerrainPatchMaker>.Enumerator enumerator2 = map.Biome.terrainPatchMakers.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					TerrainPatchMaker current2 = enumerator2.Current;
					current2.Cleanup();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
		}

		private TerrainDef TerrainFrom(IntVec3 c, Map map, float elevation, float fertility, RiverMaker river, bool preferSolid)
		{
			TerrainDef terrainDef = null;
			if (river != null)
			{
				terrainDef = river.TerrainAt(c);
			}
			if (terrainDef == null && preferSolid)
			{
				return GenStep_RocksFromGrid.RockDefAt(c).naturalTerrain;
			}
			TerrainDef terrainDef2 = BeachMaker.BeachTerrainAt(c, map.Biome);
			if (terrainDef2 == TerrainDefOf.WaterOceanDeep)
			{
				return terrainDef2;
			}
			if (terrainDef != TerrainDefOf.WaterMovingShallow && terrainDef != TerrainDefOf.WaterMovingDeep)
			{
				if (terrainDef2 != null)
				{
					return terrainDef2;
				}
				if (terrainDef != null)
				{
					return terrainDef;
				}
				for (int i = 0; i < map.Biome.terrainPatchMakers.Count; i++)
				{
					terrainDef2 = map.Biome.terrainPatchMakers[i].TerrainAt(c, map);
					if (terrainDef2 != null)
					{
						return terrainDef2;
					}
				}
				if (elevation > 0.550000011920929 && elevation < 0.61000001430511475)
				{
					return TerrainDefOf.Gravel;
				}
				if (elevation >= 0.61000001430511475)
				{
					return GenStep_RocksFromGrid.RockDefAt(c).naturalTerrain;
				}
				terrainDef2 = TerrainThreshold.TerrainAtValue(map.Biome.terrainsByFertility, fertility);
				if (terrainDef2 != null)
				{
					return terrainDef2;
				}
				if (!GenStep_Terrain.debug_WarnedMissingTerrain)
				{
					Log.Error("No terrain found in biome " + map.Biome.defName + " for elevation=" + elevation + ", fertility=" + fertility);
					GenStep_Terrain.debug_WarnedMissingTerrain = true;
				}
				return TerrainDefOf.Sand;
			}
			return terrainDef;
		}

		private RiverMaker GenerateRiver(Map map)
		{
			Tile tile = Find.WorldGrid[map.Tile];
			List<Tile.RiverLink> visibleRivers = tile.VisibleRivers;
			if (((visibleRivers != null) ? visibleRivers.Count : 0) != 0)
			{
				float num = 0f;
				float num2 = 0f;
				if (visibleRivers.Count == 1)
				{
					WorldGrid worldGrid = Find.WorldGrid;
					int tile2 = map.Tile;
					Tile.RiverLink riverLink = visibleRivers[0];
					num = worldGrid.GetHeadingFromTo(tile2, riverLink.neighbor);
					num2 = (float)(num + 180.0);
				}
				else
				{
					List<Tile.RiverLink> list = (from rl in visibleRivers
					orderby -rl.river.degradeThreshold
					select rl).ToList();
					WorldGrid worldGrid2 = Find.WorldGrid;
					int tile3 = map.Tile;
					Tile.RiverLink riverLink2 = list[0];
					num = worldGrid2.GetHeadingFromTo(tile3, riverLink2.neighbor);
					WorldGrid worldGrid3 = Find.WorldGrid;
					int tile4 = map.Tile;
					Tile.RiverLink riverLink3 = list[1];
					num2 = worldGrid3.GetHeadingFromTo(tile4, riverLink3.neighbor);
				}
				float num3 = Rand.Range(0.3f, 0.7f);
				IntVec3 size = map.Size;
				float x = num3 * (float)size.x;
				float num4 = Rand.Range(0.3f, 0.7f);
				IntVec3 size2 = map.Size;
				Vector3 vector = new Vector3(x, 0f, num4 * (float)size2.z);
				Vector3 center = vector;
				float angleA = num;
				float angleB = num2;
				Tile.RiverLink riverLink4 = (from rl in visibleRivers
				orderby -rl.river.degradeThreshold
				select rl).FirstOrDefault();
				return new RiverMaker(center, angleA, angleB, riverLink4.river);
			}
			return null;
		}
	}
}
