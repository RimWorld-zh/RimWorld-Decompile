using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CAB RID: 3243
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x0400307E RID: 12414
		private Map map;

		// Token: 0x0400307F RID: 12415
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x04003080 RID: 12416
		public CachedTempInfo[] tempCache;

		// Token: 0x04003081 RID: 12417
		private HashSet<int> processedRoomGroupIDs = new HashSet<int>();

		// Token: 0x04003082 RID: 12418
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();

		// Token: 0x0600477B RID: 18299 RVA: 0x0025B858 File Offset: 0x00259C58
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x0025B8AC File Offset: 0x00259CAC
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0025B8F0 File Offset: 0x00259CF0
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x0025B8FE File Offset: 0x00259CFE
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x0025B922 File Offset: 0x00259D22
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0025B948 File Offset: 0x00259D48
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				RoomGroup group = room.Group;
				this.SetCachedCellInfo(c, new CachedTempInfo(group.ID, group.CellCount, group.Temperature));
			}
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0025B98C File Offset: 0x00259D8C
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
