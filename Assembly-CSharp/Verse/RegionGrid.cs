using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

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

		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static RegionGrid()
		{
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Region>, IEnumerator, IDisposable, IEnumerator<Region>
		{
			internal int <count>__1;

			internal int <i>__2;

			internal RegionGrid $this;

			internal Region $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					RegionGrid.allRegionsYielded.Clear();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						break;
					default:
						count = this.map.cellIndices.NumGridCells;
						i = 0;
						goto IL_FA;
					}
					IL_EB:
					i++;
					IL_FA:
					if (i < count)
					{
						if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							this.$current = this.regionGrid[i];
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_EB;
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			Region IEnumerator<Region>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Region>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RegionGrid.<>c__Iterator0 <>c__Iterator = new RegionGrid.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			private void <>__Finally0()
			{
				RegionGrid.allRegionsYielded.Clear();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<Region>, IEnumerator, IDisposable, IEnumerator<Region>
		{
			internal int <count>__1;

			internal int <i>__2;

			internal RegionGrid $this;

			internal Region $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
					{
						Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
					}
					this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
					RegionGrid.allRegionsYielded.Clear();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						break;
					default:
						count = this.map.cellIndices.NumGridCells;
						i = 0;
						goto IL_16C;
					}
					IL_15D:
					i++;
					IL_16C:
					if (i < count)
					{
						if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							this.$current = this.regionGrid[i];
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_15D;
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			Region IEnumerator<Region>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Region>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RegionGrid.<>c__Iterator1 <>c__Iterator = new RegionGrid.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			private void <>__Finally0()
			{
				RegionGrid.allRegionsYielded.Clear();
			}
		}
	}
}
