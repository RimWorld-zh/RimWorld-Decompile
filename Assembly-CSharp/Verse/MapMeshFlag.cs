using System;

namespace Verse
{
	// Token: 0x02000C3E RID: 3134
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04002F2A RID: 12074
		None = 0,
		// Token: 0x04002F2B RID: 12075
		Things = 1,
		// Token: 0x04002F2C RID: 12076
		FogOfWar = 2,
		// Token: 0x04002F2D RID: 12077
		Buildings = 4,
		// Token: 0x04002F2E RID: 12078
		GroundGlow = 8,
		// Token: 0x04002F2F RID: 12079
		Terrain = 16,
		// Token: 0x04002F30 RID: 12080
		Roofs = 32,
		// Token: 0x04002F31 RID: 12081
		Snow = 64,
		// Token: 0x04002F32 RID: 12082
		Zone = 128,
		// Token: 0x04002F33 RID: 12083
		PowerGrid = 256,
		// Token: 0x04002F34 RID: 12084
		BuildingsDamage = 512
	}
}
