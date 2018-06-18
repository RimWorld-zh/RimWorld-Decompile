using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8C RID: 3212
	public sealed class RegionGrid
	{
		// Token: 0x0600466B RID: 18027 RVA: 0x00251B40 File Offset: 0x0024FF40
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600466C RID: 18028 RVA: 0x00251B90 File Offset: 0x0024FF90
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

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600466D RID: 18029 RVA: 0x00251BF4 File Offset: 0x0024FFF4
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

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x0600466E RID: 18030 RVA: 0x00251C20 File Offset: 0x00250020
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

		// Token: 0x0600466F RID: 18031 RVA: 0x00251C4C File Offset: 0x0025004C
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

		// Token: 0x06004670 RID: 18032 RVA: 0x00251D1C File Offset: 0x0025011C
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

		// Token: 0x06004671 RID: 18033 RVA: 0x00251D94 File Offset: 0x00250194
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00251DC1 File Offset: 0x002501C1
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x00251DE0 File Offset: 0x002501E0
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

		// Token: 0x06004674 RID: 18036 RVA: 0x00251E58 File Offset: 0x00250258
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

		// Token: 0x04002FF4 RID: 12276
		private Map map;

		// Token: 0x04002FF5 RID: 12277
		private Region[] regionGrid;

		// Token: 0x04002FF6 RID: 12278
		private int curCleanIndex = 0;

		// Token: 0x04002FF7 RID: 12279
		public List<Room> allRooms = new List<Room>();

		// Token: 0x04002FF8 RID: 12280
		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x04002FF9 RID: 12281
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x04002FFA RID: 12282
		public HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}
