using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CAB RID: 3243
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x0600476F RID: 18287 RVA: 0x0025A0AC File Offset: 0x002584AC
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x0025A100 File Offset: 0x00258500
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x0025A144 File Offset: 0x00258544
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0025A152 File Offset: 0x00258552
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0025A176 File Offset: 0x00258576
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0025A19C File Offset: 0x0025859C
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				RoomGroup group = room.Group;
				this.SetCachedCellInfo(c, new CachedTempInfo(group.ID, group.CellCount, group.Temperature));
			}
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x0025A1E0 File Offset: 0x002585E0
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

		// Token: 0x0400306C RID: 12396
		private Map map;

		// Token: 0x0400306D RID: 12397
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x0400306E RID: 12398
		public CachedTempInfo[] tempCache;

		// Token: 0x0400306F RID: 12399
		private HashSet<int> processedRoomGroupIDs = new HashSet<int>();

		// Token: 0x04003070 RID: 12400
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
