using System;

namespace Verse
{
	// Token: 0x02000C3D RID: 3133
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04002F1D RID: 12061
		None = 0,
		// Token: 0x04002F1E RID: 12062
		Verts = 1,
		// Token: 0x04002F1F RID: 12063
		Tris = 2,
		// Token: 0x04002F20 RID: 12064
		Colors = 4,
		// Token: 0x04002F21 RID: 12065
		UVs = 8,
		// Token: 0x04002F22 RID: 12066
		All = 127
	}
}
