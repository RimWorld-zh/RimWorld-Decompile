using System;
using System.Collections.Generic;

namespace Verse
{
	public static class RegionTraverser
	{
		private class BFSWorker
		{
			private Queue<Region> open = new Queue<Region>();

			private int numRegionsProcessed;

			private uint closedIndex = 1u;

			private int closedArrayPos;

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
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			private void FinalizeSearch()
			{
			}

			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) != 0)
				{
					this.closedIndex += 1u;
					this.open.Clear();
					this.numRegionsProcessed = 0;
					this.QueueNewOpenRegion(root);
					while (this.open.Count > 0)
					{
						Region region = this.open.Dequeue();
						if (DebugViewSettings.drawRegionTraversal)
						{
							region.Debug_Notify_Traversed();
						}
						if ((object)regionProcessor != null && regionProcessor(region))
						{
							this.FinalizeSearch();
							return;
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
								if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != 0 && ((object)entryCondition == null || entryCondition(region, region2)))
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

		private static Queue<BFSWorker> freeWorkers;

		public static int NumWorkers;

		static RegionTraverser()
		{
			RegionTraverser.freeWorkers = new Queue<BFSWorker>();
			RegionTraverser.NumWorkers = 8;
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new BFSWorker(i));
			}
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
			Room result;
			if (!root.type.AllowsMultipleRegionsPerRoom())
			{
				result = floodingRoom;
			}
			else
			{
				RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.type == root.type && r.Room != floodingRoom);
				RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
				{
					r.Room = floodingRoom;
					return false;
				};
				RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
				result = floodingRoom;
			}
			return result;
		}

		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (root.type.AllowsMultipleRegionsPerRoom())
			{
				RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0);
				RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
				{
					r.newRegionGroupIndex = newRegionGroupIndex;
					return false;
				};
				RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			}
		}

		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				throw new ArgumentException("traverseParams (PassAllDestroyableThings not supported)");
			}
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
					RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParams, false));
					bool found = false;
					RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
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
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, (RegionProcessor)delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
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
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers + ", or a bug has put this system in an inconsistent state. Resetting.");
			}
			else if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.");
			}
			else
			{
				BFSWorker bFSWorker = RegionTraverser.freeWorkers.Dequeue();
				try
				{
					bFSWorker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
				}
				catch (Exception ex)
				{
					Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString());
				}
				finally
				{
					bFSWorker.Clear();
					RegionTraverser.freeWorkers.Enqueue(bFSWorker);
				}
			}
		}
	}
}
