using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class RegionAndRoomUpdater
	{
		private Map map;

		private List<Region> newRegions = new List<Region>();

		private List<Room> newRooms = new List<Room>();

		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		private List<RoomGroup> newRoomGroups = new List<RoomGroup>();

		private HashSet<RoomGroup> reusedOldRoomGroups = new HashSet<RoomGroup>();

		private List<Region> currentRegionGroup = new List<Region>();

		private List<Room> currentRoomGroup = new List<Room>();

		private Stack<Room> tmpRoomStack = new Stack<Room>();

		private HashSet<Room> tmpVisitedRooms = new HashSet<Room>();

		private bool initialized;

		private bool working;

		private bool enabledInt = true;

		public bool Enabled
		{
			get
			{
				return this.enabledInt;
			}
			set
			{
				this.enabledInt = value;
			}
		}

		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.");
			}
			this.map.temperatureCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		public void TryRebuildDirtyRegionsAndRooms()
		{
			if (!this.working && this.Enabled)
			{
				this.working = true;
				if (!this.initialized)
				{
					this.RebuildAllRegionsAndRooms();
				}
				if (!this.map.regionDirtyer.AnyDirty)
				{
					this.working = false;
				}
				else
				{
					ProfilerThreadCheck.BeginSample("RebuildDirtyRegionsAndRooms");
					try
					{
						ProfilerThreadCheck.BeginSample("Regenerate new regions from dirty cells");
						this.RegenerateNewRegionsFromDirtyCells();
						ProfilerThreadCheck.EndSample();
						ProfilerThreadCheck.BeginSample("Create or update rooms");
						this.CreateOrUpdateRooms();
						ProfilerThreadCheck.EndSample();
					}
					catch (Exception arg)
					{
						Log.Error("Exception while rebuilding dirty regions: " + arg);
					}
					this.newRegions.Clear();
					this.map.regionDirtyer.SetAllClean();
					this.initialized = true;
					this.working = false;
					ProfilerThreadCheck.EndSample();
					if (DebugSettings.detectRegionListersBugs)
					{
						Autotests_RegionListers.CheckBugs(this.map);
					}
				}
			}
		}

		private void RegenerateNewRegionsFromDirtyCells()
		{
			this.newRegions.Clear();
			List<IntVec3> dirtyCells = this.map.regionDirtyer.DirtyCells;
			for (int i = 0; i < dirtyCells.Count; i++)
			{
				IntVec3 intVec = dirtyCells[i];
				if (intVec.GetRegion(this.map, RegionType.Set_All) == null)
				{
					Region region = this.map.regionMaker.TryGenerateRegionFrom(intVec);
					if (region != null)
					{
						this.newRegions.Add(region);
					}
				}
			}
		}

		private void CreateOrUpdateRooms()
		{
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
			ProfilerThreadCheck.BeginSample("Combine new regions into contiguous groups");
			int num = this.CombineNewRegionsIntoContiguousGroups();
			ProfilerThreadCheck.EndSample();
			ProfilerThreadCheck.BeginSample("Process " + num + " new region groups");
			this.CreateOrAttachToExistingRooms(num);
			ProfilerThreadCheck.EndSample();
			ProfilerThreadCheck.BeginSample("Combine new and reused rooms into contiguous groups");
			int num2 = this.CombineNewAndReusedRoomsIntoContiguousGroups();
			ProfilerThreadCheck.EndSample();
			ProfilerThreadCheck.BeginSample("Process " + num2 + " new room groups");
			this.CreateOrAttachToExistingRoomGroups(num2);
			ProfilerThreadCheck.EndSample();
			this.NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
		}

		private int CombineNewRegionsIntoContiguousGroups()
		{
			int num = 0;
			for (int i = 0; i < this.newRegions.Count; i++)
			{
				if (this.newRegions[i].newRegionGroupIndex < 0)
				{
					RegionTraverser.FloodAndSetNewRegionIndex(this.newRegions[i], num);
					num++;
				}
			}
			return num;
		}

		private void CreateOrAttachToExistingRooms(int numRegionGroups)
		{
			for (int num = 0; num < numRegionGroups; num++)
			{
				ProfilerThreadCheck.BeginSample("Remake currentRegionGroup list");
				this.currentRegionGroup.Clear();
				for (int i = 0; i < this.newRegions.Count; i++)
				{
					if (this.newRegions[i].newRegionGroupIndex == num)
					{
						this.currentRegionGroup.Add(this.newRegions[i]);
					}
				}
				ProfilerThreadCheck.EndSample();
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerRoom())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.");
					}
					Room room = Room.MakeNew(this.map);
					this.currentRegionGroup[0].Room = room;
					this.newRooms.Add(room);
				}
				else
				{
					ProfilerThreadCheck.BeginSample("Determine neighboring old rooms");
					bool flag = default(bool);
					Room room2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					ProfilerThreadCheck.EndSample();
					ProfilerThreadCheck.BeginSample("Apply final result");
					if (room2 == null)
					{
						Room item = RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, null);
						this.newRooms.Add(item);
					}
					else if (!flag)
					{
						for (int j = 0; j < this.currentRegionGroup.Count; j++)
						{
							this.currentRegionGroup[j].Room = room2;
						}
						this.reusedOldRooms.Add(room2);
					}
					else
					{
						RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, room2);
						this.reusedOldRooms.Add(room2);
					}
					ProfilerThreadCheck.EndSample();
				}
			}
		}

		private int CombineNewAndReusedRoomsIntoContiguousGroups()
		{
			int num = 0;
			HashSet<Room>.Enumerator enumerator = this.reusedOldRooms.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Room current = enumerator.Current;
					current.newOrReusedRoomGroupIndex = -1;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			foreach (Room item in this.reusedOldRooms.Concat(this.newRooms))
			{
				if (item.newOrReusedRoomGroupIndex < 0)
				{
					this.tmpRoomStack.Clear();
					this.tmpRoomStack.Push(item);
					item.newOrReusedRoomGroupIndex = num;
					while (this.tmpRoomStack.Count != 0)
					{
						Room room = this.tmpRoomStack.Pop();
						List<Room>.Enumerator enumerator3 = room.Neighbors.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								Room current3 = enumerator3.Current;
								if (current3.newOrReusedRoomGroupIndex < 0 && this.ShouldBeInTheSameRoomGroup(room, current3))
								{
									current3.newOrReusedRoomGroupIndex = num;
									this.tmpRoomStack.Push(current3);
								}
							}
						}
						finally
						{
							((IDisposable)(object)enumerator3).Dispose();
						}
					}
					this.tmpRoomStack.Clear();
					num++;
				}
			}
			return num;
		}

		private void CreateOrAttachToExistingRoomGroups(int numRoomGroups)
		{
			for (int num = 0; num < numRoomGroups; num++)
			{
				ProfilerThreadCheck.BeginSample("Remake currentRoomGroup list");
				this.currentRoomGroup.Clear();
				HashSet<Room>.Enumerator enumerator = this.reusedOldRooms.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Room current = enumerator.Current;
						if (current.newOrReusedRoomGroupIndex == num)
						{
							this.currentRoomGroup.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				for (int i = 0; i < this.newRooms.Count; i++)
				{
					if (this.newRooms[i].newOrReusedRoomGroupIndex == num)
					{
						this.currentRoomGroup.Add(this.newRooms[i]);
					}
				}
				ProfilerThreadCheck.EndSample();
				ProfilerThreadCheck.BeginSample("Determine neighboring old room groups");
				bool flag = default(bool);
				RoomGroup roomGroup = this.FindCurrentRoomGroupNeighborWithMostRegions(out flag);
				ProfilerThreadCheck.EndSample();
				ProfilerThreadCheck.BeginSample("Apply final result");
				if (roomGroup == null)
				{
					RoomGroup roomGroup2 = RoomGroup.MakeNew(this.map);
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup2);
					this.newRoomGroups.Add(roomGroup2);
				}
				else if (!flag)
				{
					for (int j = 0; j < this.currentRoomGroup.Count; j++)
					{
						this.currentRoomGroup[j].Group = roomGroup;
					}
					this.reusedOldRoomGroups.Add(roomGroup);
				}
				else
				{
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup);
					this.reusedOldRoomGroups.Add(roomGroup);
				}
				ProfilerThreadCheck.EndSample();
			}
		}

		private void FloodAndSetRoomGroups(Room start, RoomGroup roomGroup)
		{
			this.tmpRoomStack.Clear();
			this.tmpRoomStack.Push(start);
			this.tmpVisitedRooms.Clear();
			this.tmpVisitedRooms.Add(start);
			while (this.tmpRoomStack.Count != 0)
			{
				Room room = this.tmpRoomStack.Pop();
				room.Group = roomGroup;
				List<Room>.Enumerator enumerator = room.Neighbors.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Room current = enumerator.Current;
						if (!this.tmpVisitedRooms.Contains(current) && this.ShouldBeInTheSameRoomGroup(room, current))
						{
							this.tmpRoomStack.Push(current);
							this.tmpVisitedRooms.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			this.tmpVisitedRooms.Clear();
			this.tmpRoomStack.Clear();
		}

		private void NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature()
		{
			HashSet<Room>.Enumerator enumerator = this.reusedOldRooms.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Room current = enumerator.Current;
					current.Notify_RoomShapeOrContainedBedsChanged();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			for (int i = 0; i < this.newRooms.Count; i++)
			{
				this.newRooms[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			HashSet<RoomGroup>.Enumerator enumerator2 = this.reusedOldRoomGroups.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					RoomGroup current2 = enumerator2.Current;
					current2.Notify_RoomGroupShapeChanged();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			for (int j = 0; j < this.newRoomGroups.Count; j++)
			{
				RoomGroup roomGroup = this.newRoomGroups[j];
				roomGroup.Notify_RoomGroupShapeChanged();
				float temperature = default(float);
				if (this.map.temperatureCache.TryGetAverageCachedRoomGroupTemp(roomGroup, out temperature))
				{
					roomGroup.Temperature = temperature;
				}
			}
		}

		private Room FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region item in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (item.Room != null && !this.reusedOldRooms.Contains(item.Room))
					{
						if (room == null)
						{
							room = item.Room;
						}
						else if (item.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (item.Room.RegionCount > room.RegionCount)
							{
								room = item.Room;
							}
						}
					}
				}
			}
			return room;
		}

		private RoomGroup FindCurrentRoomGroupNeighborWithMostRegions(out bool multipleOldNeighborRoomGroups)
		{
			multipleOldNeighborRoomGroups = false;
			RoomGroup roomGroup = null;
			for (int i = 0; i < this.currentRoomGroup.Count; i++)
			{
				List<Room>.Enumerator enumerator = this.currentRoomGroup[i].Neighbors.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Room current = enumerator.Current;
						if (current.Group != null && this.ShouldBeInTheSameRoomGroup(this.currentRoomGroup[i], current) && !this.reusedOldRoomGroups.Contains(current.Group))
						{
							if (roomGroup == null)
							{
								roomGroup = current.Group;
							}
							else if (current.Group != roomGroup)
							{
								multipleOldNeighborRoomGroups = true;
								if (current.Group.RegionCount > roomGroup.RegionCount)
								{
									roomGroup = current.Group;
								}
							}
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			return roomGroup;
		}

		private bool ShouldBeInTheSameRoomGroup(Room a, Room b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange);
		}
	}
}
