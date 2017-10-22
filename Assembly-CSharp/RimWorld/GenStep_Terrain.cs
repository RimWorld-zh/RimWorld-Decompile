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

		private static bool debug_WarnedMissingTerrain = false;

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
					goto IL_0086;
				}
				if (caves[allCell] > 0.0)
					goto IL_0086;
				terrainDef = this.TerrainFrom(allCell, map, elevation[allCell], fertility[allCell], riverMaker, false);
				goto IL_00c5;
				IL_0086:
				terrainDef = this.TerrainFrom(allCell, map, elevation[allCell], fertility[allCell], riverMaker, true);
				goto IL_00c5;
				IL_00c5:
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
			TerrainDef result;
			TerrainDef terrainDef2;
			if (terrainDef == null && preferSolid)
			{
				result = GenStep_RocksFromGrid.RockDefAt(c).naturalTerrain;
			}
			else
			{
				terrainDef2 = BeachMaker.BeachTerrainAt(c, map.Biome);
				if (terrainDef2 == TerrainDefOf.WaterOceanDeep)
				{
					result = terrainDef2;
				}
				else if (terrainDef == TerrainDefOf.WaterMovingShallow || terrainDef == TerrainDefOf.WaterMovingDeep)
				{
					result = terrainDef;
				}
				else if (terrainDef2 != null)
				{
					result = terrainDef2;
				}
				else if (terrainDef != null)
				{
					result = terrainDef;
				}
				else
				{
					for (int i = 0; i < map.Biome.terrainPatchMakers.Count; i++)
					{
						terrainDef2 = map.Biome.terrainPatchMakers[i].TerrainAt(c, map);
						if (terrainDef2 != null)
							goto IL_00af;
					}
					if (elevation > 0.550000011920929 && elevation < 0.61000001430511475)
					{
						result = TerrainDefOf.Gravel;
					}
					else if (elevation >= 0.61000001430511475)
					{
						result = GenStep_RocksFromGrid.RockDefAt(c).naturalTerrain;
					}
					else
					{
						terrainDef2 = TerrainThreshold.TerrainAtValue(map.Biome.terrainsByFertility, fertility);
						if (terrainDef2 != null)
						{
							result = terrainDef2;
						}
						else
						{
							if (!GenStep_Terrain.debug_WarnedMissingTerrain)
							{
								Log.Error("No terrain found in biome " + map.Biome.defName + " for elevation=" + elevation + ", fertility=" + fertility);
								GenStep_Terrain.debug_WarnedMissingTerrain = true;
							}
							result = TerrainDefOf.Sand;
						}
					}
				}
			}
			goto IL_0198;
			IL_0198:
			return result;
			IL_00af:
			result = terrainDef2;
			goto IL_0198;
		}

		private RiverMaker GenerateRiver(Map map)
		{
			Tile tile = Find.WorldGrid[map.Tile];
			List<Tile.RiverLink> visibleRivers = tile.VisibleRivers;
			RiverMaker result;
			if (visibleRivers == null || visibleRivers.Count == 0)
			{
				result = null;
			}
			else
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
				result = riverMaker;
			}
			return result;
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
			for (int num9 = num7; num9 < num8; num9++)
			{
				WaterInfo waterInfo = map.waterInfo;
				List<Vector3> riverDebugData = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry = dictionary2[num9];
				riverDebugData.Add(gRLT_Entry.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData2 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry2 = dictionary2[num9 + 1];
				riverDebugData2.Add(gRLT_Entry2.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData3 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry3 = dictionary[num9];
				riverDebugData3.Add(gRLT_Entry3.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData4 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry4 = dictionary[num9 + 1];
				riverDebugData4.Add(gRLT_Entry4.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData5 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry5 = dictionary3[num9];
				riverDebugData5.Add(gRLT_Entry5.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData6 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry6 = dictionary3[num9 + 1];
				riverDebugData6.Add(gRLT_Entry6.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData7 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry7 = dictionary2[num9];
				riverDebugData7.Add(gRLT_Entry7.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData8 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry8 = dictionary[num9];
				riverDebugData8.Add(gRLT_Entry8.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData9 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry9 = dictionary[num9];
				riverDebugData9.Add(gRLT_Entry9.bestNode.ToVector3Shifted());
				List<Vector3> riverDebugData10 = waterInfo.riverDebugData;
				GRLT_Entry gRLT_Entry10 = dictionary3[num9];
				riverDebugData10.Add(gRLT_Entry10.bestNode.ToVector3Shifted());
			}
			IntVec3 size3 = map.Size;
			int width = size3.x + 4;
			IntVec3 size4 = map.Size;
			CellRect cellRect = new CellRect(-2, -2, width, size4.z + 4);
			float[] array = new float[cellRect.Area * 2];
			int num10 = 0;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 a = new IntVec3(j, 0, i);
					bool flag = true;
					for (int k = 0; k < GenAdj.AdjacentCellsAndInside.Length; k++)
					{
						flag = (flag && riverMaker.TerrainAt(a + GenAdj.AdjacentCellsAndInside[k], false) == null);
					}
					if (!flag)
					{
						Vector2 p = a.ToIntVec2.ToVector2();
						int num11 = -2147483648;
						Vector2 vector2 = Vector2.zero;
						for (int num12 = num7; num12 < num8; num12++)
						{
							GRLT_Entry gRLT_Entry11 = dictionary2[num12];
							Vector2 p2 = gRLT_Entry11.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry12 = dictionary2[num12 + 1];
							Vector2 p3 = gRLT_Entry12.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry13 = dictionary[num12];
							Vector2 p4 = gRLT_Entry13.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry14 = dictionary[num12 + 1];
							Vector2 p5 = gRLT_Entry14.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry15 = dictionary3[num12];
							Vector2 p6 = gRLT_Entry15.bestNode.ToIntVec2.ToVector2();
							GRLT_Entry gRLT_Entry16 = dictionary3[num12 + 1];
							Vector2 p7 = gRLT_Entry16.bestNode.ToIntVec2.ToVector2();
							Vector2 vector3 = GenGeo.InverseQuadBilinear(p, p4, p2, p5, p3);
							if (vector3.x >= -9.9999997473787516E-05 && vector3.x <= 1.0001000165939331 && vector3.y >= -9.9999997473787516E-05 && vector3.y <= 1.0001000165939331)
							{
								vector2 = new Vector2((float)((0.0 - vector3.x) * (float)num), (float)((vector3.y + (float)num12) * 4.0));
								num11 = num12;
								break;
							}
							Vector2 vector4 = GenGeo.InverseQuadBilinear(p, p4, p6, p5, p7);
							if (vector4.x >= -9.9999997473787516E-05 && vector4.x <= 1.0001000165939331 && vector4.y >= -9.9999997473787516E-05 && vector4.y <= 1.0001000165939331)
							{
								vector2 = new Vector2(vector4.x * (float)num, (float)((vector4.y + (float)num12) * 4.0));
								num11 = num12;
								break;
							}
						}
						if (num11 == -2147483648)
						{
							Log.ErrorOnce("Failed to find all necessary river flow data", 5273133);
						}
						array[num10] = vector2.x;
						array[num10 + 1] = vector2.y;
					}
					num10 += 2;
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
			int num13 = 0;
			for (int l = cellRect.minZ; l <= cellRect.maxZ; l++)
			{
				for (int m = cellRect.minX; m <= cellRect.maxX; m++)
				{
					IntVec3 a2 = new IntVec3(m, 0, l);
					float num14 = 0f;
					float num15 = 0f;
					float num16 = 0f;
					for (int n = 0; n < GenAdj.AdjacentCellsAndInside.Length; n++)
					{
						IntVec3 c = a2 + GenAdj.AdjacentCellsAndInside[n];
						if (cellRect.Contains(c))
						{
							int num17 = num13 + (GenAdj.AdjacentCellsAndInside[n].x + GenAdj.AdjacentCellsAndInside[n].z * cellRect.Width) * 2;
							if (array.Length <= num17 + 1 || num17 < 0)
							{
								Log.Message("you wut");
							}
							if (array[num17] != 0.0 || array[num17 + 1] != 0.0)
							{
								num14 += array[num17] * array3[n];
								num15 += array[num17 + 1] * array3[n];
								num16 += array3[n];
							}
						}
					}
					if (num16 > 0.0)
					{
						array2[num13] = num14 / num16;
						array2[num13 + 1] = num15 / num16;
					}
					num13 += 2;
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
