using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020005B1 RID: 1457
	public class WorldFloodFiller
	{
		// Token: 0x06001BDB RID: 7131 RVA: 0x000EFA8C File Offset: 0x000EDE8C
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000EFAC0 File Offset: 0x000EDEC0
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x000EFAF4 File Offset: 0x000EDEF4
		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (int tile, int traversalDistance) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000EFB28 File Offset: 0x000EDF28
		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.", false);
			}
			this.working = true;
			this.ClearVisited();
			if (rootTile != -1 && extraRootTiles == null && !passCheck(rootTile))
			{
				this.working = false;
			}
			else
			{
				int tilesCount = Find.WorldGrid.TilesCount;
				int num = tilesCount;
				if (this.traversalDistance.Count != tilesCount)
				{
					this.traversalDistance.Clear();
					for (int i = 0; i < tilesCount; i++)
					{
						this.traversalDistance.Add(-1);
					}
				}
				WorldGrid worldGrid = Find.WorldGrid;
				List<int> tileIDToNeighbors_offsets = worldGrid.tileIDToNeighbors_offsets;
				List<int> tileIDToNeighbors_values = worldGrid.tileIDToNeighbors_values;
				int num2 = 0;
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
						for (int j = 0; j < list.Count; j++)
						{
							int num3 = list[j];
							this.traversalDistance[num3] = 0;
							this.openSet.Enqueue(num3);
						}
					}
					else
					{
						foreach (int num4 in extraRootTiles)
						{
							this.traversalDistance[num4] = 0;
							this.openSet.Enqueue(num4);
						}
					}
				}
				while (this.openSet.Count > 0)
				{
					int num5 = this.openSet.Dequeue();
					int num6 = this.traversalDistance[num5];
					if (processor(num5, num6))
					{
						break;
					}
					num2++;
					if (num2 == maxTilesToProcess)
					{
						break;
					}
					int num7 = (num5 + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[num5 + 1];
					for (int k = tileIDToNeighbors_offsets[num5]; k < num7; k++)
					{
						int num8 = tileIDToNeighbors_values[k];
						if (this.traversalDistance[num8] == -1 && passCheck(num8))
						{
							this.visited.Add(num8);
							this.openSet.Enqueue(num8);
							this.traversalDistance[num8] = num6 + 1;
						}
					}
					if (this.openSet.Count > num)
					{
						Log.Error("Overflow on world flood fill (>" + num + " cells). Make sure we're not flooding over the same area after we check it.", false);
						this.working = false;
						return;
					}
				}
				this.working = false;
			}
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000EFE34 File Offset: 0x000EE234
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				this.traversalDistance[this.visited[i]] = -1;
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		// Token: 0x04001096 RID: 4246
		private bool working;

		// Token: 0x04001097 RID: 4247
		private Queue<int> openSet = new Queue<int>();

		// Token: 0x04001098 RID: 4248
		private List<int> traversalDistance = new List<int>();

		// Token: 0x04001099 RID: 4249
		private List<int> visited = new List<int>();
	}
}
