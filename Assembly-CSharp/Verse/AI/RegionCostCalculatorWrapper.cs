using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A9F RID: 2719
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x06003C83 RID: 15491 RVA: 0x001FF8BA File Offset: 0x001FDCBA
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x001FF8E4 File Offset: 0x001FDCE4
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

		// Token: 0x06003C85 RID: 15493 RVA: 0x001FFA74 File Offset: 0x001FDE74
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

		// Token: 0x06003C86 RID: 15494 RVA: 0x001FFBC4 File Offset: 0x001FDFC4
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x001FFC20 File Offset: 0x001FE020
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}

		// Token: 0x0400263D RID: 9789
		private Map map;

		// Token: 0x0400263E RID: 9790
		private IntVec3 endCell;

		// Token: 0x0400263F RID: 9791
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x04002640 RID: 9792
		private int moveTicksCardinal;

		// Token: 0x04002641 RID: 9793
		private int moveTicksDiagonal;

		// Token: 0x04002642 RID: 9794
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x04002643 RID: 9795
		private Region cachedRegion;

		// Token: 0x04002644 RID: 9796
		private RegionLink cachedBestLink;

		// Token: 0x04002645 RID: 9797
		private RegionLink cachedSecondBestLink;

		// Token: 0x04002646 RID: 9798
		private int cachedBestLinkCost;

		// Token: 0x04002647 RID: 9799
		private int cachedSecondBestLinkCost;

		// Token: 0x04002648 RID: 9800
		private int cachedRegionCellPathCost;

		// Token: 0x04002649 RID: 9801
		private bool cachedRegionIsDestination;

		// Token: 0x0400264A RID: 9802
		private Region[] regionGrid;
	}
}
