using System;

namespace Verse
{
	// Token: 0x02000C3C RID: 3132
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04002F1B RID: 12059
		None = 0,
		// Token: 0x04002F1C RID: 12060
		Verts = 1,
		// Token: 0x04002F1D RID: 12061
		Tris = 2,
		// Token: 0x04002F1E RID: 12062
		Colors = 4,
		// Token: 0x04002F1F RID: 12063
		UVs = 8,
		// Token: 0x04002F20 RID: 12064
		All = 127
	}
}
