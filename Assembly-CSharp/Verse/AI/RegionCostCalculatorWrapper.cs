using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A9E RID: 2718
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x04002649 RID: 9801
		private Map map;

		// Token: 0x0400264A RID: 9802
		private IntVec3 endCell;

		// Token: 0x0400264B RID: 9803
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x0400264C RID: 9804
		private int moveTicksCardinal;

		// Token: 0x0400264D RID: 9805
		private int moveTicksDiagonal;

		// Token: 0x0400264E RID: 9806
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x0400264F RID: 9807
		private Region cachedRegion;

		// Token: 0x04002650 RID: 9808
		private RegionLink cachedBestLink;

		// Token: 0x04002651 RID: 9809
		private RegionLink cachedSecondBestLink;

		// Token: 0x04002652 RID: 9810
		private int cachedBestLinkCost;

		// Token: 0x04002653 RID: 9811
		private int cachedSecondBestLinkCost;

		// Token: 0x04002654 RID: 9812
		private int cachedRegionCellPathCost;

		// Token: 0x04002655 RID: 9813
		private bool cachedRegionIsDestination;

		// Token: 0x04002656 RID: 9814
		private Region[] regionGrid;

		// Token: 0x06003C83 RID: 15491 RVA: 0x00200036 File Offset: 0x001FE436
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x00200060 File Offset: 0x001FE460
		public void Init(CellRect end, TraverseParms traverseParms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted, List<int> disallowedCorners)
		{
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.endCell = end.CenterCell;
			this.cachedRegion = null;
			this.cachedBestLink = null;
			this.cachedSecondBestLink = null;
			this.cachedBestLinkCost = 0;
			this.cachedSecondBestLinkCost = 0;
			this.cachedRegionCellPathCost = 0;
			this.cachedRegionIsDestination = false;
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.destRegions.Clear();
			if (end.Width == 1 && end.Height == 1)
			{
				Region region = this.endCell.GetRegion(this.map, RegionType.Set_Passable);
				if (region != null)
				{
					this.destRegions.Add(region);
				}
			}
			else
			{
				CellRect.CellRectIterator iterator = end.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 intVec = iterator.Current;
					if (intVec.InBounds(this.map) && !disallowedCorners.Contains(this.map.cellIndices.CellToIndex(intVec)))
					{
						Region region2 = intVec.GetRegion(this.map, RegionType.Set_Passable);
						if (region2 != null)
						{
							if (region2.Allows(traverseParms, true))
							{
								this.destRegions.Add(region2);
							}
						}
					}
					iterator.MoveNext();
				}
			}
			if (this.destRegions.Count == 0)
			{
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.", false);
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x002001F0 File Offset: 0x001FE5F0
		public int GetPathCostFromDestToRegion(int cellIndex)
		{
			Region region = this.regionGrid[cellIndex];
			IntVec3 cell = this.map.cellIndices.IndexToCell(cellIndex);
			if (region != this.cachedRegion)
			{
				this.cachedRegionIsDestination = this.destRegions.Contains(region);
				if (this.cachedRegionIsDestination)
				{
					return this.OctileDistanceToEnd(cell);
				}
				this.cachedBestLinkCost = this.regionCostCalculator.GetRegionBestDistances(region, out this.cachedBestLink, out this.cachedSecondBestLink, out this.cachedSecondBestLinkCost);
				this.cachedRegionCellPathCost = this.regionCostCalculator.RegionMedianPathCost(region);
				this.cachedRegion = region;
			}
			else if (this.cachedRegionIsDestination)
			{
				return this.OctileDistanceToEnd(cell);
			}
			int result;
			if (this.cachedBestLink != null)
			{
				int num = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedBestLink, this.cachedRegionCellPathCost);
				int num3;
				if (this.cachedSecondBestLink != null)
				{
					int num2 = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedSecondBestLink, this.cachedRegionCellPathCost);
					num3 = Mathf.Min(this.cachedSecondBestLinkCost + num2, this.cachedBestLinkCost + num);
				}
				else
				{
					num3 = this.cachedBestLinkCost + num;
				}
				num3 += this.OctileDistanceToEndEps(cell);
				result = num3;
			}
			else
			{
				result = 10000;
			}
			return result;
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00200340 File Offset: 0x001FE740
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x0020039C File Offset: 0x001FE79C
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}
	}
}
