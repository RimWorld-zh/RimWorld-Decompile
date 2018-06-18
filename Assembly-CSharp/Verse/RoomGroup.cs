using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C9A RID: 3226
	public class RoomGroup
	{
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060046E4 RID: 18148 RVA: 0x00255738 File Offset: 0x00253B38
		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x060046E5 RID: 18149 RVA: 0x00255754 File Offset: 0x00253B54
		public Map Map
		{
			get
			{
				return (!this.rooms.Any<Room>()) ? null : this.rooms[0].Map;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060046E6 RID: 18150 RVA: 0x00255790 File Offset: 0x00253B90
		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060046E7 RID: 18151 RVA: 0x002557B0 File Offset: 0x00253BB0
		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x002557CC File Offset: 0x00253BCC
		// (set) Token: 0x060046E9 RID: 18153 RVA: 0x002557EC File Offset: 0x00253BEC
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

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060046EA RID: 18154 RVA: 0x002557FC File Offset: 0x00253BFC
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060046EB RID: 18155 RVA: 0x0025583C File Offset: 0x00253C3C
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

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x00255868 File Offset: 0x00253C68
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

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060046ED RID: 18157 RVA: 0x002558D4 File Offset: 0x00253CD4
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

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x00255900 File Offset: 0x00253D00
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

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060046EF RID: 18159 RVA: 0x0025594C File Offset: 0x00253D4C
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

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x002559B8 File Offset: 0x00253DB8
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

		// Token: 0x060046F1 RID: 18161 RVA: 0x00255A0C File Offset: 0x00253E0C
		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00255A4C File Offset: 0x00253E4C
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

		// Token: 0x060046F3 RID: 18163 RVA: 0x00255AA8 File Offset: 0x00253EA8
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

		// Token: 0x060046F4 RID: 18164 RVA: 0x00255B04 File Offset: 0x00253F04
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

		// Token: 0x060046F5 RID: 18165 RVA: 0x00255B43 File Offset: 0x00253F43
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x00255B58 File Offset: 0x00253F58
		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x00255B74 File Offset: 0x00253F74
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

		// Token: 0x060046F8 RID: 18168 RVA: 0x00255C3C File Offset: 0x0025403C
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x04003036 RID: 12342
		public int ID = -1;

		// Token: 0x04003037 RID: 12343
		private List<Room> rooms = new List<Room>();

		// Token: 0x04003038 RID: 12344
		private RoomGroupTempTracker tempTracker;

		// Token: 0x04003039 RID: 12345
		private int cachedOpenRoofCount = -1;

		// Token: 0x0400303A RID: 12346
		private int cachedCellCount = -1;

		// Token: 0x0400303B RID: 12347
		private static int nextRoomGroupID;

		// Token: 0x0400303C RID: 12348
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;
	}
}
