using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C9B RID: 3227
	public class RoomGroup
	{
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x060046E6 RID: 18150 RVA: 0x00255760 File Offset: 0x00253B60
		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060046E7 RID: 18151 RVA: 0x0025577C File Offset: 0x00253B7C
		public Map Map
		{
			get
			{
				return (!this.rooms.Any<Room>()) ? null : this.rooms[0].Map;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x002557B8 File Offset: 0x00253BB8
		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x060046E9 RID: 18153 RVA: 0x002557D8 File Offset: 0x00253BD8
		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060046EA RID: 18154 RVA: 0x002557F4 File Offset: 0x00253BF4
		// (set) Token: 0x060046EB RID: 18155 RVA: 0x00255814 File Offset: 0x00253C14
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
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x00255824 File Offset: 0x00253C24
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060046ED RID: 18157 RVA: 0x00255864 File Offset: 0x00253C64
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
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x00255890 File Offset: 0x00253C90
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
		// (get) Token: 0x060046EF RID: 18159 RVA: 0x002558FC File Offset: 0x00253CFC
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
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00255928 File Offset: 0x00253D28
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
		// (get) Token: 0x060046F1 RID: 18161 RVA: 0x00255974 File Offset: 0x00253D74
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
		// (get) Token: 0x060046F2 RID: 18162 RVA: 0x002559E0 File Offset: 0x00253DE0
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

		// Token: 0x060046F3 RID: 18163 RVA: 0x00255A34 File Offset: 0x00253E34
		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00255A74 File Offset: 0x00253E74
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

		// Token: 0x060046F5 RID: 18165 RVA: 0x00255AD0 File Offset: 0x00253ED0
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

		// Token: 0x060046F6 RID: 18166 RVA: 0x00255B2C File Offset: 0x00253F2C
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

		// Token: 0x060046F7 RID: 18167 RVA: 0x00255B6B File Offset: 0x00253F6B
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x00255B80 File Offset: 0x00253F80
		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x00255B9C File Offset: 0x00253F9C
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

		// Token: 0x060046FA RID: 18170 RVA: 0x00255C64 File Offset: 0x00254064
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x04003038 RID: 12344
		public int ID = -1;

		// Token: 0x04003039 RID: 12345
		private List<Room> rooms = new List<Room>();

		// Token: 0x0400303A RID: 12346
		private RoomGroupTempTracker tempTracker;

		// Token: 0x0400303B RID: 12347
		private int cachedOpenRoofCount = -1;

		// Token: 0x0400303C RID: 12348
		private int cachedCellCount = -1;

		// Token: 0x0400303D RID: 12349
		private static int nextRoomGroupID;

		// Token: 0x0400303E RID: 12350
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;
	}
}
