using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C95 RID: 3221
	public static class RegionTraverser
	{
		// Token: 0x060046A3 RID: 18083 RVA: 0x0025362C File Offset: 0x00251A2C
		static RegionTraverser()
		{
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x00253684 File Offset: 0x00251A84
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

		// Token: 0x060046A5 RID: 18085 RVA: 0x0025372C File Offset: 0x00251B2C
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

		// Token: 0x060046A6 RID: 18086 RVA: 0x002537A8 File Offset: 0x00251BA8
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

		// Token: 0x060046A7 RID: 18087 RVA: 0x0025384C File Offset: 0x00251C4C
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x00253880 File Offset: 0x00251C80
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region != null)
			{
				RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x002538B0 File Offset: 0x00251CB0
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

		// Token: 0x0400300E RID: 12302
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x0400300F RID: 12303
		public static int NumWorkers = 8;

		// Token: 0x04003010 RID: 12304
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x02000C96 RID: 3222
		private class BFSWorker
		{
			// Token: 0x060046AB RID: 18091 RVA: 0x0025398D File Offset: 0x00251D8D
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x060046AC RID: 18092 RVA: 0x002539AF File Offset: 0x00251DAF
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x060046AD RID: 18093 RVA: 0x002539C0 File Offset: 0x00251DC0
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x060046AE RID: 18094 RVA: 0x00253A1B File Offset: 0x00251E1B
			private void FinalizeSearch()
			{
			}

			// Token: 0x060046AF RID: 18095 RVA: 0x00253A20 File Offset: 0x00251E20
			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) != RegionType.None)
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

			// Token: 0x04003011 RID: 12305
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x04003012 RID: 12306
			private int numRegionsProcessed;

			// Token: 0x04003013 RID: 12307
			private uint closedIndex = 1u;

			// Token: 0x04003014 RID: 12308
			private int closedArrayPos;
		}
	}
}
