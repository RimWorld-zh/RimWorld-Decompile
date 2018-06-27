using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class RegionTraverser
	{
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		public static int NumWorkers = 8;

		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		public static Room FloodAndSetRooms(Region root, Map map, Room existingRoom)
		{
			Room floodingRoom;
			if (existingRoom == null)
			{
				floodingRoom = Room.MakeNew(map);
			}
			else
			{
				floodingRoom = existingRoom;
			}
			root.Room = floodingRoom;
			Room floodingRoom2;
			if (!root.type.AllowsMultipleRegionsPerRoom())
			{
				floodingRoom2 = floodingRoom;
			}
			else
			{
				RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.Room != floodingRoom;
				RegionProcessor regionProcessor = delegate(Region r)
				{
					r.Room = floodingRoom;
					return false;
				};
				RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
				floodingRoom2 = floodingRoom;
			}
			return floodingRoom2;
		}

		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (root.type.AllowsMultipleRegionsPerRoom())
			{
				RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0;
				RegionProcessor regionProcessor = delegate(Region r)
				{
					r.newRegionGroupIndex = newRegionGroupIndex;
					return false;
				};
				RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			}
		}

		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = A.GetRegion(map, traversableRegionTypes);
			bool result;
			if (region == null)
			{
				result = false;
			}
			else
			{
				Region regB = B.GetRegion(map, traversableRegionTypes);
				if (regB == null)
				{
					result = false;
				}
				else if (region == regB)
				{
					result = true;
				}
				else
				{
					RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
					bool found = false;
					RegionProcessor regionProcessor = delegate(Region r)
					{
						bool result2;
						if (r == regB)
						{
							found = true;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
						return result2;
					};
					RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, regionLookCount, traversableRegionTypes);
					result = found;
				}
			}
			return result;
		}

		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region != null)
			{
				RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
		}

		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers + ", or a bug has put this system in an inconsistent state. Resetting.", false);
			}
			else if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.", false);
			}
			else
			{
				RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
				try
				{
					bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
				}
				catch (Exception ex)
				{
					Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString(), false);
				}
				finally
				{
					bfsworker.Clear();
					RegionTraverser.freeWorkers.Enqueue(bfsworker);
				}
			}
		}

		[CompilerGenerated]
		private static bool <PassAll>m__0(Region from, Region to)
		{
			return true;
		}

		private class BFSWorker
		{
			private Deque<Region> open = new Deque<Region>();

			private int numRegionsProcessed;

			private uint closedIndex = 1u;

			private int closedArrayPos;

			private const int skippableRegionSize = 4;

			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			public void Clear()
			{
				this.open.Clear();
			}

			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				if (region.extentsClose.Area <= 4)
				{
					this.open.PushFront(region);
				}
				else
				{
					this.open.PushBack(region);
				}
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			private void FinalizeSearch()
			{
			}

			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) != RegionType.None)
				{
					this.closedIndex += 1u;
					this.open.Clear();
					this.numRegionsProcessed = 0;
					this.QueueNewOpenRegion(root);
					while (!this.open.Empty)
					{
						Region region = this.open.PopFront();
						if (DebugViewSettings.drawRegionTraversal)
						{
							region.Debug_Notify_Traversed();
						}
						if (regionProcessor != null)
						{
							if (regionProcessor(region))
							{
								this.FinalizeSearch();
								return;
							}
						}
						this.numRegionsProcessed++;
						if (this.numRegionsProcessed >= maxRegions)
						{
							this.FinalizeSearch();
							return;
						}
						for (int i = 0; i < region.links.Count; i++)
						{
							RegionLink regionLink = region.links[i];
							for (int j = 0; j < 2; j++)
							{
								Region region2 = regionLink.regions[j];
								if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != RegionType.None && (entryCondition == null || entryCondition(region, region2)))
								{
									this.QueueNewOpenRegion(region2);
								}
							}
						}
					}
					this.FinalizeSearch();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <FloodAndSetRooms>c__AnonStorey0
		{
			internal Region root;

			internal Room floodingRoom;

			public <FloodAndSetRooms>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.type == this.root.type && r.Room != this.floodingRoom;
			}

			internal bool <>m__1(Region r)
			{
				r.Room = this.floodingRoom;
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <FloodAndSetNewRegionIndex>c__AnonStorey1
		{
			internal Region root;

			internal int newRegionGroupIndex;

			public <FloodAndSetNewRegionIndex>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.type == this.root.type && r.newRegionGroupIndex < 0;
			}

			internal bool <>m__1(Region r)
			{
				r.newRegionGroupIndex = this.newRegionGroupIndex;
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <WithinRegions>c__AnonStorey2
		{
			internal TraverseParms traverseParams;

			internal Region regB;

			internal bool found;

			public <WithinRegions>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Allows(this.traverseParams, false);
			}

			internal bool <>m__1(Region r)
			{
				bool result;
				if (r == this.regB)
				{
					this.found = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <MarkRegionsBFS>c__AnonStorey3
		{
			internal int inRadiusMark;

			public <MarkRegionsBFS>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Region r)
			{
				r.mark = this.inRadiusMark;
				return false;
			}
		}
	}
}
