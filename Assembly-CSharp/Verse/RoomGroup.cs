using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C9A RID: 3226
	public class RoomGroup
	{
		// Token: 0x04003048 RID: 12360
		public int ID = -1;

		// Token: 0x04003049 RID: 12361
		private List<Room> rooms = new List<Room>();

		// Token: 0x0400304A RID: 12362
		private RoomGroupTempTracker tempTracker;

		// Token: 0x0400304B RID: 12363
		private int cachedOpenRoofCount = -1;

		// Token: 0x0400304C RID: 12364
		private int cachedCellCount = -1;

		// Token: 0x0400304D RID: 12365
		private static int nextRoomGroupID;

		// Token: 0x0400304E RID: 12366
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00256EE4 File Offset: 0x002552E4
		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060046F1 RID: 18161 RVA: 0x00256F00 File Offset: 0x00255300
		public Map Map
		{
			get
			{
				return (!this.rooms.Any<Room>()) ? null : this.rooms[0].Map;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060046F2 RID: 18162 RVA: 0x00256F3C File Offset: 0x0025533C
		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x060046F3 RID: 18163 RVA: 0x00256F5C File Offset: 0x0025535C
		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060046F4 RID: 18164 RVA: 0x00256F78 File Offset: 0x00255378
		// (set) Token: 0x060046F5 RID: 18165 RVA: 0x00256F98 File Offset: 0x00255398
		public float Temperature
		{
			get
			{
				return this.tempTracker.Temperature;
			}
			set
			{
				this.tempTracker.Temperature = value;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060046F6 RID: 18166 RVA: 0x00256FA8 File Offset: 0x002553A8
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x00256FE8 File Offset: 0x002553E8
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					foreach (IntVec3 c in this.rooms[i].Cells)
					{
						yield return c;
					}
				}
				yield break;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060046F8 RID: 18168 RVA: 0x00257014 File Offset: 0x00255414
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.rooms.Count; i++)
					{
						this.cachedCellCount += this.rooms[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x00257080 File Offset: 0x00255480
		public IEnumerable<Region> Regions
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					foreach (Region r in this.rooms[i].Regions)
					{
						yield return r;
					}
				}
				yield break;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060046FA RID: 18170 RVA: 0x002570AC File Offset: 0x002554AC
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					num += this.rooms[i].RegionCount;
				}
				return num;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060046FB RID: 18171 RVA: 0x002570F8 File Offset: 0x002554F8
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = 0;
					for (int i = 0; i < this.rooms.Count; i++)
					{
						this.cachedOpenRoofCount += this.rooms[i].OpenRoofCount;
					}
				}
				return this.cachedOpenRoofCount;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060046FC RID: 18172 RVA: 0x00257164 File Offset: 0x00255564
		public bool AnyRoomTouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					if (this.rooms[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x002571B8 File Offset: 0x002555B8
		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x002571F8 File Offset: 0x002555F8
		public void AddRoom(Room room)
		{
			if (this.rooms.Contains(room))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same room twice to RoomGroup. room=",
					room,
					", roomGroup=",
					this
				}), false);
			}
			else
			{
				this.rooms.Add(room);
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00257254 File Offset: 0x00255654
		public void RemoveRoom(Room room)
		{
			if (!this.rooms.Contains(room))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove room from RoomGroup but this room is not here. room=",
					room,
					", roomGroup=",
					this
				}), false);
			}
			else
			{
				this.rooms.Remove(room);
			}
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x002572B0 File Offset: 0x002556B0
		public bool PushHeat(float energy)
		{
			bool result;
			if (this.UsesOutdoorTemperature)
			{
				result = false;
			}
			else
			{
				this.Temperature += energy / (float)this.CellCount;
				result = true;
			}
			return result;
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x002572EF File Offset: 0x002556EF
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00257304 File Offset: 0x00255704
		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x00257320 File Offset: 0x00255720
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"RoomGroup ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RoomCount=",
				this.RoomCount,
				"\n  RegionCount=",
				this.RegionCount,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  ",
				this.tempTracker.DebugString()
			});
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x002573E8 File Offset: 0x002557E8
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}
	}
}
