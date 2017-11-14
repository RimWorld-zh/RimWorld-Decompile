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
		private struct GRLT_Entry
		{
			public float bestDistance;

			public IntVec3 bestNode;
		}

		private static bool debug_WarnedMissingTerrain;

		public override void Generate(Map map)
		{
			BeachMaker.Init(map);
			RiverMaker riverMaker = this.GenerateRiver(map);
			List<IntVec3> list = new List<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid fertility = MapGenerator.Fertility;
			MapGenFloatGrid caves = MapGenerator.Caves;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 allCell in map.AllCells)
			{
				Building edifice = allCell.GetEdifice(map);
				TerrainDef terrainDef = null;
				if (edifice != null && edifice.def.Fillage == FillCategory.Full)
				{
					goto IL_0083;
				}
				if (caves[allCell] > 0.0)
					goto IL_0083;
				terrainDef = this.TerrainFrom(allCell, map, elevation[allCell], fertility[allCell], riverMaker, false);
				goto IL_00c2;
				IL_0083:
				terrainDef = this.TerrainFrom(allCell, map, elevation[allCell], fertility[allCell], riverMaker, true);
				goto IL_00c2;
				IL_00c2:
				if ((terrainDef == TerrainDefOf.WaterMovingShallow || terrainDef == TerrainDefOf.WaterMovingDeep) && edifice != null)
				{
					list.Add(edifice.Position);
					edifice.Destroy(DestroyMode.Vanish);
				}
				terrainGrid.SetTerrain(allCell, terrainDef);
			}
			if (riverMaker != null)
			{
				riverMaker.ValidatePassage(map);
			}
			RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
			BeachMaker.Cleanup();
			foreach (TerrainPatchMaker terrainPatchMaker in map.Biome.terrainPatchMakers)
			{
				terrainPatchMaker.Cleanup();
			}
		}

		private TerrainDef TerrainFrom(IntVec3 c, Map map, float elevation, float fertility, RiverMaker river, bool preferSolid)
		{
			TerrainDef terrainDef = null;
			if (river != null)
			{
				terrainDef = river.TerrainAt(c, true);
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
			if (visibleRivers != null && visibleRivers.Count != 0)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				int tile2 = map.Tile;
				Tile.RiverLink riverLink = (from rl in visibleRivers
				orderby -rl.river.degradeThreshold
				select rl).First();
				float headingFromTo = worldGrid.GetHeadingFromTo(tile2, riverLink.neighbor);
				float num = Rand.Range(0.3f, 0.7f);
				IntVec3 size = map.Size;
				float x = num * (float)size.x;
				float num2 = Rand.Range(0.3f, 0.7f);
				IntVec3 size2 = map.Size;
				Vector3 vector = new Vector3(x, 0f, num2 * (float)size2.z);
				Vector3 center = vector;
				float angle = headingFromTo;
				Tile.RiverLink riverLink2 = (from rl in visibleRivers
				orderby -rl.river.degradeThreshold
				select rl).FirstOrDefault();
				RiverMaker riverMaker = new RiverMaker(center, angle, riverLink2.river);
				this.GenerateRiverLookupTexture(map, riverMaker);
				return riverMaker;
			}
			return null;
		}

		private void UpdateRiverAnchorEntry(Dictionary<int, GRLT_Entry> entries, IntVec3 center, int entryId, float zValue)
		{
			float num = zValue - (float)entryId;
			if (!(num > 2.0))
			{
				if (entries.ContainsKey(entryId))
				{
					GRLT_Entry gRLT_Entry = entries[entryId];
					if (!(gRLT_Entry.bestDistance > num))
						return;
				}
				entries[entryId] = new GRLT_Entry
				{
					bestDistance = num,
					bestNode = center
				};
			}
		}

		private void GenerateRiverLookupTexture(Map map, RiverMaker riverMaker)
		{
			int num = Mathf.CeilToInt((from rd in DefDatabase<RiverDef>.AllDefs
			select (float)(rd.widthOnMap / 2.0 + 5.0)).Max());
			int num2 = Mathf.Max(4, num) * 2;
			Dictionary<int, GRLT_Entry> dictionary = new Dictionary<int, GRLT_Entry>();
			Dictionary<int, GRLT_Entry> dictionary2 = new Dictionary<int, GRLT_Entry>();
			Dictionary<int, GRLT_Entry> dictionary3 = new Dictionary<int, GRLT_Entry>();
			int num3 = -num2;
			while (true)
			{
				int num4 = num3;
				IntVec3 size = map.Size;
				if (num4 < size.z + num2)
				{
					int num5 = -num2;
					while (true)
					{
						int num6 = num5;
						IntVec3 size2 = map.Size;
						if (num6 < size2.x + num2)
						{
							IntVec3 intVec = new IntVec3(num5, 0, num3);
							Vector3 vector = riverMaker.WaterCoordinateAt(intVec);
							int entryId = Mathf.FloorToInt((float)(vector.z / 4.0));
							this.UpdateRiverAnchorEntry(dictionary, intVec, entryId, (float)((vector.z + Mathf.Abs(vector.x)) / 4.0));
							this.UpdateRiverAnchorEntry(dictionary2, intVec, entryId, (float)((vector.z + Mathf.Abs(vector.x - (float)num)) / 4.0));
							this.UpdateRiverAnchorEntry(dictionary3, intVec, entryId, (float)((vector.z + Mathf.Abs(vector.x + (float)num)) / 4.0));
							num5++;
							continue;
						}
						break;
					}
					num3++;
					continue;
				}
				break;
			}
			int num7 = Mathf.Max(dictionary.Keys.Min(), dictionary2.Keys.Min(), dictionary3.Keys.Min());
			int num8 = Mathf.Min(dictionary.Keys.Max(), dictionary2.Keys.Max(), dictionary3.Keys.Max());
			for (int i = num7; i < num8; i++)
			{
				WaterInfo waterInfo = map.waterInfo;
				if (dictionary2.ContainsKey(i) && dictionary2.ContainsKey(i + 1))
				{
					List<Vector3> riverDebugData = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry = dictionary2[i];
					riverDebugData.Add(gRLT_Entry.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData2 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry2 = dictionary2[i + 1];
					riverDebugData2.Add(gRLT_Entry2.bestNode.ToVector3Shifted());
				}
				if (dictionary.ContainsKey(i) && dictionary.ContainsKey(i + 1))
				{
					List<Vector3> riverDebugData3 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry3 = dictionary[i];
					riverDebugData3.Add(gRLT_Entry3.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData4 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry4 = dictionary[i + 1];
					riverDebugData4.Add(gRLT_Entry4.bestNode.ToVector3Shifted());
				}
				if (dictionary3.ContainsKey(i) && dictionary3.ContainsKey(i + 1))
				{
					List<Vector3> riverDebugData5 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry5 = dictionary3[i];
					riverDebugData5.Add(gRLT_Entry5.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData6 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry6 = dictionary3[i + 1];
					riverDebugData6.Add(gRLT_Entry6.bestNode.ToVector3Shifted());
				}
				if (dictionary2.ContainsKey(i) && dictionary.ContainsKey(i))
				{
					List<Vector3> riverDebugData7 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry7 = dictionary2[i];
					riverDebugData7.Add(gRLT_Entry7.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData8 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry8 = dictionary[i];
					riverDebugData8.Add(gRLT_Entry8.bestNode.ToVector3Shifted());
				}
				if (dictionary.ContainsKey(i) && dictionary3.ContainsKey(i))
				{
					List<Vector3> riverDebugData9 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry9 = dictionary[i];
					riverDebugData9.Add(gRLT_Entry9.bestNode.ToVector3Shifted());
					List<Vector3> riverDebugData10 = waterInfo.riverDebugData;
					GRLT_Entry gRLT_Entry10 = dictionary3[i];
					riverDebugData10.Add(gRLT_Entry10.bestNode.ToVector3Shifted());
				}
			}
			IntVec3 size3 = map.Size;
			int width = size3.x + 4;
			IntVec3 size4 = map.Size;
			CellRect cellRect = new CellRect(-2, -2, width, size4.z + 4);
			float[] array = new float[cellRect.Area * 2];
			int num9 = 0;
			for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
			{
				for (int k = cellRect.minX; k <= cellRect.maxX; k++)
				{
					IntVec3 a = new IntVec3(k, 0, j);
					bool flag = true;
					int num10 = 0;
					while (num10 < GenAdj.AdjacentCellsAndInside.Length)
					{
						if (riverMaker.TerrainAt(a + GenAdj.AdjacentCellsAndInside[num10], false) == null)
						{
							num10++;
							continue;
						}
						flag = false;
						break;
					}
					if (!flag)
					{
						Vector2 p = a.ToIntVec2.ToVector2();
						int num11 = -2147483648;
						Vector2 vector2 = Vector2.zero;
						for (int l = num7; l < num8; l++)
						{
							GRLT_Entry gRLT_Entry11 = dictionary2[l];
							Vector2 p2 = gRLT_Entry11.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry12 = dictionary2[l + 1];
							Vector2 p3 = gRLT_Entry12.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry13 = dictionary[l];
							Vector2 p4 = gRLT_Entry13.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry14 = dictionary[l + 1];
							Vector2 p5 = gRLT_Entry14.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry15 = dictionary3[l];
							Vector2 p6 = gRLT_Entry15.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry16 = dictionary3[l + 1];
							Vector2 p7 = gRLT_Entry16.bestNode.ToIntVec2.ToVector2();
							Vector2 vector3 = GenGeo.InverseQuadBilinear(p, p4, p2, p5, p3);
							if (vector3.x >= -9.9999997473787516E-05 && vector3.x <= 1.0001000165939331 && vector3.y >= -9.9999997473787516E-05 && vector3.y <= 1.0001000165939331)
							{
								vector2 = new Vector2((float)((0.0 - vector3.x) * (float)num), (float)((vector3.y + (float)l) * 4.0));
								num11 = l;
								break;
							}
							Vector2 vector4 = GenGeo.InverseQuadBilinear(p, p4, p6, p5, p7);
							if (vector4.x >= -9.9999997473787516E-05 && vector4.x <= 1.0001000165939331 && vector4.y >= -9.9999997473787516E-05 && vector4.y <= 1.0001000165939331)
							{
								vector2 = new Vector2(vector4.x * (float)num, (float)((vector4.y + (float)l) * 4.0));
								num11 = l;
								break;
							}
						}
						if (num11 == -2147483648)
						{
							Log.ErrorOnce("Failed to find all necessary river flow data", 5273133);
						}
						array[num9] = vector2.x;
						array[num9 + 1] = vector2.y;
					}
					num9 += 2;
				}
			}
			float[] array2 = new float[cellRect.Area * 2];
			float[] array3 = new float[9]
			{
				0.123317f,
				0.123317f,
				0.123317f,
				0.123317f,
				0.077847f,
				0.077847f,
				0.077847f,
				0.077847f,
				0.195346f
			};
			int num12 = 0;
			for (int m = cellRect.minZ; m <= cellRect.maxZ; m++)
			{
				for (int n = cellRect.minX; n <= cellRect.maxX; n++)
				{
					IntVec3 a2 = new IntVec3(n, 0, m);
					float num13 = 0f;
					float num14 = 0f;
					float num15 = 0f;
					for (int num16 = 0; num16 < GenAdj.AdjacentCellsAndInside.Length; num16++)
					{
						IntVec3 c = a2 + GenAdj.AdjacentCellsAndInside[num16];
						if (cellRect.Contains(c))
						{
							int num17 = num12 + (GenAdj.AdjacentCellsAndInside[num16].x + GenAdj.AdjacentCellsAndInside[num16].z * cellRect.Width) * 2;
							if (array.Length <= num17 + 1 || num17 < 0)
							{
								Log.Message("you wut");
							}
							if (array[num17] != 0.0 || array[num17 + 1] != 0.0)
							{
								num13 += array[num17] * array3[num16];
								num14 += array[num17 + 1] * array3[num16];
								num15 += array3[num16];
							}
						}
					}
					if (num15 > 0.0)
					{
						array2[num12] = num13 / num15;
						array2[num12 + 1] = num14 / num15;
					}
					num12 += 2;
				}
			}
			array = array2;
			for (int num18 = 0; num18 < array.Length; num18 += 2)
			{
				if (array[num18] != 0.0 || array[num18 + 1] != 0.0)
				{
					Vector3 vector5 = Rand.PointOnDisc * 0.4f;
					array[num18] += vector5.x;
					array[num18 + 1] += vector5.z;
				}
			}
			byte[] array4 = new byte[array.Length * 4];
			Buffer.BlockCopy(array, 0, array4, 0, array.Length * 4);
			map.waterInfo.riverOffsetMap = array4;
			map.waterInfo.GenerateRiverFlowMap();
		}
	}
}
