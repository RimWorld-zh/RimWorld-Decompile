using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class RoomGroup
	{
		public int ID = -1;

		private List<Room> rooms = new List<Room>();

		private RoomGroupTempTracker tempTracker;

		private int cachedOpenRoofCount = -1;

		private int cachedCellCount = -1;

		private static int nextRoomGroupID;

		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		public Map Map
		{
			get
			{
				return (!this.rooms.Any()) ? null : this.rooms[0].Map;
			}
		}

		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

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

		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)((float)this.CellCount * 0.25));
			}
		}

		public IEnumerable<IntVec3> Cells
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					using (IEnumerator<IntVec3> enumerator = this.rooms[i].Cells.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							IntVec3 c = enumerator.Current;
							yield return c;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
			}
		}

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

		public IEnumerable<Region> Regions
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					using (List<Region>.Enumerator enumerator = this.rooms[i].Regions.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Region r = enumerator.Current;
							yield return r;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_00ff:
				/*Error near IL_0100: Unexpected return in MoveNext()*/;
			}
		}

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

		public bool AnyRoomTouchesMapEdge
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.rooms.Count)
					{
						if (this.rooms[num].TouchesMapEdge)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
			}
		}

		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		public void AddRoom(Room room)
		{
			if (this.rooms.Contains(room))
			{
				Log.Error("Tried to add the same room twice to RoomGroup. room=" + room + ", roomGroup=" + this);
			}
			else
			{
				this.rooms.Add(room);
			}
		}

		public void RemoveRoom(Room room)
		{
			if (!this.rooms.Contains(room))
			{
				Log.Error("Tried to remove room from RoomGroup but this room is not here. room=" + room + ", roomGroup=" + this);
			}
			else
			{
				this.rooms.Remove(room);
			}
		}

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

		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		public string DebugString()
		{
			return "RoomGroup ID=" + this.ID + "\n  first cell=" + this.Cells.FirstOrDefault() + "\n  RoomCount=" + this.RoomCount + "\n  RegionCount=" + this.RegionCount + "\n  CellCount=" + this.CellCount + "\n  OpenRoofCount=" + this.OpenRoofCount + "\n  " + this.tempTracker.DebugString();
		}

		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 cell in this.Cells)
			{
				CellRenderer.RenderCell(cell, (float)((float)num * 0.0099999997764825821));
			}
			this.tempTracker.DebugDraw();
		}
	}
}
