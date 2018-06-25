using System;

namespace Verse
{
	// Token: 0x02000CAA RID: 3242
	public struct CachedTempInfo
	{
		// Token: 0x0400307B RID: 12411
		public int roomGroupID;

		// Token: 0x0400307C RID: 12412
		public int numCells;

		// Token: 0x0400307D RID: 12413
		public float temperature;

		// Token: 0x06004778 RID: 18296 RVA: 0x0025B7FB File Offset: 0x00259BFB
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0025B814 File Offset: 0x00259C14
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0025B839 File Offset: 0x00259C39
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}
	}
}
