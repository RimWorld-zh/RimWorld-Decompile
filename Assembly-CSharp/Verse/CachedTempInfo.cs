using System;

namespace Verse
{
	// Token: 0x02000CAB RID: 3243
	public struct CachedTempInfo
	{
		// Token: 0x0600476E RID: 18286 RVA: 0x0025A077 File Offset: 0x00258477
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x0025A090 File Offset: 0x00258490
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x0025A0B5 File Offset: 0x002584B5
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x0400306B RID: 12395
		public int roomGroupID;

		// Token: 0x0400306C RID: 12396
		public int numCells;

		// Token: 0x0400306D RID: 12397
		public float temperature;
	}
}
