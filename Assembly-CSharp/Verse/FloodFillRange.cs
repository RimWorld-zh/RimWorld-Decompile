using System;

namespace Verse
{
	// Token: 0x02000C7F RID: 3199
	public struct FloodFillRange
	{
		// Token: 0x060045FB RID: 17915 RVA: 0x0024D403 File Offset: 0x0024B803
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04002FAE RID: 12206
		public int minX;

		// Token: 0x04002FAF RID: 12207
		public int maxX;

		// Token: 0x04002FB0 RID: 12208
		public int z;
	}
}
