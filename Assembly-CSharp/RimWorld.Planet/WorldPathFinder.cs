using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldPathFinder
	{
		private struct CostNode
		{
			public int tile;

			public int cost;

			public CostNode(int tile, int cost)
			{
				this.tile = tile;
				this.cost = cost;
			}
		}

		private struct PathFinderNodeFast
		{
			public int knownCost;

			public int heuristicCost;

			public int parentTile;

			public int costNodeCost;

			public ushort status;
		}

		private class CostNodeComparer : IComparer<CostNode>
		{
			public int Compare(CostNode a, CostNode b)
			{
				int cost = a.cost;
				int cost2 = b.cost;
				if (cost > cost2)
				{
					return 1;
				}
				if (cost < cost2)
				{
					return -1;
				}
				return 0;
			}
		}

		private const int SearchLimit = 500000;

		private const float BestRoadDiscount = 0.5f;

		private FastPriorityQueue<CostNode> openList;

		private PathFinderNodeFast[] calcGrid;

		private ushort statusOpenValue = (ushort)1;

		private ushort statusClosedValue = (ushort)2;

		private static readonly SimpleCurve HeuristicStrength_DistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(30f, 1f),
				true
			},
			{
				new CurvePoint(40f, 1.3f),
				true
			},
			{
				new CurvePoint(130f, 2f),
				true
			}
		};

		public WorldPathFinder()
		{
			this.calcGrid = new PathFinderNodeFast[Find.WorldGrid.TilesCount];
			this.openList = new FastPriorityQueue<CostNode>(new CostNodeComparer());
		}

		public WorldPath FindPath(int startTile, int destTile, Caravan caravan, Func<float, bool> terminator = null)
		{
			if (startTile < 0)
			{
				Log.Error("Tried to FindPath with invalid start tile " + startTile + ", caravan= " + caravan);
				return WorldPath.NotFound;
			}
			if (destTile < 0)
			{
				Log.Error("Tried to FindPath with invalid dest tile " + destTile + ", caravan= " + caravan);
				return WorldPath.NotFound;
			}
			if (caravan != null)
			{
				if (!caravan.CanReach(destTile))
				{
					return WorldPath.NotFound;
				}
			}
			else if (!Find.WorldReachability.CanReach(startTile, destTile))
			{
				return WorldPath.NotFound;
			}
			World world = Find.World;
			WorldGrid grid = world.grid;
			List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
			Vector3 normalized = grid.GetTileCenter(destTile).normalized;
			int[] pathGrid = world.pathGrid.pathGrid;
			int num = 0;
			int num2 = (caravan == null) ? 2500 : caravan.TicksPerMove;
			int num3 = this.CalculateHeuristicStrength(startTile, destTile);
			this.statusOpenValue = (ushort)(this.statusOpenValue + 2);
			this.statusClosedValue = (ushort)(this.statusClosedValue + 2);
			if (this.statusClosedValue >= 65435)
			{
				this.ResetStatuses();
			}
			this.calcGrid[startTile].knownCost = 0;
			this.calcGrid[startTile].heuristicCost = 0;
			this.calcGrid[startTile].costNodeCost = 0;
			this.calcGrid[startTile].parentTile = startTile;
			this.calcGrid[startTile].status = this.statusOpenValue;
			this.openList.Clear();
			this.openList.Push(new CostNode(startTile, 0));
			while (true)
			{
				if (this.openList.Count <= 0)
				{
					Log.Warning(caravan + " pathing from " + startTile + " to " + destTile + " ran out of tiles to process.");
					return WorldPath.NotFound;
				}
				CostNode costNode = this.openList.Pop();
				if (costNode.cost == this.calcGrid[costNode.tile].costNodeCost)
				{
					int tile = costNode.tile;
					if (this.calcGrid[tile].status != this.statusClosedValue)
					{
						if (DebugViewSettings.drawPaths)
						{
							Find.WorldDebugDrawer.FlashTile(tile, (float)((float)this.calcGrid[tile].knownCost / 375000.0), this.calcGrid[tile].knownCost.ToString());
						}
						if (tile == destTile)
						{
							return this.FinalizedPath(tile);
						}
						if (num > 500000)
						{
							Log.Warning(caravan + " pathing from " + startTile + " to " + destTile + " hit search limit of " + 500000 + " tiles.");
							return WorldPath.NotFound;
						}
						int num4 = (tile + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[tile + 1];
						for (int i = tileIDToNeighbors_offsets[tile]; i < num4; i++)
						{
							int num5 = tileIDToNeighbors_values[i];
							int num7;
							ushort status;
							if (this.calcGrid[num5].status != this.statusClosedValue && !world.Impassable(num5))
							{
								int num6 = num2;
								num6 += pathGrid[num5];
								num6 = (int)((float)num6 * grid.GetRoadMovementMultiplierFast(tile, num5));
								num7 = num6 + this.calcGrid[tile].knownCost;
								status = this.calcGrid[num5].status;
								if (status != this.statusClosedValue && status != this.statusOpenValue)
								{
									goto IL_0416;
								}
								if (this.calcGrid[num5].knownCost > num7)
									goto IL_0416;
							}
							continue;
							IL_0416:
							Vector3 tileCenter = grid.GetTileCenter(num5);
							if (status != this.statusClosedValue && status != this.statusOpenValue)
							{
								float num8 = grid.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, normalized));
								this.calcGrid[num5].heuristicCost = Mathf.RoundToInt((float)((float)num2 * num8 * (float)num3 * 0.5));
							}
							int num9 = num7 + this.calcGrid[num5].heuristicCost;
							this.calcGrid[num5].parentTile = tile;
							this.calcGrid[num5].knownCost = num7;
							this.calcGrid[num5].status = this.statusOpenValue;
							this.calcGrid[num5].costNodeCost = num9;
							this.openList.Push(new CostNode(num5, num9));
						}
						num++;
						this.calcGrid[tile].status = this.statusClosedValue;
						if ((object)terminator != null && terminator((float)this.calcGrid[tile].costNodeCost))
							break;
					}
				}
			}
			return WorldPath.NotFound;
		}

		public void FloodPathsWithCost(List<int> startTiles, Func<int, int, int> costFunc, Func<int, bool> impassable = null, Func<int, float, bool> terminator = null)
		{
			if (startTiles.Count < 1 || startTiles.Any((Predicate<int>)((int st) => st == -1)))
			{
				Log.Error("Tried to FindPath with invalid start tiles");
			}
			else
			{
				World world = Find.World;
				WorldGrid grid = world.grid;
				List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
				List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
				if ((object)impassable == null)
				{
					impassable = (Func<int, bool>)((int tid) => world.Impassable(tid));
				}
				this.statusOpenValue = (ushort)(this.statusOpenValue + 2);
				this.statusClosedValue = (ushort)(this.statusClosedValue + 2);
				if (this.statusClosedValue >= 65435)
				{
					this.ResetStatuses();
				}
				this.openList.Clear();
				List<int>.Enumerator enumerator = startTiles.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						this.calcGrid[current].knownCost = 0;
						this.calcGrid[current].costNodeCost = 0;
						this.calcGrid[current].parentTile = current;
						this.calcGrid[current].status = this.statusOpenValue;
						this.openList.Push(new CostNode(current, 0));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				while (this.openList.Count > 0)
				{
					CostNode costNode = this.openList.Pop();
					if (costNode.cost == this.calcGrid[costNode.tile].costNodeCost)
					{
						int tile = costNode.tile;
						if (this.calcGrid[tile].status != this.statusClosedValue)
						{
							int num = (tile + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[tile + 1];
							for (int i = tileIDToNeighbors_offsets[tile]; i < num; i++)
							{
								int num2 = tileIDToNeighbors_values[i];
								int num4;
								if (this.calcGrid[num2].status != this.statusClosedValue && !impassable(num2))
								{
									int num3 = costFunc(tile, num2);
									num4 = num3 + this.calcGrid[tile].knownCost;
									ushort status = this.calcGrid[num2].status;
									if (status != this.statusClosedValue && status != this.statusOpenValue)
									{
										goto IL_029a;
									}
									if (this.calcGrid[num2].knownCost > num4)
										goto IL_029a;
								}
								continue;
								IL_029a:
								int num5 = num4;
								this.calcGrid[num2].parentTile = tile;
								this.calcGrid[num2].knownCost = num4;
								this.calcGrid[num2].status = this.statusOpenValue;
								this.calcGrid[num2].costNodeCost = num5;
								this.openList.Push(new CostNode(num2, num5));
							}
							this.calcGrid[tile].status = this.statusClosedValue;
							if ((object)terminator != null && terminator(tile, (float)this.calcGrid[tile].costNodeCost))
								break;
						}
					}
				}
			}
		}

		public List<int>[] FloodPathsWithCostForTree(List<int> startTiles, Func<int, int, int> costFunc, Func<int, bool> impassable = null, Func<int, float, bool> terminator = null)
		{
			this.FloodPathsWithCost(startTiles, costFunc, impassable, terminator);
			World world = Find.World;
			WorldGrid grid = world.grid;
			List<int>[] array = new List<int>[grid.TilesCount];
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (this.calcGrid[i].status == this.statusClosedValue)
				{
					int parentTile = this.calcGrid[i].parentTile;
					if (parentTile != i)
					{
						if (array[parentTile] == null)
						{
							array[parentTile] = new List<int>();
						}
						array[parentTile].Add(i);
					}
				}
			}
			return array;
		}

		private WorldPath FinalizedPath(int lastTile)
		{
			WorldPath emptyWorldPath = Find.WorldPathPool.GetEmptyWorldPath();
			int num = lastTile;
			while (true)
			{
				PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num];
				int parentTile = pathFinderNodeFast.parentTile;
				int num2 = num;
				emptyWorldPath.AddNode(num2);
				if (num2 != parentTile)
				{
					num = parentTile;
					continue;
				}
				break;
			}
			emptyWorldPath.SetupFound((float)this.calcGrid[lastTile].knownCost);
			return emptyWorldPath;
		}

		private void ResetStatuses()
		{
			int num = this.calcGrid.Length;
			for (int num2 = 0; num2 < num; num2++)
			{
				this.calcGrid[num2].status = (ushort)0;
			}
			this.statusOpenValue = (ushort)1;
			this.statusClosedValue = (ushort)2;
		}

		private int CalculateHeuristicStrength(int startTile, int destTile)
		{
			float x = Find.WorldGrid.ApproxDistanceInTiles(startTile, destTile);
			return Mathf.RoundToInt(WorldPathFinder.HeuristicStrength_DistanceCurve.Evaluate(x));
		}

		public static int StandardPathCost(int curTile, int neigh, Caravan caravan)
		{
			int num = (caravan == null) ? 2500 : caravan.TicksPerMove;
			num += Find.World.pathGrid.pathGrid[neigh];
			return (int)((float)num * Find.WorldGrid.GetRoadMovementMultiplierFast(curTile, neigh));
		}
	}
}
