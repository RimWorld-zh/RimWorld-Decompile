using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C8A RID: 3210
	public class RegionAndRoomUpdater
	{
		// Token: 0x06004651 RID: 18001 RVA: 0x0025080C File Offset: 0x0024EC0C
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x002508A0 File Offset: 0x0024ECA0
		// (set) Token: 0x06004653 RID: 18003 RVA: 0x002508BB File Offset: 0x0024ECBB
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

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06004654 RID: 18004 RVA: 0x002508C8 File Offset: 0x0024ECC8
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x002508FE File Offset: 0x0024ECFE
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.", false);
			}
			this.map.temperatureCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00250940 File Offset: 0x0024ED40
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
					try
					{
						this.RegenerateNewRegionsFromDirtyCells();
						this.CreateOrUpdateRooms();
					}
					catch (Exception arg)
					{
						Log.Error("Exception while rebuilding dirty regions: " + arg, false);
					}
					this.newRegions.Clear();
					this.map.regionDirtyer.SetAllClean();
					this.initialized = true;
					this.working = false;
					if (DebugSettings.detectRegionListersBugs)
					{
						Autotests_RegionListers.CheckBugs(this.map);
					}
				}
			}
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00250A20 File Offset: 0x0024EE20
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

		// Token: 0x06004658 RID: 18008 RVA: 0x00250AAC File Offset: 0x0024EEAC
		private void CreateOrUpdateRooms()
		{
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingRooms(numRegionGroups);
			int numRoomGroups = this.CombineNewAndReusedRoomsIntoContiguousGroups();
			this.CreateOrAttachToExistingRoomGroups(numRoomGroups);
			this.NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00250B34 File Offset: 0x0024EF34
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

		// Token: 0x0600465A RID: 18010 RVA: 0x00250B9C File Offset: 0x0024EF9C
		private void CreateOrAttachToExistingRooms(int numRegionGroups)
		{
			for (int i = 0; i < numRegionGroups; i++)
			{
				this.currentRegionGroup.Clear();
				for (int j = 0; j < this.newRegions.Count; j++)
				{
					if (this.newRegions[j].newRegionGroupIndex == i)
					{
						this.currentRegionGroup.Add(this.newRegions[j]);
					}
				}
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerRoom())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.", false);
					}
					Room room = Room.MakeNew(this.map);
					this.currentRegionGroup[0].Room = room;
					this.newRooms.Add(room);
				}
				else
				{
					bool flag;
					Room room2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (room2 == null)
					{
						Room item = RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, null);
						this.newRooms.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].Room = room2;
						}
						this.reusedOldRooms.Add(room2);
					}
					else
					{
						RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, room2);
						this.reusedOldRooms.Add(room2);
					}
				}
			}
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00250D3C File Offset: 0x0024F13C
		private int CombineNewAndReusedRoomsIntoContiguousGroups()
		{
			int num = 0;
			foreach (Room room in this.reusedOldRooms)
			{
				room.newOrReusedRoomGroupIndex = -1;
			}
			foreach (Room room2 in this.reusedOldRooms.Concat(this.newRooms))
			{
				if (room2.newOrReusedRoomGroupIndex < 0)
				{
					this.tmpRoomStack.Clear();
					this.tmpRoomStack.Push(room2);
					room2.newOrReusedRoomGroupIndex = num;
					while (this.tmpRoomStack.Count != 0)
					{
						Room room3 = this.tmpRoomStack.Pop();
						foreach (Room room4 in room3.Neighbors)
						{
							if (room4.newOrReusedRoomGroupIndex < 0 && this.ShouldBeInTheSameRoomGroup(room3, room4))
							{
								room4.newOrReusedRoomGroupIndex = num;
								this.tmpRoomStack.Push(room4);
							}
						}
					}
					this.tmpRoomStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00250EE0 File Offset: 0x0024F2E0
		private void CreateOrAttachToExistingRoomGroups(int numRoomGroups)
		{
			for (int i = 0; i < numRoomGroups; i++)
			{
				this.currentRoomGroup.Clear();
				foreach (Room room in this.reusedOldRooms)
				{
					if (room.newOrReusedRoomGroupIndex == i)
					{
						this.currentRoomGroup.Add(room);
					}
				}
				for (int j = 0; j < this.newRooms.Count; j++)
				{
					if (this.newRooms[j].newOrReusedRoomGroupIndex == i)
					{
						this.currentRoomGroup.Add(this.newRooms[j]);
					}
				}
				bool flag;
				RoomGroup roomGroup = this.FindCurrentRoomGroupNeighborWithMostRegions(out flag);
				if (roomGroup == null)
				{
					RoomGroup roomGroup2 = RoomGroup.MakeNew(this.map);
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup2);
					this.newRoomGroups.Add(roomGroup2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentRoomGroup.Count; k++)
					{
						this.currentRoomGroup[k].Group = roomGroup;
					}
					this.reusedOldRoomGroups.Add(roomGroup);
				}
				else
				{
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup);
					this.reusedOldRoomGroups.Add(roomGroup);
				}
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x0025107C File Offset: 0x0024F47C
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
				foreach (Room room2 in room.Neighbors)
				{
					if (!this.tmpVisitedRooms.Contains(room2) && this.ShouldBeInTheSameRoomGroup(room, room2))
					{
						this.tmpRoomStack.Push(room2);
						this.tmpVisitedRooms.Add(room2);
					}
				}
			}
			this.tmpVisitedRooms.Clear();
			this.tmpRoomStack.Clear();
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00251180 File Offset: 0x0024F580
		private void NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature()
		{
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newRooms.Count; i++)
			{
				this.newRooms[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (RoomGroup roomGroup in this.reusedOldRoomGroups)
			{
				roomGroup.Notify_RoomGroupShapeChanged();
			}
			for (int j = 0; j < this.newRoomGroups.Count; j++)
			{
				RoomGroup roomGroup2 = this.newRoomGroups[j];
				roomGroup2.Notify_RoomGroupShapeChanged();
				float temperature;
				if (this.map.temperatureCache.TryGetAverageCachedRoomGroupTemp(roomGroup2, out temperature))
				{
					roomGroup2.Temperature = temperature;
				}
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x002512B8 File Offset: 0x0024F6B8
		private Room FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.Room != null)
					{
						if (!this.reusedOldRooms.Contains(region.Room))
						{
							if (room == null)
							{
								room = region.Room;
							}
							else if (region.Room != room)
							{
								multipleOldNeighborRooms = true;
								if (region.Room.RegionCount > room.RegionCount)
								{
									room = region.Room;
								}
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x002513B4 File Offset: 0x0024F7B4
		private RoomGroup FindCurrentRoomGroupNeighborWithMostRegions(out bool multipleOldNeighborRoomGroups)
		{
			multipleOldNeighborRoomGroups = false;
			RoomGroup roomGroup = null;
			for (int i = 0; i < this.currentRoomGroup.Count; i++)
			{
				foreach (Room room in this.currentRoomGroup[i].Neighbors)
				{
					if (room.Group != null && this.ShouldBeInTheSameRoomGroup(this.currentRoomGroup[i], room))
					{
						if (!this.reusedOldRoomGroups.Contains(room.Group))
						{
							if (roomGroup == null)
							{
								roomGroup = room.Group;
							}
							else if (room.Group != roomGroup)
							{
								multipleOldNeighborRoomGroups = true;
								if (room.Group.RegionCount > roomGroup.RegionCount)
								{
									roomGroup = room.Group;
								}
							}
						}
					}
				}
			}
			return roomGroup;
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x002514C8 File Offset: 0x0024F8C8
		private bool ShouldBeInTheSameRoomGroup(Room a, Room b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange);
		}

		// Token: 0x04002FE4 RID: 12260
		private Map map;

		// Token: 0x04002FE5 RID: 12261
		private List<Region> newRegions = new List<Region>();

		// Token: 0x04002FE6 RID: 12262
		private List<Room> newRooms = new List<Room>();

		// Token: 0x04002FE7 RID: 12263
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x04002FE8 RID: 12264
		private List<RoomGroup> newRoomGroups = new List<RoomGroup>();

		// Token: 0x04002FE9 RID: 12265
		private HashSet<RoomGroup> reusedOldRoomGroups = new HashSet<RoomGroup>();

		// Token: 0x04002FEA RID: 12266
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x04002FEB RID: 12267
		private List<Room> currentRoomGroup = new List<Room>();

		// Token: 0x04002FEC RID: 12268
		private Stack<Room> tmpRoomStack = new Stack<Room>();

		// Token: 0x04002FED RID: 12269
		private HashSet<Room> tmpVisitedRooms = new HashSet<Room>();

		// Token: 0x04002FEE RID: 12270
		private bool initialized = false;

		// Token: 0x04002FEF RID: 12271
		private bool working = false;

		// Token: 0x04002FF0 RID: 12272
		private bool enabledInt = true;
	}
}
