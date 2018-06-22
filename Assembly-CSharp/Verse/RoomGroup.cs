using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C97 RID: 3223
	public class RoomGroup
	{
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060046ED RID: 18157 RVA: 0x00256B28 File Offset: 0x00254F28
		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x00256B44 File Offset: 0x00254F44
		public Map Map
		{
			get
			{
				return (!this.rooms.Any<Room>()) ? null : this.rooms[0].Map;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x060046EF RID: 18159 RVA: 0x00256B80 File Offset: 0x00254F80
		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00256BA0 File Offset: 0x00254FA0
		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060046F1 RID: 18161 RVA: 0x00256BBC File Offset: 0x00254FBC
		// (set) Token: 0x060046F2 RID: 18162 RVA: 0x00256BDC File Offset: 0x00254FDC
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

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060046F3 RID: 18163 RVA: 0x00256BEC File Offset: 0x00254FEC
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060046F4 RID: 18164 RVA: 0x00256C2C File Offset: 0x0025502C
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

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060046F5 RID: 18165 RVA: 0x00256C58 File Offset: 0x00255058
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

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060046F6 RID: 18166 RVA: 0x00256CC4 File Offset: 0x002550C4
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

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x00256CF0 File Offset: 0x002550F0
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

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060046F8 RID: 18168 RVA: 0x00256D3C File Offset: 0x0025513C
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

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x00256DA8 File Offset: 0x002551A8
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

		// Token: 0x060046FA RID: 18170 RVA: 0x00256DFC File Offset: 0x002551FC
		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00256E3C File Offset: 0x0025523C
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

		// Token: 0x060046FC RID: 18172 RVA: 0x00256E98 File Offset: 0x00255298
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

		// Token: 0x060046FD RID: 18173 RVA: 0x00256EF4 File Offset: 0x002552F4
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

		// Token: 0x060046FE RID: 18174 RVA: 0x00256F33 File Offset: 0x00255333
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00256F48 File Offset: 0x00255348
		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00256F64 File Offset: 0x00255364
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

		// Token: 0x06004701 RID: 18177 RVA: 0x0025702C File Offset: 0x0025542C
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x04003041 RID: 12353
		public int ID = -1;

		// Token: 0x04003042 RID: 12354
		private List<Room> rooms = new List<Room>();

		// Token: 0x04003043 RID: 12355
		private RoomGroupTempTracker tempTracker;

		// Token: 0x04003044 RID: 12356
		private int cachedOpenRoofCount = -1;

		// Token: 0x04003045 RID: 12357
		private int cachedCellCount = -1;

		// Token: 0x04003046 RID: 12358
		private static int nextRoomGroupID;

		// Token: 0x04003047 RID: 12359
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;
	}
}
