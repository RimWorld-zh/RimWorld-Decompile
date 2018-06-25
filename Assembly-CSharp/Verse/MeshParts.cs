using System;

namespace Verse
{
	// Token: 0x02000C3C RID: 3132
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04002F2C RID: 12076
		None = 0,
		// Token: 0x04002F2D RID: 12077
		Verts = 1,
		// Token: 0x04002F2E RID: 12078
		Tris = 2,
		// Token: 0x04002F2F RID: 12079
		Colors = 4,
		// Token: 0x04002F30 RID: 12080
		UVs = 8,
		// Token: 0x04002F31 RID: 12081
		All = 127
	}
}
