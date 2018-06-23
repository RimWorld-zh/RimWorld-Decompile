using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CA8 RID: 3240
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x04003077 RID: 12407
		private Map map;

		// Token: 0x04003078 RID: 12408
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x04003079 RID: 12409
		public CachedTempInfo[] tempCache;

		// Token: 0x0400307A RID: 12410
		private HashSet<int> processedRoomGroupIDs = new HashSet<int>();

		// Token: 0x0400307B RID: 12411
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();

		// Token: 0x06004778 RID: 18296 RVA: 0x0025B49C File Offset: 0x0025989C
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0025B4F0 File Offset: 0x002598F0
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0025B534 File Offset: 0x00259934
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x0025B542 File Offset: 0x00259942
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0025B566 File Offset: 0x00259966
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0025B58C File Offset: 0x0025998C
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				RoomGroup group = room.Group;
				this.SetCachedCellInfo(c, new CachedTempInfo(group.ID, group.CellCount, group.Temperature));
			}
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x0025B5D0 File Offset: 0x002599D0
		public bool TryGetAverageCachedRoomGroupTemp(RoomGroup r, out float result)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in r.Cells)
			{
				CachedTempInfo item = this.map.temperatureCache.tempCache[cellIndices.CellToIndex(c)];
				if (item.numCells > 0 && !this.processedRoomGroupIDs.Contains(item.roomGroupID))
				{
					this.relevantTempInfoList.Add(item);
					this.processedRoomGroupIDs.Add(item.roomGroupID);
				}
			}
			int num = 0;
			float num2 = 0f;
			foreach (CachedTempInfo cachedTempInfo in this.relevantTempInfoList)
			{
				num += cachedTempInfo.numCells;
				num2 += cachedTempInfo.temperature * (float)cachedTempInfo.numCells;
			}
			result = num2 / (float)num;
			bool result2 = !this.relevantTempInfoList.NullOrEmpty<CachedTempInfo>();
			this.processedRoomGroupIDs.Clear();
			this.relevantTempInfoList.Clear();
			return result2;
		}
	}
}
