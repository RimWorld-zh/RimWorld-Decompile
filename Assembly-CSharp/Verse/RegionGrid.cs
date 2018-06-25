using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8B RID: 3211
	public sealed class RegionGrid
	{
		// Token: 0x04002FFE RID: 12286
		private Map map;

		// Token: 0x04002FFF RID: 12287
		private Region[] regionGrid;

		// Token: 0x04003000 RID: 12288
		private int curCleanIndex = 0;

		// Token: 0x04003001 RID: 12289
		public List<Room> allRooms = new List<Room>();

		// Token: 0x04003002 RID: 12290
		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x04003003 RID: 12291
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x04003004 RID: 12292
		public HashSet<Region> drawnRegions = new HashSet<Region>();

		// Token: 0x06004677 RID: 18039 RVA: 0x00252FEC File Offset: 0x002513EC
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06004678 RID: 18040 RVA: 0x0025303C File Offset: 0x0025143C
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06004679 RID: 18041 RVA: 0x002530A0 File Offset: 0x002514A0
		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					for (int i = 0; i < count; i++)
					{
						if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x0600467A RID: 18042 RVA: 0x002530CC File Offset: 0x002514CC
		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					for (int i = 0; i < count; i++)
					{
						if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
			}
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x002530F8 File Offset: 0x002514F8
		public Region GetValidRegionAt(IntVec3 c)
		{
			Region result;
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c, false);
				result = null;
			}
			else
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get valid region at " + c + " but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
				if (region != null && region.valid)
				{
					result = region;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x002531C8 File Offset: 0x002515C8
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			Region result;
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c, false);
				result = null;
			}
			else
			{
				Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
				if (region != null && region.valid)
				{
					result = region;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00253240 File Offset: 0x00251640
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0025326D File Offset: 0x0025166D
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x0025328C File Offset: 0x0025168C
		public void UpdateClean()
		{
			for (int i = 0; i < 16; i++)
			{
				if (this.curCleanIndex >= this.regionGrid.Length)
				{
					this.curCleanIndex = 0;
				}
				Region region = this.regionGrid[this.curCleanIndex];
				if (region != null && !region.valid)
				{
					this.regionGrid[this.curCleanIndex] = null;
				}
				this.curCleanIndex++;
			}
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x00253304 File Offset: 0x00251704
		public void DebugDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				if (DebugViewSettings.drawRegionTraversal)
				{
					CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
					currentViewRect.ClipInsideMap(this.map);
					foreach (IntVec3 c in currentViewRect)
					{
						Region validRegionAt = this.GetValidRegionAt(c);
						if (validRegionAt != null && !this.drawnRegions.Contains(validRegionAt))
						{
							validRegionAt.DebugDraw();
							this.drawnRegions.Add(validRegionAt);
						}
					}
					this.drawnRegions.Clear();
				}
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(this.map))
				{
					if (DebugViewSettings.drawRooms)
					{
						Room room = intVec.GetRoom(this.map, RegionType.Set_All);
						if (room != null)
						{
							room.DebugDraw();
						}
					}
					if (DebugViewSettings.drawRoomGroups)
					{
						RoomGroup roomGroup = intVec.GetRoomGroup(this.map);
						if (roomGroup != null)
						{
							roomGroup.DebugDraw();
						}
					}
					if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
					{
						Region regionAt_NoRebuild_InvalidAllowed = this.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
						if (regionAt_NoRebuild_InvalidAllowed != null)
						{
							regionAt_NoRebuild_InvalidAllowed.DebugDrawMouseover();
						}
					}
				}
			}
		}
	}
}
