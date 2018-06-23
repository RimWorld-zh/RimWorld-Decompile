using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C5 RID: 1477
	public class WorldGrid : IExposable
	{
		// Token: 0x0400112B RID: 4395
		public List<Tile> tiles = new List<Tile>();

		// Token: 0x0400112C RID: 4396
		public List<Vector3> verts;

		// Token: 0x0400112D RID: 4397
		public List<int> tileIDToVerts_offsets;

		// Token: 0x0400112E RID: 4398
		public List<int> tileIDToNeighbors_offsets;

		// Token: 0x0400112F RID: 4399
		public List<int> tileIDToNeighbors_values;

		// Token: 0x04001130 RID: 4400
		public float averageTileSize;

		// Token: 0x04001131 RID: 4401
		public Vector3 viewCenter;

		// Token: 0x04001132 RID: 4402
		public float viewAngle;

		// Token: 0x04001133 RID: 4403
		private byte[] tileBiome;

		// Token: 0x04001134 RID: 4404
		private byte[] tileElevation;

		// Token: 0x04001135 RID: 4405
		private byte[] tileHilliness;

		// Token: 0x04001136 RID: 4406
		private byte[] tileTemperature;

		// Token: 0x04001137 RID: 4407
		private byte[] tileRainfall;

		// Token: 0x04001138 RID: 4408
		private byte[] tileSwampiness;

		// Token: 0x04001139 RID: 4409
		public byte[] tileFeature;

		// Token: 0x0400113A RID: 4410
		private byte[] tileRoadOrigins;

		// Token: 0x0400113B RID: 4411
		private byte[] tileRoadAdjacency;

		// Token: 0x0400113C RID: 4412
		private byte[] tileRoadDef;

		// Token: 0x0400113D RID: 4413
		private byte[] tileRiverOrigins;

		// Token: 0x0400113E RID: 4414
		private byte[] tileRiverAdjacency;

		// Token: 0x0400113F RID: 4415
		private byte[] tileRiverDef;

		// Token: 0x04001140 RID: 4416
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04001141 RID: 4417
		private const int SubdivisionsCount = 10;

		// Token: 0x04001142 RID: 4418
		public const float PlanetRadius = 100f;

		// Token: 0x04001143 RID: 4419
		public const int ElevationOffset = 8192;

		// Token: 0x04001144 RID: 4420
		public const int TemperatureOffset = 300;

		// Token: 0x04001145 RID: 4421
		public const float TemperatureMultiplier = 10f;

		// Token: 0x04001146 RID: 4422
		private int cachedTraversalDistance = -1;

		// Token: 0x04001147 RID: 4423
		private int cachedTraversalDistanceForStart = -1;

		// Token: 0x04001148 RID: 4424
		private int cachedTraversalDistanceForEnd = -1;

		// Token: 0x06001C7E RID: 7294 RVA: 0x000F5124 File Offset: 0x000F3524
		public WorldGrid()
		{
			this.CalculateViewCenterAndAngle();
			PlanetShapeGenerator.Generate(10, out this.verts, out this.tileIDToVerts_offsets, out this.tileIDToNeighbors_offsets, out this.tileIDToNeighbors_values, 100f, this.viewCenter, this.viewAngle);
			this.CalculateAverageTileSize();
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x000F5194 File Offset: 0x000F3594
		public int TilesCount
		{
			get
			{
				return this.tileIDToNeighbors_offsets.Count;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000F51B4 File Offset: 0x000F35B4
		public Vector3 NorthPolePos
		{
			get
			{
				return new Vector3(0f, 100f, 0f);
			}
		}

		// Token: 0x17000428 RID: 1064
		public Tile this[int tileID]
		{
			get
			{
				return ((ulong)tileID >= (ulong)((long)this.TilesCount)) ? null : this.tiles[tileID];
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x000F5218 File Offset: 0x000F3618
		public bool HasWorldData
		{
			get
			{
				return this.tileBiome != null;
			}
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000F523C File Offset: 0x000F363C
		public bool InBounds(int tileID)
		{
			return (ulong)tileID < (ulong)((long)this.TilesCount);
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000F525C File Offset: 0x000F365C
		public Vector2 LongLatOf(int tileID)
		{
			Vector3 tileCenter = this.GetTileCenter(tileID);
			float x = Mathf.Atan2(tileCenter.x, -tileCenter.z) * 57.29578f;
			float y = Mathf.Asin(tileCenter.y / 100f) * 57.29578f;
			return new Vector2(x, y);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000F52B4 File Offset: 0x000F36B4
		public float GetHeadingFromTo(Vector3 from, Vector3 to)
		{
			float result;
			if (from == to)
			{
				result = 0f;
			}
			else
			{
				Vector3 northPolePos = this.NorthPolePos;
				Vector3 from2;
				Vector3 rhs;
				WorldRendererUtility.GetTangentialVectorFacing(from, northPolePos, out from2, out rhs);
				Vector3 vector;
				Vector3 vector2;
				WorldRendererUtility.GetTangentialVectorFacing(from, to, out vector, out vector2);
				float num = Vector3.Angle(from2, vector);
				float num2 = Vector3.Dot(vector, rhs);
				if (num2 < 0f)
				{
					num = 360f - num;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000F532C File Offset: 0x000F372C
		public float GetHeadingFromTo(int fromTileID, int toTileID)
		{
			float result;
			if (fromTileID == toTileID)
			{
				result = 0f;
			}
			else
			{
				Vector3 tileCenter = this.GetTileCenter(fromTileID);
				Vector3 tileCenter2 = this.GetTileCenter(toTileID);
				result = this.GetHeadingFromTo(tileCenter, tileCenter2);
			}
			return result;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000F536C File Offset: 0x000F376C
		public Direction8Way GetDirection8WayFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			Direction8Way result;
			if (headingFromTo >= 337.5f || headingFromTo < 22.5f)
			{
				result = Direction8Way.North;
			}
			else if (headingFromTo < 67.5f)
			{
				result = Direction8Way.NorthEast;
			}
			else if (headingFromTo < 112.5f)
			{
				result = Direction8Way.East;
			}
			else if (headingFromTo < 157.5f)
			{
				result = Direction8Way.SouthEast;
			}
			else if (headingFromTo < 202.5f)
			{
				result = Direction8Way.South;
			}
			else if (headingFromTo < 247.5f)
			{
				result = Direction8Way.SouthWest;
			}
			else if (headingFromTo < 292.5f)
			{
				result = Direction8Way.West;
			}
			else
			{
				result = Direction8Way.NorthWest;
			}
			return result;
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000F5414 File Offset: 0x000F3814
		public Rot4 GetRotFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			Rot4 result;
			if (headingFromTo >= 315f || headingFromTo < 45f)
			{
				result = Rot4.North;
			}
			else if (headingFromTo < 135f)
			{
				result = Rot4.East;
			}
			else if (headingFromTo < 225f)
			{
				result = Rot4.South;
			}
			else
			{
				result = Rot4.West;
			}
			return result;
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000F5484 File Offset: 0x000F3884
		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000F549A File Offset: 0x000F389A
		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			PackedListOfLists.GetListValuesIndices<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVertsIndices);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000F54B0 File Offset: 0x000F38B0
		public void GetTileNeighbors(int tileID, List<int> outNeighbors)
		{
			PackedListOfLists.GetList<int>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000F54C8 File Offset: 0x000F38C8
		public int GetTileNeighborCount(int tileID)
		{
			return PackedListOfLists.GetListCount<int>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000F54F0 File Offset: 0x000F38F0
		public int GetMaxTileNeighborCountEver(int tileID)
		{
			return PackedListOfLists.GetListCount<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000F5518 File Offset: 0x000F3918
		public bool IsNeighbor(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.Contains(tile2);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000F5544 File Offset: 0x000F3944
		public bool IsNeighborOrSame(int tile1, int tile2)
		{
			return tile1 == tile2 || this.IsNeighbor(tile1, tile2);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000F556C File Offset: 0x000F396C
		public int GetNeighborId(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.IndexOf(tile2);
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000F5598 File Offset: 0x000F3998
		public int GetTileNeighbor(int tileID, int adjacentId)
		{
			this.GetTileNeighbors(tileID, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors[adjacentId];
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000F55C4 File Offset: 0x000F39C4
		public Vector3 GetTileCenter(int tileID)
		{
			int num = (tileID + 1 >= this.tileIDToVerts_offsets.Count) ? this.verts.Count : this.tileIDToVerts_offsets[tileID + 1];
			Vector3 a = Vector3.zero;
			int num2 = 0;
			for (int i = this.tileIDToVerts_offsets[tileID]; i < num; i++)
			{
				a += this.verts[i];
				num2++;
			}
			return a / (float)num2;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000F5654 File Offset: 0x000F3A54
		public float TileRadiusToAngle(float radius)
		{
			return this.DistOnSurfaceToAngle(radius * this.averageTileSize);
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000F5678 File Offset: 0x000F3A78
		public float DistOnSurfaceToAngle(float dist)
		{
			return dist / 628.318542f * 360f;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000F569C File Offset: 0x000F3A9C
		public float DistanceFromEquatorNormalized(int tile)
		{
			return Mathf.Abs(Find.WorldGrid.GetTileCenter(tile).y / 100f);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000F56D0 File Offset: 0x000F3AD0
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return sphericalDistance * 100f / this.averageTileSize;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000F56F4 File Offset: 0x000F3AF4
		public float ApproxDistanceInTiles(int firstTile, int secondTile)
		{
			Vector3 tileCenter = this.GetTileCenter(firstTile);
			Vector3 tileCenter2 = this.GetTileCenter(secondTile);
			return this.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, tileCenter2.normalized));
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000F5734 File Offset: 0x000F3B34
		public void OverlayRoad(int fromTile, int toTile, RoadDef roadDef)
		{
			if (roadDef == null)
			{
				Log.ErrorOnce("Attempted to remove road with overlayRoad; not supported", 90292249, false);
			}
			else
			{
				RoadDef roadDef2 = this.GetRoadDef(fromTile, toTile, false);
				if (roadDef2 != roadDef)
				{
					Tile tile = this[fromTile];
					Tile tile2 = this[toTile];
					if (roadDef2 != null)
					{
						if (roadDef2.priority >= roadDef.priority)
						{
							return;
						}
						tile.potentialRoads.RemoveAll((Tile.RoadLink rl) => rl.neighbor == toTile);
						tile2.potentialRoads.RemoveAll((Tile.RoadLink rl) => rl.neighbor == fromTile);
					}
					if (tile.potentialRoads == null)
					{
						tile.potentialRoads = new List<Tile.RoadLink>();
					}
					if (tile2.potentialRoads == null)
					{
						tile2.potentialRoads = new List<Tile.RoadLink>();
					}
					tile.potentialRoads.Add(new Tile.RoadLink
					{
						neighbor = toTile,
						road = roadDef
					});
					tile2.potentialRoads.Add(new Tile.RoadLink
					{
						neighbor = fromTile,
						road = roadDef
					});
				}
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000F587C File Offset: 0x000F3C7C
		public RoadDef GetRoadDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			RoadDef result;
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444, false);
				result = null;
			}
			else
			{
				Tile tile = this.tiles[fromTile];
				List<Tile.RoadLink> list = (!visibleOnly) ? tile.potentialRoads : tile.Roads;
				if (list == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].neighbor == toTile)
						{
							return list[i].road;
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000F5930 File Offset: 0x000F3D30
		public void OverlayRiver(int fromTile, int toTile, RiverDef riverDef)
		{
			if (riverDef == null)
			{
				Log.ErrorOnce("Attempted to remove river with overlayRiver; not supported", 90292250, false);
			}
			else
			{
				RiverDef riverDef2 = this.GetRiverDef(fromTile, toTile, false);
				if (riverDef2 != riverDef)
				{
					Tile tile = this[fromTile];
					Tile tile2 = this[toTile];
					if (riverDef2 != null)
					{
						if (riverDef2.degradeThreshold >= riverDef.degradeThreshold)
						{
							return;
						}
						tile.potentialRivers.RemoveAll((Tile.RiverLink rl) => rl.neighbor == toTile);
						tile2.potentialRivers.RemoveAll((Tile.RiverLink rl) => rl.neighbor == fromTile);
					}
					if (tile.potentialRivers == null)
					{
						tile.potentialRivers = new List<Tile.RiverLink>();
					}
					if (tile2.potentialRivers == null)
					{
						tile2.potentialRivers = new List<Tile.RiverLink>();
					}
					tile.potentialRivers.Add(new Tile.RiverLink
					{
						neighbor = toTile,
						river = riverDef
					});
					tile2.potentialRivers.Add(new Tile.RiverLink
					{
						neighbor = fromTile,
						river = riverDef
					});
				}
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000F5A78 File Offset: 0x000F3E78
		public RiverDef GetRiverDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			RiverDef result;
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444, false);
				result = null;
			}
			else
			{
				Tile tile = this.tiles[fromTile];
				List<Tile.RiverLink> list = (!visibleOnly) ? tile.potentialRivers : tile.Rivers;
				if (list == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].neighbor == toTile)
						{
							return list[i].river;
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000F5B2C File Offset: 0x000F3F2C
		public float GetRoadMovementDifficultyMultiplier(int fromTile, int toTile, StringBuilder explanation = null)
		{
			List<Tile.RoadLink> roads = this.tiles[fromTile].Roads;
			float result;
			if (roads == null)
			{
				result = 1f;
			}
			else
			{
				if (toTile == -1)
				{
					toTile = this.FindMostReasonableAdjacentTileForDisplayedPathCost(fromTile);
				}
				for (int i = 0; i < roads.Count; i++)
				{
					if (roads[i].neighbor == toTile)
					{
						float movementCostMultiplier = roads[i].road.movementCostMultiplier;
						if (explanation != null)
						{
							if (explanation.Length > 0)
							{
								explanation.AppendLine();
							}
							explanation.Append(roads[i].road.LabelCap + ": " + movementCostMultiplier.ToStringPercent());
						}
						return movementCostMultiplier;
					}
				}
				result = 1f;
			}
			return result;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000F5C10 File Offset: 0x000F4010
		public int FindMostReasonableAdjacentTileForDisplayedPathCost(int fromTile)
		{
			Tile tile = this.tiles[fromTile];
			float num = 1f;
			int num2 = -1;
			List<Tile.RoadLink> roads = tile.Roads;
			if (roads != null)
			{
				for (int i = 0; i < roads.Count; i++)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (movementCostMultiplier < num && !Find.World.Impassable(roads[i].neighbor))
					{
						num = movementCostMultiplier;
						num2 = roads[i].neighbor;
					}
				}
			}
			int result;
			if (num2 != -1)
			{
				result = num2;
			}
			else
			{
				WorldGrid.tmpNeighbors.Clear();
				this.GetTileNeighbors(fromTile, WorldGrid.tmpNeighbors);
				for (int j = 0; j < WorldGrid.tmpNeighbors.Count; j++)
				{
					if (!Find.World.Impassable(WorldGrid.tmpNeighbors[j]))
					{
						return WorldGrid.tmpNeighbors[j];
					}
				}
				result = fromTile;
			}
			return result;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000F5D34 File Offset: 0x000F4134
		public int TraversalDistanceBetween(int start, int end, bool passImpassable = true, int maxDist = 2147483647)
		{
			int result;
			if (start < 0 || end < 0)
			{
				result = int.MaxValue;
			}
			else if (this.cachedTraversalDistanceForStart == start && this.cachedTraversalDistanceForEnd == end && passImpassable && maxDist == 2147483647)
			{
				result = this.cachedTraversalDistance;
			}
			else if (!passImpassable && !Find.WorldReachability.CanReach(start, end))
			{
				result = int.MaxValue;
			}
			else
			{
				int finalDist = int.MaxValue;
				int maxTilesToProcess = (maxDist != int.MaxValue) ? this.TilesNumWithinTraversalDistance(maxDist + 1) : int.MaxValue;
				Find.WorldFloodFiller.FloodFill(start, (int x) => passImpassable || !Find.World.Impassable(x), delegate(int tile, int dist)
				{
					bool result2;
					if (tile == end)
					{
						finalDist = dist;
						result2 = true;
					}
					else
					{
						result2 = false;
					}
					return result2;
				}, maxTilesToProcess, null);
				if (passImpassable && maxDist == 2147483647)
				{
					this.cachedTraversalDistance = finalDist;
					this.cachedTraversalDistanceForStart = start;
					this.cachedTraversalDistanceForEnd = end;
				}
				result = finalDist;
			}
			return result;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000F5E78 File Offset: 0x000F4278
		public int TilesNumWithinTraversalDistance(int traversalDist)
		{
			int result;
			if (traversalDist < 0)
			{
				result = 0;
			}
			else
			{
				result = 3 * traversalDist * (traversalDist + 1) + 1;
			}
			return result;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000F5EA4 File Offset: 0x000F42A4
		public bool IsOnEdge(int tileID)
		{
			return this.InBounds(tileID) && this.GetTileNeighborCount(tileID) < this.GetMaxTileNeighborCountEver(tileID);
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000F5ED8 File Offset: 0x000F42D8
		private void CalculateAverageTileSize()
		{
			int tilesCount = this.TilesCount;
			double num = 0.0;
			int num2 = 0;
			for (int i = 0; i < tilesCount; i++)
			{
				Vector3 tileCenter = this.GetTileCenter(i);
				int num3 = (i + 1 >= this.tileIDToNeighbors_offsets.Count) ? this.tileIDToNeighbors_values.Count : this.tileIDToNeighbors_offsets[i + 1];
				for (int j = this.tileIDToNeighbors_offsets[i]; j < num3; j++)
				{
					int tileID = this.tileIDToNeighbors_values[j];
					Vector3 tileCenter2 = this.GetTileCenter(tileID);
					num += (double)Vector3.Distance(tileCenter, tileCenter2);
					num2++;
				}
			}
			this.averageTileSize = (float)(num / (double)num2);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000F5FA4 File Offset: 0x000F43A4
		private void CalculateViewCenterAndAngle()
		{
			this.viewAngle = Find.World.PlanetCoverage * 180f;
			this.viewCenter = Vector3.back;
			float angle = 45f;
			if (this.viewAngle > 45f)
			{
				angle = Mathf.Max(90f - this.viewAngle, 0f);
			}
			this.viewCenter = Quaternion.AngleAxis(angle, Vector3.right) * this.viewCenter;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000F601C File Offset: 0x000F441C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.TilesToRawData();
			}
			DataExposeUtility.ByteArray(ref this.tileBiome, "tileBiome");
			DataExposeUtility.ByteArray(ref this.tileElevation, "tileElevation");
			DataExposeUtility.ByteArray(ref this.tileHilliness, "tileHilliness");
			DataExposeUtility.ByteArray(ref this.tileTemperature, "tileTemperature");
			DataExposeUtility.ByteArray(ref this.tileRainfall, "tileRainfall");
			DataExposeUtility.ByteArray(ref this.tileSwampiness, "tileSwampiness");
			DataExposeUtility.ByteArray(ref this.tileFeature, "tileFeature");
			DataExposeUtility.ByteArray(ref this.tileRoadOrigins, "tileRoadOrigins");
			DataExposeUtility.ByteArray(ref this.tileRoadAdjacency, "tileRoadAdjacency");
			DataExposeUtility.ByteArray(ref this.tileRoadDef, "tileRoadDef");
			DataExposeUtility.ByteArray(ref this.tileRiverOrigins, "tileRiverOrigins");
			DataExposeUtility.ByteArray(ref this.tileRiverAdjacency, "tileRiverAdjacency");
			DataExposeUtility.ByteArray(ref this.tileRiverDef, "tileRiverDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.RawDataToTiles();
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000F611C File Offset: 0x000F451C
		public void StandardizeTileData()
		{
			this.TilesToRawData();
			this.RawDataToTiles();
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000F612C File Offset: 0x000F452C
		private void TilesToRawData()
		{
			this.tileBiome = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => this.tiles[i].biome.shortHash);
			this.tileElevation = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(((!this.tiles[i].WaterCovered) ? Mathf.Max(this.tiles[i].elevation, 1f) : this.tiles[i].elevation) + 8192f), 0, 65535));
			this.tileHilliness = DataSerializeUtility.SerializeByte(this.TilesCount, (int i) => (byte)this.tiles[i].hilliness);
			this.tileTemperature = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((this.tiles[i].temperature + 300f) * 10f), 0, 65535));
			this.tileRainfall = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].rainfall), 0, 65535));
			this.tileSwampiness = DataSerializeUtility.SerializeByte(this.TilesCount, (int i) => (byte)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].swampiness * 255f), 0, 255));
			this.tileFeature = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (this.tiles[i].feature != null) ? ((ushort)this.tiles[i].feature.uniqueID) : ushort.MaxValue);
			List<int> list = new List<int>();
			List<byte> list2 = new List<byte>();
			List<ushort> list3 = new List<ushort>();
			for (int m = 0; m < this.TilesCount; m++)
			{
				List<Tile.RoadLink> potentialRoads = this.tiles[m].potentialRoads;
				if (potentialRoads != null)
				{
					for (int j = 0; j < potentialRoads.Count; j++)
					{
						Tile.RoadLink roadLink = potentialRoads[j];
						if (roadLink.neighbor >= m)
						{
							byte b = (byte)this.GetNeighborId(m, roadLink.neighbor);
							if (b < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for road piece", 81637014, false);
							}
							else
							{
								list.Add(m);
								list2.Add(b);
								list3.Add(roadLink.road.shortHash);
							}
						}
					}
				}
			}
			this.tileRoadOrigins = DataSerializeUtility.SerializeInt(list.ToArray());
			this.tileRoadAdjacency = DataSerializeUtility.SerializeByte(list2.ToArray());
			this.tileRoadDef = DataSerializeUtility.SerializeUshort(list3.ToArray());
			List<int> list4 = new List<int>();
			List<byte> list5 = new List<byte>();
			List<ushort> list6 = new List<ushort>();
			for (int k = 0; k < this.TilesCount; k++)
			{
				List<Tile.RiverLink> potentialRivers = this.tiles[k].potentialRivers;
				if (potentialRivers != null)
				{
					for (int l = 0; l < potentialRivers.Count; l++)
					{
						Tile.RiverLink riverLink = potentialRivers[l];
						if (riverLink.neighbor >= k)
						{
							byte b2 = (byte)this.GetNeighborId(k, riverLink.neighbor);
							if (b2 < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for river piece", 81637014, false);
							}
							else
							{
								list4.Add(k);
								list5.Add(b2);
								list6.Add(riverLink.river.shortHash);
							}
						}
					}
				}
			}
			this.tileRiverOrigins = DataSerializeUtility.SerializeInt(list4.ToArray());
			this.tileRiverAdjacency = DataSerializeUtility.SerializeByte(list5.ToArray());
			this.tileRiverDef = DataSerializeUtility.SerializeUshort(list6.ToArray());
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000F6424 File Offset: 0x000F4824
		private void RawDataToTiles()
		{
			if (this.tiles.Count != this.TilesCount)
			{
				this.tiles.Clear();
				for (int m = 0; m < this.TilesCount; m++)
				{
					this.tiles.Add(new Tile());
				}
			}
			else
			{
				for (int j = 0; j < this.TilesCount; j++)
				{
					this.tiles[j].potentialRoads = null;
					this.tiles[j].potentialRivers = null;
				}
			}
			DataSerializeUtility.LoadUshort(this.tileBiome, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].biome = (DefDatabase<BiomeDef>.GetByShortHash(data) ?? BiomeDefOf.TemperateForest);
			});
			DataSerializeUtility.LoadUshort(this.tileElevation, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].elevation = (float)(data - 8192);
			});
			DataSerializeUtility.LoadByte(this.tileHilliness, this.TilesCount, delegate(int i, byte data)
			{
				this.tiles[i].hilliness = (Hilliness)data;
			});
			DataSerializeUtility.LoadUshort(this.tileTemperature, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].temperature = (float)data / 10f - 300f;
			});
			DataSerializeUtility.LoadUshort(this.tileRainfall, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].rainfall = (float)data;
			});
			DataSerializeUtility.LoadByte(this.tileSwampiness, this.TilesCount, delegate(int i, byte data)
			{
				this.tiles[i].swampiness = (float)data / 255f;
			});
			int[] array = DataSerializeUtility.DeserializeInt(this.tileRoadOrigins);
			byte[] array2 = DataSerializeUtility.DeserializeByte(this.tileRoadAdjacency);
			ushort[] array3 = DataSerializeUtility.DeserializeUshort(this.tileRoadDef);
			for (int k = 0; k < array.Length; k++)
			{
				int num = array[k];
				int tileNeighbor = this.GetTileNeighbor(num, (int)array2[k]);
				RoadDef byShortHash = DefDatabase<RoadDef>.GetByShortHash(array3[k]);
				if (byShortHash != null)
				{
					if (this.tiles[num].potentialRoads == null)
					{
						this.tiles[num].potentialRoads = new List<Tile.RoadLink>();
					}
					if (this.tiles[tileNeighbor].potentialRoads == null)
					{
						this.tiles[tileNeighbor].potentialRoads = new List<Tile.RoadLink>();
					}
					this.tiles[num].potentialRoads.Add(new Tile.RoadLink
					{
						neighbor = tileNeighbor,
						road = byShortHash
					});
					this.tiles[tileNeighbor].potentialRoads.Add(new Tile.RoadLink
					{
						neighbor = num,
						road = byShortHash
					});
				}
			}
			int[] array4 = DataSerializeUtility.DeserializeInt(this.tileRiverOrigins);
			byte[] array5 = DataSerializeUtility.DeserializeByte(this.tileRiverAdjacency);
			ushort[] array6 = DataSerializeUtility.DeserializeUshort(this.tileRiverDef);
			for (int l = 0; l < array4.Length; l++)
			{
				int num2 = array4[l];
				int tileNeighbor2 = this.GetTileNeighbor(num2, (int)array5[l]);
				RiverDef byShortHash2 = DefDatabase<RiverDef>.GetByShortHash(array6[l]);
				if (byShortHash2 != null)
				{
					if (this.tiles[num2].potentialRivers == null)
					{
						this.tiles[num2].potentialRivers = new List<Tile.RiverLink>();
					}
					if (this.tiles[tileNeighbor2].potentialRivers == null)
					{
						this.tiles[tileNeighbor2].potentialRivers = new List<Tile.RiverLink>();
					}
					this.tiles[num2].potentialRivers.Add(new Tile.RiverLink
					{
						neighbor = tileNeighbor2,
						river = byShortHash2
					});
					this.tiles[tileNeighbor2].potentialRivers.Add(new Tile.RiverLink
					{
						neighbor = num2,
						river = byShortHash2
					});
				}
			}
		}
	}
}
