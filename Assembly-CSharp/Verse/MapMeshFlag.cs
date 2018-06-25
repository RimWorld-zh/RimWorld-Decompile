using System;

namespace Verse
{
	// Token: 0x02000C3E RID: 3134
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04002F3B RID: 12091
		None = 0,
		// Token: 0x04002F3C RID: 12092
		Things = 1,
		// Token: 0x04002F3D RID: 12093
		FogOfWar = 2,
		// Token: 0x04002F3E RID: 12094
		Buildings = 4,
		// Token: 0x04002F3F RID: 12095
		GroundGlow = 8,
		// Token: 0x04002F40 RID: 12096
		Terrain = 16,
		// Token: 0x04002F41 RID: 12097
		Roofs = 32,
		// Token: 0x04002F42 RID: 12098
		Snow = 64,
		// Token: 0x04002F43 RID: 12099
		Zone = 128,
		// Token: 0x04002F44 RID: 12100
		PowerGrid = 256,
		// Token: 0x04002F45 RID: 12101
		BuildingsDamage = 512
	}
}
