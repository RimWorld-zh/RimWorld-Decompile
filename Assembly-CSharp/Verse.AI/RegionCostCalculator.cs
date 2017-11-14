using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	public class RegionCostCalculator
	{
		private struct RegionLinkQueueEntry
		{
			private Region from;

			private RegionLink link;

			private int cost;

			private int estimatedPathCost;

			public Region From
			{
				get
				{
					return this.from;
				}
			}

			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}

			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}
		}

		private class DistanceComparer : IComparer<RegionLinkQueueEntry>
		{
			public int Compare(RegionLinkQueueEntry a, RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}

		private Map map;

		private Region[] regionGrid;

		private TraverseParms traverseParms;

		private IntVec3 destinationCell;

		private int moveTicksCardinal;

		private int moveTicksDiagonal;

		private ByteGrid avoidGrid;

		private Area allowedArea;

		private Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		private Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		private Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		private FastPriorityQueue<RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionLinkQueueEntry>(new DistanceComparer());

		private Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		private List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		private Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		private const int SampleCount = 11;

		private static int[] pathCostSamples = new int[11];

		private static List<int> tmpCellIndices = new List<int>();

		private static Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		private static List<int> tmpPathableNeighborIndices = new List<int>();

		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = this.PreciseRegionLinkDistancesDistanceGetter;
		}

		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.destinationCell = destination.CenterCell;
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.avoidGrid = avoidGrid;
			this.allowedArea = allowedArea;
			this.regionMinLink.Clear();
			this.distances.Clear();
			this.linkTargetCells.Clear();
			this.queue.Clear();
			this.minPathCosts.Clear();
			foreach (Region destRegion in destRegions)
			{
				int minPathCost = this.RegionMedianPathCost(destRegion);
				for (int i = 0; i < destRegion.links.Count; i++)
				{
					RegionLink regionLink = destRegion.links[i];
					if (regionLink.GetOtherRegion(destRegion).type.Passable())
					{
						int num = this.RegionLinkDistance(this.destinationCell, regionLink, minPathCost);
						int num2 = default(int);
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
				this.GetPreciseRegionLinkDistances(destRegion, destination, this.preciseRegionLinkDistances);
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
					this.queue.Push(new RegionLinkQueueEntry(destRegion, first, num4, num4));
				}
			}
		}

		public int GetRegionDistance(Region region, out RegionLink minLink)
		{
			if (this.regionMinLink.TryGetValue(region.id, out minLink))
			{
				return this.distances[minLink];
			}
			while (this.queue.Count != 0)
			{
				RegionLinkQueueEntry regionLinkQueueEntry = this.queue.Pop();
				int num = this.distances[regionLinkQueueEntry.Link];
				Region otherRegion;
				int num2;
				if (regionLinkQueueEntry.Cost == num)
				{
					otherRegion = regionLinkQueueEntry.Link.GetOtherRegion(regionLinkQueueEntry.From);
					if (otherRegion != null && otherRegion.valid)
					{
						num2 = 0;
						if (otherRegion.portal != null)
						{
							num2 = PathFinder.GetBuildingCost(otherRegion.portal, this.traverseParms, this.traverseParms.pawn);
							if (num2 != 2147483647)
							{
								num2 += this.OctileDistance(1, 0);
								goto IL_00ca;
							}
							continue;
						}
						goto IL_00ca;
					}
				}
				continue;
				IL_00ca:
				int minPathCost = this.RegionMedianPathCost(otherRegion);
				for (int i = 0; i < otherRegion.links.Count; i++)
				{
					RegionLink regionLink = otherRegion.links[i];
					if (regionLink != regionLinkQueueEntry.Link && regionLink.GetOtherRegion(otherRegion).type.Passable())
					{
						int val = (otherRegion.portal == null) ? this.RegionLinkDistance(regionLinkQueueEntry.Link, regionLink, minPathCost) : num2;
						val = Math.Max(val, 1);
						int num3 = num + val;
						int estimatedPathCost = this.MinimumRegionLinkDistance(this.destinationCell, regionLink) + num3;
						int num4 = default(int);
						if (this.distances.TryGetValue(regionLink, out num4))
						{
							if (num3 < num4)
							{
								this.distances[regionLink] = num3;
								this.queue.Push(new RegionLinkQueueEntry(otherRegion, regionLink, num3, estimatedPathCost));
							}
						}
						else
						{
							this.distances.Add(regionLink, num3);
							this.queue.Push(new RegionLinkQueueEntry(otherRegion, regionLink, num3, estimatedPathCost));
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
			return 10000;
		}

		public int GetRegionBestDistances(Region region, out RegionLink bestLink, out RegionLink secondBestLink, out int secondBestCost)
		{
			int regionDistance = this.GetRegionDistance(region, out bestLink);
			secondBestLink = null;
			secondBestCost = 2147483647;
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				int num = default(int);
				if (regionLink != bestLink && regionLink.GetOtherRegion(region).type.Passable() && this.distances.TryGetValue(regionLink, out num) && num < secondBestCost)
				{
					secondBestCost = num;
					secondBestLink = regionLink;
				}
			}
			return regionDistance;
		}

		public int RegionMedianPathCost(Region region)
		{
			int result = default(int);
			if (this.minPathCosts.TryGetValue(region, out result))
			{
				return result;
			}
			bool ignoreAllowedAreaCost = this.allowedArea != null && region.OverlapWith(this.allowedArea) != AreaOverlap.None;
			CellIndices cellIndices = this.map.cellIndices;
			Rand.PushState();
			Rand.Seed = cellIndices.CellToIndex(region.extentsClose.CenterCell) * (region.links.Count + 1);
			for (int i = 0; i < 11; i++)
			{
				RegionCostCalculator.pathCostSamples[i] = this.GetCellCostFast(cellIndices.CellToIndex(region.RandomCell), ignoreAllowedAreaCost);
			}
			Rand.PopState();
			Array.Sort(RegionCostCalculator.pathCostSamples);
			int num = RegionCostCalculator.pathCostSamples[4];
			this.minPathCosts[region] = num;
			return num;
		}

		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.map.pathGrid.pathGrid[index];
			if (this.avoidGrid != null)
			{
				num += this.avoidGrid[index] * 8;
			}
			if (this.allowedArea != null && !ignoreAllowedAreaCost && !this.allowedArea[index])
			{
				num += 600;
			}
			return num;
		}

		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 a2 = (!this.linkTargetCells.ContainsKey(a)) ? RegionCostCalculator.RegionLinkCenter(a) : this.linkTargetCells[a];
			IntVec3 b2 = (!this.linkTargetCells.ContainsKey(b)) ? RegionCostCalculator.RegionLinkCenter(b) : this.linkTargetCells[b];
			IntVec3 intVec = a2 - b2;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir == SpanDirection.East) ? (e.length / 2) : 0);
		}

		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir == SpanDirection.North) ? (e.length / 2) : 0);
		}

		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

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
			return new IntVec3(Mathf.Clamp(cell.x, root.x, root.x + num), 0, Mathf.Clamp(cell.z, root.z, root.z + num2));
		}

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
					IntVec3 current = iterator.Current;
					if (current.InBounds(this.map))
					{
						RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(current));
					}
					iterator.MoveNext();
				}
			}
			Dijkstra<int>.Run(RegionCostCalculator.tmpCellIndices, (int x) => this.PreciseRegionLinkDistancesNeighborsGetter(x, region), this.preciseRegionLinkDistancesDistanceGetter, RegionCostCalculator.tmpDistances, null);
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink.GetOtherRegion(region).type.Passable())
				{
					float num = default(float);
					if (!RegionCostCalculator.tmpDistances.TryGetValue(this.map.cellIndices.CellToIndex(this.linkTargetCells[regionLink]), out num))
					{
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			if (this.regionGrid[node] != null && this.regionGrid[node] == region)
			{
				return this.PathableNeighborIndices(node);
			}
			return null;
		}

		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)(this.GetCellCostFast(b, false) + ((!this.AreCellsDiagonal(a, b)) ? this.moveTicksCardinal : this.moveTicksDiagonal));
		}

		private bool AreCellsDiagonal(int a, int b)
		{
			IntVec3 size = this.map.Size;
			int x = size.x;
			return a % x != b % x && a / x != b / x;
		}

		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.map.pathGrid;
			IntVec3 size = this.map.Size;
			int x = size.x;
			bool flag = index % x > 0;
			bool flag2 = index % x < x - 1;
			bool flag3 = index >= x;
			int num = index / x;
			IntVec3 size2 = this.map.Size;
			bool flag4 = num < size2.z - 1;
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
	}
}
