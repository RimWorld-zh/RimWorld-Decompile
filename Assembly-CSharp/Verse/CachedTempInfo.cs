using System;

namespace Verse
{
	// Token: 0x02000CA9 RID: 3241
	public struct CachedTempInfo
	{
		// Token: 0x04003074 RID: 12404
		public int roomGroupID;

		// Token: 0x04003075 RID: 12405
		public int numCells;

		// Token: 0x04003076 RID: 12406
		public float temperature;

		// Token: 0x06004778 RID: 18296 RVA: 0x0025B51B File Offset: 0x0025991B
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0025B534 File Offset: 0x00259934
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0025B559 File Offset: 0x00259959
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}
	}
}
