using System;

namespace Verse
{
	// Token: 0x02000C7E RID: 3198
	public struct FloodFillRange
	{
		// Token: 0x060045F9 RID: 17913 RVA: 0x0024D3DB File Offset: 0x0024B7DB
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04002FAC RID: 12204
		public int minX;

		// Token: 0x04002FAD RID: 12205
		public int maxX;

		// Token: 0x04002FAE RID: 12206
		public int z;
	}
}
