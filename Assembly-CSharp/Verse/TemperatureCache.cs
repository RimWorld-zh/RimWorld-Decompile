using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CAC RID: 3244
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x06004771 RID: 18289 RVA: 0x0025A0D4 File Offset: 0x002584D4
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0025A128 File Offset: 0x00258528
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0025A16C File Offset: 0x0025856C
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0025A17A File Offset: 0x0025857A
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x0025A19E File Offset: 0x0025859E
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0025A1C4 File Offset: 0x002585C4
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				RoomGroup group = room.Group;
				this.SetCachedCellInfo(c, new CachedTempInfo(group.ID, group.CellCount, group.Temperature));
			}
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0025A208 File Offset: 0x00258608
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

		// Token: 0x0400306E RID: 12398
		private Map map;

		// Token: 0x0400306F RID: 12399
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x04003070 RID: 12400
		public CachedTempInfo[] tempCache;

		// Token: 0x04003071 RID: 12401
		private HashSet<int> processedRoomGroupIDs = new HashSet<int>();

		// Token: 0x04003072 RID: 12402
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
