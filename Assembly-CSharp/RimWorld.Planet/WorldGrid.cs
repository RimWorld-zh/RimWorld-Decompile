using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGrid
	{
		private const int SubdivisionsCount = 10;

		public const float PlanetRadius = 100f;

		public List<Tile> tiles = new List<Tile>();

		public List<Vector3> verts;

		public List<int> tileIDToVerts_offsets;

		public List<int> tileIDToNeighbors_offsets;

		public List<int> tileIDToNeighbors_values;

		public float averageTileSize;

		public Vector3 viewCenter;

		public float viewAngle;

		private List<int> tileIndices;

		private static List<int> tmpNeighbors = new List<int>();

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

		public List<int> TileIndices
		{
			get
			{
				if (this.tileIndices == null)
				{
					int tilesCount = this.TilesCount;
					this.tileIndices = new List<int>();
					for (int num = 0; num < tilesCount; num++)
					{
						this.tileIndices.Add(num);
					}
				}
				return this.tileIndices;
			}
		}

		public Tile this[int tileID]
		{
			get
			{
				return (tileID < 0 || tileID >= this.tiles.Count) ? null : this.tiles[tileID];
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
			if (from == to)
			{
				return 0f;
			}
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
			return num;
		}

		public float GetHeadingFromTo(int fromTileID, int toTileID)
		{
			if (fromTileID == toTileID)
			{
				return 0f;
			}
			Vector3 tileCenter = this.GetTileCenter(fromTileID);
			Vector3 tileCenter2 = this.GetTileCenter(toTileID);
			return this.GetHeadingFromTo(tileCenter, tileCenter2);
		}

		public Direction8Way GetDirection8WayFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (!(headingFromTo >= 337.5) && !(headingFromTo < 22.5))
			{
				if (headingFromTo < 67.5)
				{
					return Direction8Way.NorthEast;
				}
				if (headingFromTo < 112.5)
				{
					return Direction8Way.East;
				}
				if (headingFromTo < 157.5)
				{
					return Direction8Way.SouthEast;
				}
				if (headingFromTo < 202.5)
				{
					return Direction8Way.South;
				}
				if (headingFromTo < 247.5)
				{
					return Direction8Way.SouthWest;
				}
				if (headingFromTo < 292.5)
				{
					return Direction8Way.West;
				}
				return Direction8Way.NorthWest;
			}
			return Direction8Way.North;
		}

		public Rot4 GetRotFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (!(headingFromTo >= 315.0) && !(headingFromTo < 45.0))
			{
				if (headingFromTo < 135.0)
				{
					return Rot4.East;
				}
				if (headingFromTo < 225.0)
				{
					return Rot4.South;
				}
				return Rot4.West;
			}
			return Rot4.North;
		}

		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			outVertsIndices.Clear();
			int num = this.tileIDToVerts_offsets[tileID];
			int num2 = this.verts.Count;
			if (tileID + 1 < this.tileIDToVerts_offsets.Count)
			{
				num2 = this.tileIDToVerts_offsets[tileID + 1];
			}
			for (int num3 = num; num3 < num2; num3++)
			{
				outVertsIndices.Add(num3);
			}
		}

		public void GetTileNeighbors(int tileID, List<int> outNeighbors)
		{
			PackedListOfLists.GetList(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
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
			return (float)(radius / (628.31854248046875 / this.averageTileSize) * 360.0);
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
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444);
				return null;
			}
			Tile tile = this.tiles[fromTile];
			List<Tile.RoadLink> list = (!visibleOnly) ? tile.roads : tile.VisibleRoads;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				Tile.RoadLink roadLink = list[i];
				if (roadLink.neighbor == toTile)
				{
					Tile.RoadLink roadLink2 = list[i];
					return roadLink2.road;
				}
			}
			return null;
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
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444);
				return null;
			}
			Tile tile = this.tiles[fromTile];
			List<Tile.RiverLink> list = (!visibleOnly) ? tile.rivers : tile.VisibleRivers;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				Tile.RiverLink riverLink = list[i];
				if (riverLink.neighbor == toTile)
				{
					Tile.RiverLink riverLink2 = list[i];
					return riverLink2.river;
				}
			}
			return null;
		}

		public float GetRoadMovementMultiplierFast(int fromTile, int toTile)
		{
			List<Tile.RoadLink> roads = this.tiles[fromTile].roads;
			if (roads == null)
			{
				return 1f;
			}
			for (int i = 0; i < roads.Count; i++)
			{
				Tile.RoadLink roadLink = roads[i];
				if (roadLink.neighbor == toTile)
				{
					Tile.RoadLink roadLink2 = roads[i];
					return roadLink2.road.movementCostMultiplier;
				}
			}
			return 1f;
		}

		public int TraversalDistanceBetween(int start, int end)
		{
			if (start >= 0 && end >= 0)
			{
				if (this.cachedTraversalDistanceForStart == start && this.cachedTraversalDistanceForEnd == end)
				{
					return this.cachedTraversalDistance;
				}
				int finalDist = -1;
				Find.WorldFloodFiller.FloodFill(start, (Predicate<int>)((int x) => true), (Func<int, int, bool>)delegate(int tile, int dist)
				{
					if (tile == end)
					{
						finalDist = dist;
						return true;
					}
					return false;
				}, 2147483647);
				if (finalDist < 0)
				{
					Log.Error("Could not reach tile " + end + " from " + start);
					return 0;
				}
				this.cachedTraversalDistance = finalDist;
				this.cachedTraversalDistanceForStart = start;
				this.cachedTraversalDistanceForEnd = end;
				return finalDist;
			}
			return 0;
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
	}
}
