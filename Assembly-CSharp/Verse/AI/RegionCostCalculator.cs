using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A9A RID: 2714
	public class RegionCostCalculator
	{
		// Token: 0x04002620 RID: 9760
		private Map map;

		// Token: 0x04002621 RID: 9761
		private Region[] regionGrid;

		// Token: 0x04002622 RID: 9762
		private TraverseParms traverseParms;

		// Token: 0x04002623 RID: 9763
		private IntVec3 destinationCell;

		// Token: 0x04002624 RID: 9764
		private int moveTicksCardinal;

		// Token: 0x04002625 RID: 9765
		private int moveTicksDiagonal;

		// Token: 0x04002626 RID: 9766
		private ByteGrid avoidGrid;

		// Token: 0x04002627 RID: 9767
		private Area allowedArea;

		// Token: 0x04002628 RID: 9768
		private bool drafted;

		// Token: 0x04002629 RID: 9769
		private Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		// Token: 0x0400262A RID: 9770
		private Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		// Token: 0x0400262B RID: 9771
		private Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		// Token: 0x0400262C RID: 9772
		private FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry>(new RegionCostCalculator.DistanceComparer());

		// Token: 0x0400262D RID: 9773
		private Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		// Token: 0x0400262E RID: 9774
		private List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		// Token: 0x0400262F RID: 9775
		private Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		// Token: 0x04002630 RID: 9776
		private const int SampleCount = 11;

		// Token: 0x04002631 RID: 9777
		private static int[] pathCostSamples = new int[11];

		// Token: 0x04002632 RID: 9778
		private static List<int> tmpCellIndices = new List<int>();

		// Token: 0x04002633 RID: 9779
		private static Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		// Token: 0x04002634 RID: 9780
		private static List<int> tmpPathableNeighborIndices = new List<int>();

		// Token: 0x06003C66 RID: 15462 RVA: 0x001FED20 File Offset: 0x001FD120
		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = new Func<int, int, float>(this.PreciseRegionLinkDistancesDistanceGetter);
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x001FED94 File Offset: 0x001FD194
		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.destinationCell = destination.CenterCell;
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.avoidGrid = avoidGrid;
			this.allowedArea = allowedArea;
			this.drafted = drafted;
			this.regionMinLink.Clear();
			this.distances.Clear();
			this.linkTargetCells.Clear();
			this.queue.Clear();
			this.minPathCosts.Clear();
			foreach (Region region in destRegions)
			{
				int minPathCost = this.RegionMedianPathCost(region);
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
					{
						int num = this.RegionLinkDistance(this.destinationCell, regionLink, minPathCost);
						int num2;
						if (this.distances.TryGetValue(regionLink, out num2))
						{
							if (num < num2)
							{
								this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
							}
							num = Math.Min(num2, num);
						}
						else
						{
							this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
						}
						this.distances[regionLink] = num;
					}
				}
				this.GetPreciseRegionLinkDistances(region, destination, this.preciseRegionLinkDistances);
				for (int j = 0; j < this.preciseRegionLinkDistances.Count; j++)
				{
					Pair<RegionLink, int> pair = this.preciseRegionLinkDistances[j];
					RegionLink first = pair.First;
					int num3 = this.distances[first];
					int num4;
					if (pair.Second > num3)
					{
						this.distances[first] = pair.Second;
						num4 = pair.Second;
					}
					else
					{
						num4 = num3;
					}
					this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(region, first, num4, num4));
				}
			}
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x001FEFF8 File Offset: 0x001FD3F8
		public int GetRegionDistance(Region region, out RegionLink minLink)
		{
			int result;
			if (this.regionMinLink.TryGetValue(region.id, out minLink))
			{
				result = this.distances[minLink];
			}
			else
			{
				while (this.queue.Count != 0)
				{
					RegionCostCalculator.RegionLinkQueueEntry regionLinkQueueEntry = this.queue.Pop();
					int num = this.distances[regionLinkQueueEntry.Link];
					if (regionLinkQueueEntry.Cost == num)
					{
						Region otherRegion = regionLinkQueueEntry.Link.GetOtherRegion(regionLinkQueueEntry.From);
						if (otherRegion != null && otherRegion.valid)
						{
							int num2 = 0;
							if (otherRegion.portal != null)
							{
								num2 = PathFinder.GetBuildingCost(otherRegion.portal, this.traverseParms, this.traverseParms.pawn);
								if (num2 == 2147483647)
								{
									continue;
								}
								num2 += this.OctileDistance(1, 0);
							}
							int minPathCost = this.RegionMedianPathCost(otherRegion);
							for (int i = 0; i < otherRegion.links.Count; i++)
							{
								RegionLink regionLink = otherRegion.links[i];
								if (regionLink != regionLinkQueueEntry.Link && regionLink.GetOtherRegion(otherRegion).type.Passable())
								{
									int num3 = (otherRegion.portal == null) ? this.RegionLinkDistance(regionLinkQueueEntry.Link, regionLink, minPathCost) : num2;
									num3 = Math.Max(num3, 1);
									int num4 = num + num3;
									int estimatedPathCost = this.MinimumRegionLinkDistance(this.destinationCell, regionLink) + num4;
									int num5;
									if (this.distances.TryGetValue(regionLink, out num5))
									{
										if (num4 < num5)
										{
											this.distances[regionLink] = num4;
											this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
										}
									}
									else
									{
										this.distances.Add(regionLink, num4);
										this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
									}
								}
							}
							if (!this.regionMinLink.ContainsKey(otherRegion.id))
							{
								this.regionMinLink.Add(otherRegion.id, regionLinkQueueEntry.Link);
								if (otherRegion == region)
								{
									minLink = regionLinkQueueEntry.Link;
									return regionLinkQueueEntry.Cost;
								}
							}
						}
					}
				}
				result = 10000;
			}
			return result;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x001FF268 File Offset: 0x001FD668
		public int GetRegionBestDistances(Region region, out RegionLink bestLink, out RegionLink secondBestLink, out int secondBestCost)
		{
			int regionDistance = this.GetRegionDistance(region, out bestLink);
			secondBestLink = null;
			secondBestCost = int.MaxValue;
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink != bestLink && regionLink.GetOtherRegion(region).type.Passable())
				{
					int num;
					if (this.distances.TryGetValue(regionLink, out num) && num < secondBestCost)
					{
						secondBestCost = num;
						secondBestLink = regionLink;
					}
				}
			}
			return regionDistance;
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x001FF308 File Offset: 0x001FD708
		public int RegionMedianPathCost(Region region)
		{
			int num;
			int result;
			if (this.minPathCosts.TryGetValue(region, out num))
			{
				result = num;
			}
			else
			{
				bool ignoreAllowedAreaCost = this.allowedArea != null && region.OverlapWith(this.allowedArea) != AreaOverlap.None;
				CellIndices cellIndices = this.map.cellIndices;
				Rand.PushState();
				Rand.Seed = cellIndices.CellToIndex(region.extentsClose.CenterCell) * (region.links.Count + 1);
				for (int i = 0; i < 11; i++)
				{
					RegionCostCalculator.pathCostSamples[i] = this.GetCellCostFast(cellIndices.CellToIndex(region.RandomCell), ignoreAllowedAreaCost);
				}
				Rand.PopState();
				Array.Sort<int>(RegionCostCalculator.pathCostSamples);
				int num2 = RegionCostCalculator.pathCostSamples[4];
				this.minPathCosts[region] = num2;
				result = num2;
			}
			return result;
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x001FF3EC File Offset: 0x001FD7EC
		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.map.pathGrid.pathGrid[index];
			if (this.avoidGrid != null)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedArea != null && !ignoreAllowedAreaCost && !this.allowedArea[index])
			{
				num += 600;
			}
			if (this.drafted)
			{
				num += this.map.terrainGrid.topGrid[index].extraDraftedPerceivedPathCost;
			}
			else
			{
				num += this.map.terrainGrid.topGrid[index].extraNonDraftedPerceivedPathCost;
			}
			return num;
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x001FF4A0 File Offset: 0x001FD8A0
		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 a2 = (!this.linkTargetCells.ContainsKey(a)) ? RegionCostCalculator.RegionLinkCenter(a) : this.linkTargetCells[a];
			IntVec3 b2 = (!this.linkTargetCells.ContainsKey(b)) ? RegionCostCalculator.RegionLinkCenter(b) : this.linkTargetCells[b];
			IntVec3 intVec = a2 - b2;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x001FF54C File Offset: 0x001FD94C
		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x001FF5AC File Offset: 0x001FD9AC
		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir != SpanDirection.East) ? 0 : (e.length / 2));
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x001FF5EC File Offset: 0x001FD9EC
		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir != SpanDirection.North) ? 0 : (e.length / 2));
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x001FF62C File Offset: 0x001FDA2C
		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x001FF660 File Offset: 0x001FDA60
		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x001FF6A4 File Offset: 0x001FDAA4
		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x001FF6CC File Offset: 0x001FDACC
		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x001FF6E8 File Offset: 0x001FDAE8
		private static IntVec3 LinkClosestCell(IntVec3 cell, RegionLink link)
		{
			EdgeSpan span = link.span;
			int num = 0;
			int num2 = 0;
			if (span.dir == SpanDirection.North)
			{
				num2 = span.length - 1;
			}
			else
			{
				num = span.length - 1;
			}
			IntVec3 root = span.root;
			IntVec3 result = new IntVec3(Mathf.Clamp(cell.x, root.x, root.x + num), 0, Mathf.Clamp(cell.z, root.z, root.z + num2));
			return result;
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x001FF77C File Offset: 0x001FDB7C
		private void GetPreciseRegionLinkDistances(Region region, CellRect destination, List<Pair<RegionLink, int>> outDistances)
		{
			outDistances.Clear();
			RegionCostCalculator.tmpCellIndices.Clear();
			if (destination.Width == 1 && destination.Height == 1)
			{
				RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(destination.CenterCell));
			}
			else
			{
				CellRect.CellRectIterator iterator = destination.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 c = iterator.Current;
					if (c.InBounds(this.map))
					{
						RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(c));
					}
					iterator.MoveNext();
				}
			}
			Dijkstra<int>.Run(RegionCostCalculator.tmpCellIndices, (int x) => this.PreciseRegionLinkDistancesNeighborsGetter(x, region), this.preciseRegionLinkDistancesDistanceGetter, RegionCostCalculator.tmpDistances, null);
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
				{
					float num;
					if (!RegionCostCalculator.tmpDistances.TryGetValue(this.map.cellIndices.CellToIndex(this.linkTargetCells[regionLink]), out num))
					{
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531, false);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x001FF920 File Offset: 0x001FDD20
		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			IEnumerable<int> result;
			if (this.regionGrid[node] == null || this.regionGrid[node] != region)
			{
				result = null;
			}
			else
			{
				result = this.PathableNeighborIndices(node);
			}
			return result;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x001FF960 File Offset: 0x001FDD60
		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)(this.GetCellCostFast(b, false) + ((!this.AreCellsDiagonal(a, b)) ? this.moveTicksCardinal : this.moveTicksDiagonal));
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x001FF9A0 File Offset: 0x001FDDA0
		private bool AreCellsDiagonal(int a, int b)
		{
			int x = this.map.Size.x;
			return a % x != b % x && a / x != b / x;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x001FF9E4 File Offset: 0x001FDDE4
		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.map.pathGrid;
			int x = this.map.Size.x;
			bool flag = index % x > 0;
			bool flag2 = index % x < x - 1;
			bool flag3 = index >= x;
			bool flag4 = index / x < this.map.Size.z - 1;
			if (flag3 && pathGrid.WalkableFast(index - x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x);
			}
			if (flag2 && pathGrid.WalkableFast(index + 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + 1);
			}
			if (flag && pathGrid.WalkableFast(index - 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - 1);
			}
			if (flag4 && pathGrid.WalkableFast(index + x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x);
			}
			bool flag5 = !flag || PathFinder.BlocksDiagonalMovement(index - 1, this.map);
			bool flag6 = !flag2 || PathFinder.BlocksDiagonalMovement(index + 1, this.map);
			if (flag3 && !PathFinder.BlocksDiagonalMovement(index - x, this.map))
			{
				if (!flag6 && pathGrid.WalkableFast(index - x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index - x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x - 1);
				}
			}
			if (flag4 && !PathFinder.BlocksDiagonalMovement(index + x, this.map))
			{
				if (!flag6 && pathGrid.WalkableFast(index + x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index + x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x - 1);
				}
			}
			return RegionCostCalculator.tmpPathableNeighborIndices;
		}

		// Token: 0x02000A9B RID: 2715
		private struct RegionLinkQueueEntry
		{
			// Token: 0x04002635 RID: 9781
			private Region from;

			// Token: 0x04002636 RID: 9782
			private RegionLink link;

			// Token: 0x04002637 RID: 9783
			private int cost;

			// Token: 0x04002638 RID: 9784
			private int estimatedPathCost;

			// Token: 0x06003C7B RID: 15483 RVA: 0x001FFC17 File Offset: 0x001FE017
			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}

			// Token: 0x17000929 RID: 2345
			// (get) Token: 0x06003C7C RID: 15484 RVA: 0x001FFC38 File Offset: 0x001FE038
			public Region From
			{
				get
				{
					return this.from;
				}
			}

			// Token: 0x1700092A RID: 2346
			// (get) Token: 0x06003C7D RID: 15485 RVA: 0x001FFC54 File Offset: 0x001FE054
			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			// Token: 0x1700092B RID: 2347
			// (get) Token: 0x06003C7E RID: 15486 RVA: 0x001FFC70 File Offset: 0x001FE070
			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			// Token: 0x1700092C RID: 2348
			// (get) Token: 0x06003C7F RID: 15487 RVA: 0x001FFC8C File Offset: 0x001FE08C
			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}
		}

		// Token: 0x02000A9C RID: 2716
		private class DistanceComparer : IComparer<RegionCostCalculator.RegionLinkQueueEntry>
		{
			// Token: 0x06003C81 RID: 15489 RVA: 0x001FFCB0 File Offset: 0x001FE0B0
			public int Compare(RegionCostCalculator.RegionLinkQueueEntry a, RegionCostCalculator.RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}
	}
}
