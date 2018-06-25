using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C95 RID: 3221
	public static class RegionTraverser
	{
		// Token: 0x0400301F RID: 12319
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x04003020 RID: 12320
		public static int NumWorkers = 8;

		// Token: 0x04003021 RID: 12321
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x060046AF RID: 18095 RVA: 0x00254DB8 File Offset: 0x002531B8
		static RegionTraverser()
		{
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x00254E10 File Offset: 0x00253210
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

		// Token: 0x060046B1 RID: 18097 RVA: 0x00254EB8 File Offset: 0x002532B8
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

		// Token: 0x060046B2 RID: 18098 RVA: 0x00254F34 File Offset: 0x00253334
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

		// Token: 0x060046B3 RID: 18099 RVA: 0x00254FD8 File Offset: 0x002533D8
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x0025500C File Offset: 0x0025340C
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region != null)
			{
				RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x0025503C File Offset: 0x0025343C
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

		// Token: 0x02000C96 RID: 3222
		private class BFSWorker
		{
			// Token: 0x04003022 RID: 12322
			private Deque<Region> open = new Deque<Region>();

			// Token: 0x04003023 RID: 12323
			private int numRegionsProcessed;

			// Token: 0x04003024 RID: 12324
			private uint closedIndex = 1u;

			// Token: 0x04003025 RID: 12325
			private int closedArrayPos;

			// Token: 0x04003026 RID: 12326
			private const int skippableRegionSize = 4;

			// Token: 0x060046B7 RID: 18103 RVA: 0x00255119 File Offset: 0x00253519
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x060046B8 RID: 18104 RVA: 0x0025513B File Offset: 0x0025353B
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x060046B9 RID: 18105 RVA: 0x0025514C File Offset: 0x0025354C
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

			// Token: 0x060046BA RID: 18106 RVA: 0x002551C9 File Offset: 0x002535C9
			private void FinalizeSearch()
			{
			}

			// Token: 0x060046BB RID: 18107 RVA: 0x002551CC File Offset: 0x002535CC
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
	}
}
