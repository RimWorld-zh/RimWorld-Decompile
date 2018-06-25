using System;

namespace Verse
{
	// Token: 0x02000C7E RID: 3198
	public struct FloodFillRange
	{
		// Token: 0x04002FBD RID: 12221
		public int minX;

		// Token: 0x04002FBE RID: 12222
		public int maxX;

		// Token: 0x04002FBF RID: 12223
		public int z;

		// Token: 0x06004605 RID: 17925 RVA: 0x0024EB67 File Offset: 0x0024CF67
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}
	}
}
