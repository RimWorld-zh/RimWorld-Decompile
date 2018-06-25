using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F2D RID: 3885
	public class FloodFiller
	{
		// Token: 0x04003DA3 RID: 15779
		private Map map;

		// Token: 0x04003DA4 RID: 15780
		private bool working;

		// Token: 0x04003DA5 RID: 15781
		private Queue<IntVec3> openSet = new Queue<IntVec3>();

		// Token: 0x04003DA6 RID: 15782
		private IntGrid traversalDistance;

		// Token: 0x04003DA7 RID: 15783
		private CellGrid parentGrid;

		// Token: 0x04003DA8 RID: 15784
		private List<int> visited = new List<int>();

		// Token: 0x06005D3A RID: 23866 RVA: 0x002F2C6F File Offset: 0x002F106F
		public FloodFiller(Map map)
		{
			this.map = map;
			this.traversalDistance = new IntGrid(map);
			this.traversalDistance.Clear(-1);
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x002F2CB0 File Offset: 0x002F10B0
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Action<IntVec3> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			this.FloodFill(root, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				processor(cell);
				return false;
			}, maxCellsToProcess, rememberParents, extraRoots);
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x002F2CE8 File Offset: 0x002F10E8
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Action<IntVec3, int> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			this.FloodFill(root, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				processor(cell, traversalDist);
				return false;
			}, maxCellsToProcess, rememberParents, extraRoots);
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x002F2D20 File Offset: 0x002F1120
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, bool> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			this.FloodFill(root, passCheck, (IntVec3 cell, int traversalDist) => processor(cell), maxCellsToProcess, rememberParents, extraRoots);
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x002F2D58 File Offset: 0x002F1158
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, bool rememberParents = false, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.", false);
			}
			this.working = true;
			this.ClearVisited();
			if (rememberParents && this.parentGrid == null)
			{
				this.parentGrid = new CellGrid(this.map);
			}
			if (root.IsValid && extraRoots == null && !passCheck(root))
			{
				if (rememberParents)
				{
					this.parentGrid[root] = IntVec3.Invalid;
				}
				this.working = false;
			}
			else
			{
				int area = this.map.Area;
				IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
				int num = cardinalDirectionsAround.Length;
				CellIndices cellIndices = this.map.cellIndices;
				int num2 = 0;
				this.openSet.Clear();
				if (root.IsValid)
				{
					int num3 = cellIndices.CellToIndex(root);
					this.visited.Add(num3);
					this.traversalDistance[num3] = 0;
					this.openSet.Enqueue(root);
				}
				if (extraRoots != null)
				{
					IList<IntVec3> list = extraRoots as IList<IntVec3>;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							int num4 = cellIndices.CellToIndex(list[i]);
							this.visited.Add(num4);
							this.traversalDistance[num4] = 0;
							this.openSet.Enqueue(list[i]);
						}
					}
					else
					{
						foreach (IntVec3 intVec in extraRoots)
						{
							int num5 = cellIndices.CellToIndex(intVec);
							this.visited.Add(num5);
							this.traversalDistance[num5] = 0;
							this.openSet.Enqueue(intVec);
						}
					}
				}
				if (rememberParents)
				{
					for (int j = 0; j < this.visited.Count; j++)
					{
						IntVec3 intVec2 = cellIndices.IndexToCell(this.visited[j]);
						this.parentGrid[this.visited[j]] = ((!passCheck(intVec2)) ? IntVec3.Invalid : intVec2);
					}
				}
				while (this.openSet.Count > 0)
				{
					IntVec3 intVec3 = this.openSet.Dequeue();
					int num6 = this.traversalDistance[cellIndices.CellToIndex(intVec3)];
					if (processor(intVec3, num6))
					{
						break;
					}
					num2++;
					if (num2 == maxCellsToProcess)
					{
						break;
					}
					for (int k = 0; k < num; k++)
					{
						IntVec3 intVec4 = intVec3 + cardinalDirectionsAround[k];
						int num7 = cellIndices.CellToIndex(intVec4);
						if (intVec4.InBounds(this.map) && this.traversalDistance[num7] == -1 && passCheck(intVec4))
						{
							this.visited.Add(num7);
							this.openSet.Enqueue(intVec4);
							this.traversalDistance[num7] = num6 + 1;
							if (rememberParents)
							{
								this.parentGrid[num7] = intVec3;
							}
						}
					}
					if (this.openSet.Count > area)
					{
						Log.Error("Overflow on flood fill (>" + area + " cells). Make sure we're not flooding over the same area after we check it.", false);
						this.working = false;
						return;
					}
				}
				this.working = false;
			}
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x002F311C File Offset: 0x002F151C
		public void ReconstructLastFloodFillPath(IntVec3 dest, List<IntVec3> outPath)
		{
			outPath.Clear();
			if (this.parentGrid != null && dest.InBounds(this.map) && this.parentGrid[dest].IsValid)
			{
				int num = 0;
				int num2 = this.map.Area + 1;
				IntVec3 intVec = dest;
				for (;;)
				{
					num++;
					if (num > num2)
					{
						break;
					}
					if (!intVec.IsValid)
					{
						goto Block_4;
					}
					outPath.Add(intVec);
					if (this.parentGrid[intVec] == intVec)
					{
						goto Block_5;
					}
					intVec = this.parentGrid[intVec];
				}
				Log.Error("Too many iterations.", false);
				Block_4:
				Block_5:
				outPath.Reverse();
			}
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x002F31E8 File Offset: 0x002F15E8
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				int index = this.visited[i];
				this.traversalDistance[index] = -1;
				if (this.parentGrid != null)
				{
					this.parentGrid[index] = IntVec3.Invalid;
				}
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}
	}
}
