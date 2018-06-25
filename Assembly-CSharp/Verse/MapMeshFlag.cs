using System;

namespace Verse
{
	// Token: 0x02000C3D RID: 3133
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04002F34 RID: 12084
		None = 0,
		// Token: 0x04002F35 RID: 12085
		Things = 1,
		// Token: 0x04002F36 RID: 12086
		FogOfWar = 2,
		// Token: 0x04002F37 RID: 12087
		Buildings = 4,
		// Token: 0x04002F38 RID: 12088
		GroundGlow = 8,
		// Token: 0x04002F39 RID: 12089
		Terrain = 16,
		// Token: 0x04002F3A RID: 12090
		Roofs = 32,
		// Token: 0x04002F3B RID: 12091
		Snow = 64,
		// Token: 0x04002F3C RID: 12092
		Zone = 128,
		// Token: 0x04002F3D RID: 12093
		PowerGrid = 256,
		// Token: 0x04002F3E RID: 12094
		BuildingsDamage = 512
	}
}
