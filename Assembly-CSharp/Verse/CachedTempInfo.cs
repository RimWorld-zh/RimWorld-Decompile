using System;

namespace Verse
{
	// Token: 0x02000CAA RID: 3242
	public struct CachedTempInfo
	{
		// Token: 0x0600476C RID: 18284 RVA: 0x0025A04F File Offset: 0x0025844F
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x0025A068 File Offset: 0x00258468
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x0025A08D File Offset: 0x0025848D
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x04003069 RID: 12393
		public int roomGroupID;

		// Token: 0x0400306A RID: 12394
		public int numCells;

		// Token: 0x0400306B RID: 12395
		public float temperature;
	}
}
