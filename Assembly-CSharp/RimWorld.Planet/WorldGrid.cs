using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGrid : IExposable
	{
		public List<Tile> tiles = new List<Tile>();

		public List<Vector3> verts;

		public List<int> tileIDToVerts_offsets;

		public List<int> tileIDToNeighbors_offsets;

		public List<int> tileIDToNeighbors_values;

		public float averageTileSize;

		public Vector3 viewCenter;

		public float viewAngle;

		private byte[] tileBiome;

		private byte[] tileElevation;

		private byte[] tileHilliness;

		private byte[] tileTemperature;

		private byte[] tileRainfall;

		private byte[] tileSwampiness;

		public byte[] tileFeature;

		private byte[] tileRoadOrigins;

		private byte[] tileRoadAdjacency;

		private byte[] tileRoadDef;

		private byte[] tileRiverOrigins;

		private byte[] tileRiverAdjacency;

		private byte[] tileRiverDef;

		private static List<int> tmpNeighbors = new List<int>();

		private const int SubdivisionsCount = 10;

		public const float PlanetRadius = 100f;

		public const int ElevationOffset = 8192;

		public const int TemperatureOffset = 300;

		public const float TemperatureMultiplier = 10f;

		private int cachedTraversalDistance = -1;

		private int cachedTraversalDistanceForStart = -1;

		private int cachedTraversalDistanceForEnd = -1;

		public int TilesCount
		{
			get
			{
				return this.tileIDToNeighbors_offsets.Count;
			}
		}

		public Vector3 NorthPolePos
		{
			get
			{
				return new Vector3(0f, 100f, 0f);
			}
		}

		public Tile this[int tileID]
		{
			get
			{
				return ((uint)tileID >= this.TilesCount) ? null : this.tiles[tileID];
			}
		}

		public bool HasWorldData
		{
			get
			{
				return this.tileBiome != null;
			}
		}

		public WorldGrid()
		{
			this.CalculateViewCenterAndAngle();
			PlanetShapeGenerator.Generate(10, out this.verts, out this.tileIDToVerts_offsets, out this.tileIDToNeighbors_offsets, out this.tileIDToNeighbors_values, 100f, this.viewCenter, this.viewAngle);
			this.CalculateAverageTileSize();
		}

		public bool InBounds(int tileID)
		{
			return (uint)tileID < this.TilesCount;
		}

		public Vector2 LongLatOf(int tileID)
		{
			Vector3 tileCenter = this.GetTileCenter(tileID);
			float x = (float)(Mathf.Atan2(tileCenter.x, (float)(0.0 - tileCenter.z)) * 57.295780181884766);
			float y = (float)(Mathf.Asin((float)(tileCenter.y / 100.0)) * 57.295780181884766);
			return new Vector2(x, y);
		}

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
				Vector3 from2 = default(Vector3);
				Vector3 rhs = default(Vector3);
				WorldRendererUtility.GetTangentialVectorFacing(from, northPolePos, out from2, out rhs);
				Vector3 vector = default(Vector3);
				Vector3 vector2 = default(Vector3);
				WorldRendererUtility.GetTangentialVectorFacing(from, to, out vector, out vector2);
				float num = Vector3.Angle(from2, vector);
				float num2 = Vector3.Dot(vector, rhs);
				if (num2 < 0.0)
				{
					num = (float)(360.0 - num);
				}
				result = num;
			}
			return result;
		}

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

		public Direction8Way GetDirection8WayFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			return (Direction8Way)((!(headingFromTo >= 337.5) && !(headingFromTo < 22.5)) ? ((headingFromTo < 67.5) ? 1 : ((!(headingFromTo < 112.5)) ? ((!(headingFromTo < 157.5)) ? ((!(headingFromTo < 202.5)) ? ((!(headingFromTo < 247.5)) ? ((!(headingFromTo < 292.5)) ? 7 : 6) : 5) : 4) : 3) : 2)) : 0);
		}

		public Rot4 GetRotFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			return (headingFromTo >= 315.0 || headingFromTo < 45.0) ? Rot4.North : ((!(headingFromTo < 135.0)) ? ((!(headingFromTo < 225.0)) ? Rot4.West : Rot4.South) : Rot4.East);
		}

		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			PackedListOfLists.GetListValuesIndices(this.tileIDToVerts_offsets, this.verts, tileID, outVertsIndices);
		}

		public void GetTileNeighbors(int tileID, List<int> outNeighbors)
		{
			PackedListOfLists.GetList(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
		}

		public int GetTileNeighborCount(int tileID)
		{
			return PackedListOfLists.GetListCount(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
		}

		public int GetMaxTileNeighborCountEver(int tileID)
		{
			return PackedListOfLists.GetListCount(this.tileIDToVerts_offsets, this.verts, tileID);
		}

		public bool IsNeighbor(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.Contains(tile2);
		}

		public bool IsNeighborOrSame(int tile1, int tile2)
		{
			return tile1 == tile2 || this.IsNeighbor(tile1, tile2);
		}

		public int GetNeighborId(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.IndexOf(tile2);
		}

		public int GetTileNeighbor(int tileID, int adjacentId)
		{
			this.GetTileNeighbors(tileID, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors[adjacentId];
		}

		public Vector3 GetTileCenter(int tileID)
		{
			int num = (tileID + 1 >= this.tileIDToVerts_offsets.Count) ? this.verts.Count : this.tileIDToVerts_offsets[tileID + 1];
			Vector3 a = Vector3.zero;
			int num2 = 0;
			for (int num3 = this.tileIDToVerts_offsets[tileID]; num3 < num; num3++)
			{
				a += this.verts[num3];
				num2++;
			}
			return a / (float)num2;
		}

		public float TileRadiusToAngle(float radius)
		{
			return this.DistOnSurfaceToAngle(radius * this.averageTileSize);
		}

		public float DistOnSurfaceToAngle(float dist)
		{
			return (float)(dist / 628.31854248046875 * 360.0);
		}

		public float DistanceFromEquatorNormalized(int tile)
		{
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			return Mathf.Abs((float)(tileCenter.y / 100.0));
		}

		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return (float)(sphericalDistance * 100.0 / this.averageTileSize);
		}

		public float ApproxDistanceInTiles(int firstTile, int secondTile)
		{
			Vector3 tileCenter = this.GetTileCenter(firstTile);
			Vector3 tileCenter2 = this.GetTileCenter(secondTile);
			return this.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, tileCenter2.normalized));
		}

		public void OverlayRoad(int fromTile, int toTile, RoadDef roadDef)
		{
			if (roadDef == null)
			{
				Log.ErrorOnce("Attempted to remove road with overlayRoad; not supported", 90292249);
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
						tile.roads.RemoveAll((Predicate<Tile.RoadLink>)((Tile.RoadLink rl) => rl.neighbor == toTile));
						tile2.roads.RemoveAll((Predicate<Tile.RoadLink>)((Tile.RoadLink rl) => rl.neighbor == fromTile));
					}
					if (tile.roads == null)
					{
						tile.roads = new List<Tile.RoadLink>();
					}
					if (tile2.roads == null)
					{
						tile2.roads = new List<Tile.RoadLink>();
					}
					tile.roads.Add(new Tile.RoadLink
					{
						neighbor = toTile,
						road = roadDef
					});
					tile2.roads.Add(new Tile.RoadLink
					{
						neighbor = fromTile,
						road = roadDef
					});
				}
			}
		}

		public RoadDef GetRoadDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			RoadDef result;
			List<Tile.RoadLink> list;
			int i;
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444);
				result = null;
			}
			else
			{
				Tile tile = this.tiles[fromTile];
				list = ((!visibleOnly) ? tile.roads : tile.VisibleRoads);
				if (list == null)
				{
					result = null;
				}
				else
				{
					for (i = 0; i < list.Count; i++)
					{
						Tile.RoadLink roadLink = list[i];
						if (roadLink.neighbor == toTile)
							goto IL_0075;
					}
					result = null;
				}
			}
			goto IL_00a3;
			IL_0075:
			Tile.RoadLink roadLink2 = list[i];
			result = roadLink2.road;
			goto IL_00a3;
			IL_00a3:
			return result;
		}

		public void OverlayRiver(int fromTile, int toTile, RiverDef riverDef)
		{
			if (riverDef == null)
			{
				Log.ErrorOnce("Attempted to remove river with overlayRiver; not supported", 90292250);
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
						tile.rivers.RemoveAll((Predicate<Tile.RiverLink>)((Tile.RiverLink rl) => rl.neighbor == toTile));
						tile2.rivers.RemoveAll((Predicate<Tile.RiverLink>)((Tile.RiverLink rl) => rl.neighbor == fromTile));
					}
					if (tile.rivers == null)
					{
						tile.rivers = new List<Tile.RiverLink>();
					}
					if (tile2.rivers == null)
					{
						tile2.rivers = new List<Tile.RiverLink>();
					}
					tile.rivers.Add(new Tile.RiverLink
					{
						neighbor = toTile,
						river = riverDef
					});
					tile2.rivers.Add(new Tile.RiverLink
					{
						neighbor = fromTile,
						river = riverDef
					});
				}
			}
		}

		public RiverDef GetRiverDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			RiverDef result;
			List<Tile.RiverLink> list;
			int i;
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444);
				result = null;
			}
			else
			{
				Tile tile = this.tiles[fromTile];
				list = ((!visibleOnly) ? tile.rivers : tile.VisibleRivers);
				if (list == null)
				{
					result = null;
				}
				else
				{
					for (i = 0; i < list.Count; i++)
					{
						Tile.RiverLink riverLink = list[i];
						if (riverLink.neighbor == toTile)
							goto IL_0075;
					}
					result = null;
				}
			}
			goto IL_00a3;
			IL_0075:
			Tile.RiverLink riverLink2 = list[i];
			result = riverLink2.river;
			goto IL_00a3;
			IL_00a3:
			return result;
		}

		public float GetRoadMovementMultiplierFast(int fromTile, int toTile)
		{
			List<Tile.RoadLink> roads = this.tiles[fromTile].roads;
			float result;
			int i;
			if (roads == null)
			{
				result = 1f;
			}
			else
			{
				for (i = 0; i < roads.Count; i++)
				{
					Tile.RoadLink roadLink = roads[i];
					if (roadLink.neighbor == toTile)
						goto IL_0041;
				}
				result = 1f;
			}
			goto IL_0078;
			IL_0041:
			Tile.RoadLink roadLink2 = roads[i];
			result = roadLink2.road.movementCostMultiplier;
			goto IL_0078;
			IL_0078:
			return result;
		}

		public int TraversalDistanceBetween(int start, int end)
		{
			int result;
			if (start < 0 || end < 0)
			{
				result = 0;
			}
			else if (this.cachedTraversalDistanceForStart == start && this.cachedTraversalDistanceForEnd == end)
			{
				result = this.cachedTraversalDistance;
			}
			else
			{
				int finalDist = -1;
				Find.WorldFloodFiller.FloodFill(start, (Predicate<int>)((int x) => true), (Func<int, int, bool>)delegate(int tile, int dist)
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
				}, 2147483647, null);
				if (finalDist < 0)
				{
					Log.Error("Could not reach tile " + end + " from " + start);
					result = 0;
				}
				else
				{
					this.cachedTraversalDistance = finalDist;
					this.cachedTraversalDistanceForStart = start;
					this.cachedTraversalDistanceForEnd = end;
					result = finalDist;
				}
			}
			return result;
		}

		public bool IsOnEdge(int tileID)
		{
			return this.InBounds(tileID) && this.GetTileNeighborCount(tileID) < this.GetMaxTileNeighborCountEver(tileID);
		}

		private void CalculateAverageTileSize()
		{
			int tilesCount = this.TilesCount;
			double num = 0.0;
			int num2 = 0;
			for (int num3 = 0; num3 < tilesCount; num3++)
			{
				Vector3 tileCenter = this.GetTileCenter(num3);
				int num4 = (num3 + 1 >= this.tileIDToNeighbors_offsets.Count) ? this.tileIDToNeighbors_values.Count : this.tileIDToNeighbors_offsets[num3 + 1];
				for (int num5 = this.tileIDToNeighbors_offsets[num3]; num5 < num4; num5++)
				{
					int tileID = this.tileIDToNeighbors_values[num5];
					Vector3 tileCenter2 = this.GetTileCenter(tileID);
					num += (double)Vector3.Distance(tileCenter, tileCenter2);
					num2++;
				}
			}
			this.averageTileSize = (float)(num / (double)num2);
		}

		private void CalculateViewCenterAndAngle()
		{
			this.viewAngle = (float)(Find.World.PlanetCoverage * 180.0);
			this.viewCenter = Vector3.back;
			float angle = 45f;
			if (this.viewAngle > 45.0)
			{
				angle = Mathf.Max((float)(90.0 - this.viewAngle), 0f);
			}
			this.viewCenter = Quaternion.AngleAxis(angle, Vector3.right) * this.viewCenter;
		}

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

		public void StandardizeTileData()
		{
			this.TilesToRawData();
			this.RawDataToTiles();
		}

		private void TilesToRawData()
		{
			this.tileBiome = DataSerializeUtility.SerializeUshort(this.TilesCount, (Func<int, ushort>)((int i) => this.tiles[i].biome.shortHash));
			this.tileElevation = DataSerializeUtility.SerializeUshort(this.TilesCount, (Func<int, ushort>)((int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((float)(this.tiles[i].elevation + 8192.0)), 0, 65535)));
			this.tileHilliness = DataSerializeUtility.SerializeByte(this.TilesCount, (Func<int, byte>)((int i) => (byte)this.tiles[i].hilliness));
			this.tileTemperature = DataSerializeUtility.SerializeUshort(this.TilesCount, (Func<int, ushort>)((int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((float)((this.tiles[i].temperature + 300.0) * 10.0)), 0, 65535)));
			this.tileRainfall = DataSerializeUtility.SerializeUshort(this.TilesCount, (Func<int, ushort>)((int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].rainfall), 0, 65535)));
			this.tileSwampiness = DataSerializeUtility.SerializeByte(this.TilesCount, (Func<int, byte>)((int i) => (byte)Mathf.Clamp(Mathf.RoundToInt((float)(this.tiles[i].swampiness * 255.0)), 0, 255)));
			this.tileFeature = DataSerializeUtility.SerializeUshort(this.TilesCount, (Func<int, ushort>)((int i) => (ushort)((this.tiles[i].feature != null) ? ((ushort)this.tiles[i].feature.uniqueID) : 65535)));
			List<int> list = new List<int>();
			List<byte> list2 = new List<byte>();
			List<ushort> list3 = new List<ushort>();
			for (int j = 0; j < this.TilesCount; j++)
			{
				List<Tile.RoadLink> roads = this.tiles[j].roads;
				if (roads != null)
				{
					for (int k = 0; k < roads.Count; k++)
					{
						Tile.RoadLink roadLink = roads[k];
						if (roadLink.neighbor >= j)
						{
							byte b = (byte)this.GetNeighborId(j, roadLink.neighbor);
							if (b < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for road piece", 81637014);
							}
							else
							{
								list.Add(j);
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
			for (int l = 0; l < this.TilesCount; l++)
			{
				List<Tile.RiverLink> rivers = this.tiles[l].rivers;
				if (rivers != null)
				{
					for (int m = 0; m < rivers.Count; m++)
					{
						Tile.RiverLink riverLink = rivers[m];
						if (riverLink.neighbor >= l)
						{
							byte b2 = (byte)this.GetNeighborId(l, riverLink.neighbor);
							if (b2 < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for river piece", 81637014);
							}
							else
							{
								list4.Add(l);
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

		private void RawDataToTiles()
		{
			if (this.tiles.Count != this.TilesCount)
			{
				this.tiles.Clear();
				for (int j = 0; j < this.TilesCount; j++)
				{
					this.tiles.Add(new Tile());
				}
			}
			else
			{
				for (int k = 0; k < this.TilesCount; k++)
				{
					this.tiles[k].roads = null;
					this.tiles[k].rivers = null;
				}
			}
			DataSerializeUtility.LoadUshort(this.tileBiome, this.TilesCount, (Action<int, ushort>)delegate(int i, ushort data)
			{
				this.tiles[i].biome = DefDatabase<BiomeDef>.GetByShortHash(data);
			});
			DataSerializeUtility.LoadUshort(this.tileElevation, this.TilesCount, (Action<int, ushort>)delegate(int i, ushort data)
			{
				this.tiles[i].elevation = (float)(data - 8192);
			});
			DataSerializeUtility.LoadByte(this.tileHilliness, this.TilesCount, (Action<int, byte>)delegate(int i, byte data)
			{
				this.tiles[i].hilliness = (Hilliness)data;
			});
			DataSerializeUtility.LoadUshort(this.tileTemperature, this.TilesCount, (Action<int, ushort>)delegate(int i, ushort data)
			{
				this.tiles[i].temperature = (float)((float)(int)data / 10.0 - 300.0);
			});
			DataSerializeUtility.LoadUshort(this.tileRainfall, this.TilesCount, (Action<int, ushort>)delegate(int i, ushort data)
			{
				this.tiles[i].rainfall = (float)(int)data;
			});
			DataSerializeUtility.LoadByte(this.tileSwampiness, this.TilesCount, (Action<int, byte>)delegate(int i, byte data)
			{
				this.tiles[i].swampiness = (float)((float)(int)data / 255.0);
			});
			int[] array = DataSerializeUtility.DeserializeInt(this.tileRoadOrigins);
			byte[] array2 = DataSerializeUtility.DeserializeByte(this.tileRoadAdjacency);
			ushort[] array3 = DataSerializeUtility.DeserializeUshort(this.tileRoadDef);
			for (int l = 0; l < array.Length; l++)
			{
				int num = array[l];
				int tileNeighbor = this.GetTileNeighbor(num, array2[l]);
				RoadDef byShortHash = DefDatabase<RoadDef>.GetByShortHash(array3[l]);
				if (byShortHash != null)
				{
					if (this.tiles[num].roads == null)
					{
						this.tiles[num].roads = new List<Tile.RoadLink>();
					}
					if (this.tiles[tileNeighbor].roads == null)
					{
						this.tiles[tileNeighbor].roads = new List<Tile.RoadLink>();
					}
					this.tiles[num].roads.Add(new Tile.RoadLink
					{
						neighbor = tileNeighbor,
						road = byShortHash
					});
					this.tiles[tileNeighbor].roads.Add(new Tile.RoadLink
					{
						neighbor = num,
						road = byShortHash
					});
				}
			}
			int[] array4 = DataSerializeUtility.DeserializeInt(this.tileRiverOrigins);
			byte[] array5 = DataSerializeUtility.DeserializeByte(this.tileRiverAdjacency);
			ushort[] array6 = DataSerializeUtility.DeserializeUshort(this.tileRiverDef);
			for (int m = 0; m < array4.Length; m++)
			{
				int num2 = array4[m];
				int tileNeighbor2 = this.GetTileNeighbor(num2, array5[m]);
				RiverDef byShortHash2 = DefDatabase<RiverDef>.GetByShortHash(array6[m]);
				if (byShortHash2 != null)
				{
					if (this.tiles[num2].rivers == null)
					{
						this.tiles[num2].rivers = new List<Tile.RiverLink>();
					}
					if (this.tiles[tileNeighbor2].rivers == null)
					{
						this.tiles[tileNeighbor2].rivers = new List<Tile.RiverLink>();
					}
					this.tiles[num2].rivers.Add(new Tile.RiverLink
					{
						neighbor = tileNeighbor2,
						river = byShortHash2
					});
					this.tiles[tileNeighbor2].rivers.Add(new Tile.RiverLink
					{
						neighbor = num2,
						river = byShortHash2
					});
				}
			}
		}
	}
}
