using System.Collections.Generic;

namespace Verse
{
	public sealed class RegionGrid
	{
		private Map map;

		private Region[] regionGrid;

		private int curCleanIndex = 0;

		public List<Room> allRooms = new List<Room>();

		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		private const int CleanSquaresPerFrame = 16;

		public HashSet<Region> drawnRegions = new HashSet<Region>();

		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int i = 0;
					while (true)
					{
						if (i < count)
						{
							if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
								break;
							i++;
							continue;
						}
						yield break;
					}
					yield return this.regionGrid[i];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				finally
				{
					((_003C_003Ec__Iterator0)/*Error near IL_0115: stateMachine*/)._003C_003E__Finally0();
				}
				IL_0125:
				/*Error near IL_0126: Unexpected return in MoveNext()*/;
			}
		}

		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int i = 0;
					while (true)
					{
						if (i < count)
						{
							if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
								break;
							i++;
							continue;
						}
						yield break;
					}
					yield return this.regionGrid[i];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				finally
				{
					((_003C_003Ec__Iterator1)/*Error near IL_0186: stateMachine*/)._003C_003E__Finally0();
				}
				IL_0196:
				/*Error near IL_0197: Unexpected return in MoveNext()*/;
			}
		}

		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		public Region GetValidRegionAt(IntVec3 c)
		{
			Region result;
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c);
				result = null;
			}
			else
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get valid region at " + c + " but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
				result = ((region == null || !region.valid) ? null : region);
			}
			return result;
		}

		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			Region result;
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c);
				result = null;
			}
			else
			{
				Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
				result = ((region == null || !region.valid) ? null : region);
			}
			return result;
		}

		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

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

		public void DebugDraw()
		{
			if (this.map == Find.VisibleMap)
			{
				if (DebugViewSettings.drawRegionTraversal)
				{
					CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
					currentViewRect.ClipInsideMap(this.map);
					foreach (IntVec3 item in currentViewRect)
					{
						Region validRegionAt = this.GetValidRegionAt(item);
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
					if (!DebugViewSettings.drawRegions && !DebugViewSettings.drawRegionLinks && !DebugViewSettings.drawRegionThings)
						return;
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
