using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200053F RID: 1343
	public class WorldPathFinder
	{
		// Token: 0x04000EB9 RID: 3769
		private FastPriorityQueue<WorldPathFinder.CostNode> openList;

		// Token: 0x04000EBA RID: 3770
		private WorldPathFinder.PathFinderNodeFast[] calcGrid;

		// Token: 0x04000EBB RID: 3771
		private ushort statusOpenValue = 1;

		// Token: 0x04000EBC RID: 3772
		private ushort statusClosedValue = 2;

		// Token: 0x04000EBD RID: 3773
		private const int SearchLimit = 500000;

		// Token: 0x04000EBE RID: 3774
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

		// Token: 0x04000EBF RID: 3775
		private const float BestRoadDiscount = 0.5f;

		// Token: 0x0600191E RID: 6430 RVA: 0x000DA684 File Offset: 0x000D8A84
		public WorldPathFinder()
		{
			this.calcGrid = new WorldPathFinder.PathFinderNodeFast[Find.WorldGrid.TilesCount];
			this.openList = new FastPriorityQueue<WorldPathFinder.CostNode>(new WorldPathFinder.CostNodeComparer());
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x000DA6C0 File Offset: 0x000D8AC0
		public WorldPath FindPath(int startTile, int destTile, Caravan caravan, Func<float, bool> terminator = null)
		{
			WorldPath notFound;
			if (startTile < 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid start tile ",
					startTile,
					", caravan= ",
					caravan
				}), false);
				notFound = WorldPath.NotFound;
			}
			else if (destTile < 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid dest tile ",
					destTile,
					", caravan= ",
					caravan
				}), false);
				notFound = WorldPath.NotFound;
			}
			else
			{
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
				float[] movementDifficulty = world.pathGrid.movementDifficulty;
				int num = 0;
				int num2 = (caravan == null) ? 3500 : caravan.TicksPerMove;
				int num3 = this.CalculateHeuristicStrength(startTile, destTile);
				this.statusOpenValue += 2;
				this.statusClosedValue += 2;
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
				this.openList.Push(new WorldPathFinder.CostNode(startTile, 0));
				while (this.openList.Count > 0)
				{
					WorldPathFinder.CostNode costNode = this.openList.Pop();
					if (costNode.cost == this.calcGrid[costNode.tile].costNodeCost)
					{
						int tile = costNode.tile;
						if (this.calcGrid[tile].status != this.statusClosedValue)
						{
							if (DebugViewSettings.drawPaths)
							{
								Find.WorldDebugDrawer.FlashTile(tile, (float)this.calcGrid[tile].knownCost / 375000f, this.calcGrid[tile].knownCost.ToString(), 50);
							}
							if (tile == destTile)
							{
								return this.FinalizedPath(tile);
							}
							if (num > 500000)
							{
								Log.Warning(string.Concat(new object[]
								{
									caravan,
									" pathing from ",
									startTile,
									" to ",
									destTile,
									" hit search limit of ",
									500000,
									" tiles."
								}), false);
								return WorldPath.NotFound;
							}
							int num4 = (tile + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[tile + 1];
							for (int i = tileIDToNeighbors_offsets[tile]; i < num4; i++)
							{
								int num5 = tileIDToNeighbors_values[i];
								if (this.calcGrid[num5].status != this.statusClosedValue)
								{
									if (!world.Impassable(num5))
									{
										int num6 = (int)((float)num2 * movementDifficulty[num5] * grid.GetRoadMovementDifficultyMultiplier(tile, num5, null));
										int num7 = num6 + this.calcGrid[tile].knownCost;
										ushort status = this.calcGrid[num5].status;
										if ((status != this.statusClosedValue && status != this.statusOpenValue) || this.calcGrid[num5].knownCost > num7)
										{
											Vector3 tileCenter = grid.GetTileCenter(num5);
											if (status != this.statusClosedValue && status != this.statusOpenValue)
											{
												float num8 = grid.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, normalized));
												this.calcGrid[num5].heuristicCost = Mathf.RoundToInt((float)num2 * num8 * (float)num3 * 0.5f);
											}
											int num9 = num7 + this.calcGrid[num5].heuristicCost;
											this.calcGrid[num5].parentTile = tile;
											this.calcGrid[num5].knownCost = num7;
											this.calcGrid[num5].status = this.statusOpenValue;
											this.calcGrid[num5].costNodeCost = num9;
											this.openList.Push(new WorldPathFinder.CostNode(num5, num9));
										}
									}
								}
							}
							num++;
							this.calcGrid[tile].status = this.statusClosedValue;
							if (terminator != null && terminator((float)this.calcGrid[tile].costNodeCost))
							{
								return WorldPath.NotFound;
							}
						}
					}
				}
				Log.Warning(string.Concat(new object[]
				{
					caravan,
					" pathing from ",
					startTile,
					" to ",
					destTile,
					" ran out of tiles to process."
				}), false);
				notFound = WorldPath.NotFound;
			}
			return notFound;
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x000DAC64 File Offset: 0x000D9064
		public void FloodPathsWithCost(List<int> startTiles, Func<int, int, int> costFunc, Func<int, bool> impassable = null, Func<int, float, bool> terminator = null)
		{
			if (startTiles.Count < 1 || startTiles.Contains(-1))
			{
				Log.Error("Tried to FindPath with invalid start tiles", false);
			}
			else
			{
				World world = Find.World;
				WorldGrid grid = world.grid;
				List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
				List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
				if (impassable == null)
				{
					impassable = ((int tid) => world.Impassable(tid));
				}
				this.statusOpenValue += 2;
				this.statusClosedValue += 2;
				if (this.statusClosedValue >= 65435)
				{
					this.ResetStatuses();
				}
				this.openList.Clear();
				foreach (int num in startTiles)
				{
					this.calcGrid[num].knownCost = 0;
					this.calcGrid[num].costNodeCost = 0;
					this.calcGrid[num].parentTile = num;
					this.calcGrid[num].status = this.statusOpenValue;
					this.openList.Push(new WorldPathFinder.CostNode(num, 0));
				}
				while (this.openList.Count > 0)
				{
					WorldPathFinder.CostNode costNode = this.openList.Pop();
					if (costNode.cost == this.calcGrid[costNode.tile].costNodeCost)
					{
						int tile = costNode.tile;
						if (this.calcGrid[tile].status != this.statusClosedValue)
						{
							int num2 = (tile + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[tile + 1];
							for (int i = tileIDToNeighbors_offsets[tile]; i < num2; i++)
							{
								int num3 = tileIDToNeighbors_values[i];
								if (this.calcGrid[num3].status != this.statusClosedValue)
								{
									if (!impassable(num3))
									{
										int num4 = costFunc(tile, num3);
										int num5 = num4 + this.calcGrid[tile].knownCost;
										ushort status = this.calcGrid[num3].status;
										if ((status != this.statusClosedValue && status != this.statusOpenValue) || this.calcGrid[num3].knownCost > num5)
										{
											int num6 = num5;
											this.calcGrid[num3].parentTile = tile;
											this.calcGrid[num3].knownCost = num5;
											this.calcGrid[num3].status = this.statusOpenValue;
											this.calcGrid[num3].costNodeCost = num6;
											this.openList.Push(new WorldPathFinder.CostNode(num3, num6));
										}
									}
								}
							}
							this.calcGrid[tile].status = this.statusClosedValue;
							if (terminator != null && terminator(tile, (float)this.calcGrid[tile].costNodeCost))
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x000DAFE4 File Offset: 0x000D93E4
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

		// Token: 0x06001922 RID: 6434 RVA: 0x000DB094 File Offset: 0x000D9494
		private WorldPath FinalizedPath(int lastTile)
		{
			WorldPath emptyWorldPath = Find.WorldPathPool.GetEmptyWorldPath();
			int num = lastTile;
			for (;;)
			{
				WorldPathFinder.PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num];
				int parentTile = pathFinderNodeFast.parentTile;
				int num2 = num;
				emptyWorldPath.AddNodeAtStart(num2);
				if (num2 == parentTile)
				{
					break;
				}
				num = parentTile;
			}
			emptyWorldPath.SetupFound((float)this.calcGrid[lastTile].knownCost);
			return emptyWorldPath;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x000DB110 File Offset: 0x000D9510
		private void ResetStatuses()
		{
			int num = this.calcGrid.Length;
			for (int i = 0; i < num; i++)
			{
				this.calcGrid[i].status = 0;
			}
			this.statusOpenValue = 1;
			this.statusClosedValue = 2;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x000DB15C File Offset: 0x000D955C
		private int CalculateHeuristicStrength(int startTile, int destTile)
		{
			float x = Find.WorldGrid.ApproxDistanceInTiles(startTile, destTile);
			return Mathf.RoundToInt(WorldPathFinder.HeuristicStrength_DistanceCurve.Evaluate(x));
		}

		// Token: 0x02000540 RID: 1344
		private struct CostNode
		{
			// Token: 0x04000EC0 RID: 3776
			public int tile;

			// Token: 0x04000EC1 RID: 3777
			public int cost;

			// Token: 0x06001926 RID: 6438 RVA: 0x000DB1EB File Offset: 0x000D95EB
			public CostNode(int tile, int cost)
			{
				this.tile = tile;
				this.cost = cost;
			}
		}

		// Token: 0x02000541 RID: 1345
		private struct PathFinderNodeFast
		{
			// Token: 0x04000EC2 RID: 3778
			public int knownCost;

			// Token: 0x04000EC3 RID: 3779
			public int heuristicCost;

			// Token: 0x04000EC4 RID: 3780
			public int parentTile;

			// Token: 0x04000EC5 RID: 3781
			public int costNodeCost;

			// Token: 0x04000EC6 RID: 3782
			public ushort status;
		}

		// Token: 0x02000542 RID: 1346
		private class CostNodeComparer : IComparer<WorldPathFinder.CostNode>
		{
			// Token: 0x06001928 RID: 6440 RVA: 0x000DB204 File Offset: 0x000D9604
			public int Compare(WorldPathFinder.CostNode a, WorldPathFinder.CostNode b)
			{
				int cost = a.cost;
				int cost2 = b.cost;
				int result;
				if (cost > cost2)
				{
					result = 1;
				}
				else if (cost < cost2)
				{
					result = -1;
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}
	}
}
