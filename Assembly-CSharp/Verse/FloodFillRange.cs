using System;

namespace Verse
{
	// Token: 0x02000C7D RID: 3197
	public struct FloodFillRange
	{
		// Token: 0x04002FB6 RID: 12214
		public int minX;

		// Token: 0x04002FB7 RID: 12215
		public int maxX;

		// Token: 0x04002FB8 RID: 12216
		public int z;

		// Token: 0x06004605 RID: 17925 RVA: 0x0024E887 File Offset: 0x0024CC87
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}
	}
}
