using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A9D RID: 2717
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x04002639 RID: 9785
		private Map map;

		// Token: 0x0400263A RID: 9786
		private IntVec3 endCell;

		// Token: 0x0400263B RID: 9787
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x0400263C RID: 9788
		private int moveTicksCardinal;

		// Token: 0x0400263D RID: 9789
		private int moveTicksDiagonal;

		// Token: 0x0400263E RID: 9790
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x0400263F RID: 9791
		private Region cachedRegion;

		// Token: 0x04002640 RID: 9792
		private RegionLink cachedBestLink;

		// Token: 0x04002641 RID: 9793
		private RegionLink cachedSecondBestLink;

		// Token: 0x04002642 RID: 9794
		private int cachedBestLinkCost;

		// Token: 0x04002643 RID: 9795
		private int cachedSecondBestLinkCost;

		// Token: 0x04002644 RID: 9796
		private int cachedRegionCellPathCost;

		// Token: 0x04002645 RID: 9797
		private bool cachedRegionIsDestination;

		// Token: 0x04002646 RID: 9798
		private Region[] regionGrid;

		// Token: 0x06003C82 RID: 15490 RVA: 0x001FFD0A File Offset: 0x001FE10A
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x001FFD34 File Offset: 0x001FE134
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

		// Token: 0x06003C84 RID: 15492 RVA: 0x001FFEC4 File Offset: 0x001FE2C4
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

		// Token: 0x06003C85 RID: 15493 RVA: 0x00200014 File Offset: 0x001FE414
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00200070 File Offset: 0x001FE470
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}
	}
}
