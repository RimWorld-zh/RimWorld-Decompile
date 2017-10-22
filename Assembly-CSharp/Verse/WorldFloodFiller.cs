using RimWorld.Planet;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class WorldFloodFiller
	{
		private Queue<int> openSet = new Queue<int>();

		private List<int> traversalDistance = new List<int>();

		private List<int> visited = new List<int>();

		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647)
		{
			this.FloodFill(rootTile, passCheck, (Func<int, int, bool>)delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess);
		}

		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647)
		{
			this.FloodFill(rootTile, passCheck, (Func<int, int, bool>)delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess);
		}

		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647)
		{
			this.FloodFill(rootTile, passCheck, (Func<int, int, bool>)((int tile, int traversalDistance) => processor(tile)), maxTilesToProcess);
		}

		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647)
		{
			if (rootTile < 0)
			{
				Log.Error("Flood fill with rootTile=" + rootTile);
			}
			else
			{
				ProfilerThreadCheck.BeginSample("WorldFloodFill");
				this.ClearVisited();
				if (!passCheck(rootTile))
				{
					ProfilerThreadCheck.EndSample();
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
					this.visited.Add(rootTile);
					this.traversalDistance[rootTile] = 0;
					this.openSet.Clear();
					this.openSet.Enqueue(rootTile);
					while (this.openSet.Count > 0)
					{
						int num4 = this.openSet.Dequeue();
						int num5 = this.traversalDistance[num4];
						if (!processor(num4, num5))
						{
							num3++;
							if (num3 != maxTilesToProcess)
							{
								int num6 = (num4 + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[num4 + 1];
								for (int num7 = tileIDToNeighbors_offsets[num4]; num7 < num6; num7++)
								{
									int num8 = tileIDToNeighbors_values[num7];
									if (this.traversalDistance[num8] == -1 && passCheck(num8))
									{
										this.visited.Add(num8);
										this.openSet.Enqueue(num8);
										this.traversalDistance[num8] = num5 + 1;
									}
								}
								if (this.openSet.Count > tilesCount)
								{
									Log.Error("Overflow on world flood fill (>" + tilesCount + " cells). Make sure we're not flooding over the same area after we check it.");
									ProfilerThreadCheck.EndSample();
									return;
								}
								continue;
							}
						}
						break;
					}
					ProfilerThreadCheck.EndSample();
				}
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
