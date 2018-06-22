using System;

namespace Verse
{
	// Token: 0x02000CA7 RID: 3239
	public struct CachedTempInfo
	{
		// Token: 0x06004775 RID: 18293 RVA: 0x0025B43F File Offset: 0x0025983F
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0025B458 File Offset: 0x00259858
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0025B47D File Offset: 0x0025987D
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x04003074 RID: 12404
		public int roomGroupID;

		// Token: 0x04003075 RID: 12405
		public int numCells;

		// Token: 0x04003076 RID: 12406
		public float temperature;
	}
}
