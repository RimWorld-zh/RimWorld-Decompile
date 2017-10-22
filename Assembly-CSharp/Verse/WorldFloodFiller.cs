using RimWorld.Planet;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class WorldFloodFiller
	{
		private bool working;

		private Queue<int> openSet = new Queue<int>();

		private List<int> traversalDistance = new List<int>();

		private List<int> visited = new List<int>();

		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (Func<int, int, bool>)delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (Func<int, int, bool>)delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (Func<int, int, bool>)((int tile, int traversalDistance) => processor(tile)), maxTilesToProcess, extraRootTiles);
		}

		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.working = true;
			this.ClearVisited();
			if (rootTile != -1 && extraRootTiles == null && !passCheck(rootTile))
			{
				this.working = false;
			}
			else
			{
				int tilesCount;
				int num = tilesCount = Find.WorldGrid.TilesCount;
				if (this.traversalDistance.Count != num)
				{
					this.traversalDistance.Clear();
					for (int num2 = 0; num2 < num; num2++)
					{
						this.traversalDistance.Add(-1);
					}
				}
				WorldGrid worldGrid = Find.WorldGrid;
				List<int> tileIDToNeighbors_offsets = worldGrid.tileIDToNeighbors_offsets;
				List<int> tileIDToNeighbors_values = worldGrid.tileIDToNeighbors_values;
				int num3 = 0;
				this.openSet.Clear();
				if (rootTile != -1)
				{
					this.visited.Add(rootTile);
					this.traversalDistance[rootTile] = 0;
					this.openSet.Enqueue(rootTile);
				}
				if (extraRootTiles != null)
				{
					this.visited.AddRange(extraRootTiles);
					IList<int> list = extraRootTiles as IList<int>;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							int num4 = list[i];
							this.traversalDistance[num4] = 0;
							this.openSet.Enqueue(num4);
						}
					}
					else
					{
						foreach (int item in extraRootTiles)
						{
							this.traversalDistance[item] = 0;
							this.openSet.Enqueue(item);
						}
					}
				}
				while (this.openSet.Count > 0)
				{
					int num5 = this.openSet.Dequeue();
					int num6 = this.traversalDistance[num5];
					if (!processor(num5, num6))
					{
						num3++;
						if (num3 != maxTilesToProcess)
						{
							int num7 = (num5 + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[num5 + 1];
							for (int num8 = tileIDToNeighbors_offsets[num5]; num8 < num7; num8++)
							{
								int num9 = tileIDToNeighbors_values[num8];
								if (this.traversalDistance[num9] == -1 && passCheck(num9))
								{
									this.visited.Add(num9);
									this.openSet.Enqueue(num9);
									this.traversalDistance[num9] = num6 + 1;
								}
							}
							if (this.openSet.Count > tilesCount)
							{
								Log.Error("Overflow on world flood fill (>" + tilesCount + " cells). Make sure we're not flooding over the same area after we check it.");
								this.working = false;
								return;
							}
							continue;
						}
					}
					break;
				}
				this.working = false;
			}
		}

		private void ClearVisited()
		{
			int num = 0;
			int count = this.visited.Count;
			while (num < count)
			{
				this.traversalDistance[this.visited[num]] = -1;
				num++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}
	}
}
