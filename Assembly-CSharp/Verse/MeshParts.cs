using System;

namespace Verse
{
	// Token: 0x02000C3B RID: 3131
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04002F25 RID: 12069
		None = 0,
		// Token: 0x04002F26 RID: 12070
		Verts = 1,
		// Token: 0x04002F27 RID: 12071
		Tris = 2,
		// Token: 0x04002F28 RID: 12072
		Colors = 4,
		// Token: 0x04002F29 RID: 12073
		UVs = 8,
		// Token: 0x04002F2A RID: 12074
		All = 127
	}
}
