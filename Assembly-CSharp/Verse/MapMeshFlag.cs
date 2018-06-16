using System;

namespace Verse
{
	// Token: 0x02000C3F RID: 3135
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04002F2C RID: 12076
		None = 0,
		// Token: 0x04002F2D RID: 12077
		Things = 1,
		// Token: 0x04002F2E RID: 12078
		FogOfWar = 2,
		// Token: 0x04002F2F RID: 12079
		Buildings = 4,
		// Token: 0x04002F30 RID: 12080
		GroundGlow = 8,
		// Token: 0x04002F31 RID: 12081
		Terrain = 16,
		// Token: 0x04002F32 RID: 12082
		Roofs = 32,
		// Token: 0x04002F33 RID: 12083
		Snow = 64,
		// Token: 0x04002F34 RID: 12084
		Zone = 128,
		// Token: 0x04002F35 RID: 12085
		PowerGrid = 256,
		// Token: 0x04002F36 RID: 12086
		BuildingsDamage = 512
	}
}
