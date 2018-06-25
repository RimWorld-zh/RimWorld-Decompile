using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C89 RID: 3209
	public class RegionAndRoomUpdater
	{
		// Token: 0x04002FEE RID: 12270
		private Map map;

		// Token: 0x04002FEF RID: 12271
		private List<Region> newRegions = new List<Region>();

		// Token: 0x04002FF0 RID: 12272
		private List<Room> newRooms = new List<Room>();

		// Token: 0x04002FF1 RID: 12273
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x04002FF2 RID: 12274
		private List<RoomGroup> newRoomGroups = new List<RoomGroup>();

		// Token: 0x04002FF3 RID: 12275
		private HashSet<RoomGroup> reusedOldRoomGroups = new HashSet<RoomGroup>();

		// Token: 0x04002FF4 RID: 12276
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x04002FF5 RID: 12277
		private List<Room> currentRoomGroup = new List<Room>();

		// Token: 0x04002FF6 RID: 12278
		private Stack<Room> tmpRoomStack = new Stack<Room>();

		// Token: 0x04002FF7 RID: 12279
		private HashSet<Room> tmpVisitedRooms = new HashSet<Room>();

		// Token: 0x04002FF8 RID: 12280
		private bool initialized = false;

		// Token: 0x04002FF9 RID: 12281
		private bool working = false;

		// Token: 0x04002FFA RID: 12282
		private bool enabledInt = true;

		// Token: 0x0600465D RID: 18013 RVA: 0x00251CB8 File Offset: 0x002500B8
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600465E RID: 18014 RVA: 0x00251D4C File Offset: 0x0025014C
		// (set) Token: 0x0600465F RID: 18015 RVA: 0x00251D67 File Offset: 0x00250167
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

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06004660 RID: 18016 RVA: 0x00251D74 File Offset: 0x00250174
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00251DAA File Offset: 0x002501AA
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

		// Token: 0x06004662 RID: 18018 RVA: 0x00251DEC File Offset: 0x002501EC
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

		// Token: 0x06004663 RID: 18019 RVA: 0x00251ECC File Offset: 0x002502CC
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

		// Token: 0x06004664 RID: 18020 RVA: 0x00251F58 File Offset: 0x00250358
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

		// Token: 0x06004665 RID: 18021 RVA: 0x00251FE0 File Offset: 0x002503E0
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

		// Token: 0x06004666 RID: 18022 RVA: 0x00252048 File Offset: 0x00250448
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

		// Token: 0x06004667 RID: 18023 RVA: 0x002521E8 File Offset: 0x002505E8
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

		// Token: 0x06004668 RID: 18024 RVA: 0x0025238C File Offset: 0x0025078C
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

		// Token: 0x06004669 RID: 18025 RVA: 0x00252528 File Offset: 0x00250928
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

		// Token: 0x0600466A RID: 18026 RVA: 0x0025262C File Offset: 0x00250A2C
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

		// Token: 0x0600466B RID: 18027 RVA: 0x00252764 File Offset: 0x00250B64
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

		// Token: 0x0600466C RID: 18028 RVA: 0x00252860 File Offset: 0x00250C60
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

		// Token: 0x0600466D RID: 18029 RVA: 0x00252974 File Offset: 0x00250D74
		private bool ShouldBeInTheSameRoomGroup(Room a, Room b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange);
		}
	}
}
