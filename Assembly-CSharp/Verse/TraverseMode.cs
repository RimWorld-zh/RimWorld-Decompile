using System;

namespace Verse
{
	// Token: 0x02000ADA RID: 2778
	public enum TraverseMode : byte
	{
		// Token: 0x040026C3 RID: 9923
		ByPawn,
		// Token: 0x040026C4 RID: 9924
		PassDoors,
		// Token: 0x040026C5 RID: 9925
		NoPassClosedDoors,
		// Token: 0x040026C6 RID: 9926
		PassAllDestroyableThings,
		// Token: 0x040026C7 RID: 9927
		NoPassClosedDoorsOrWater,
		// Token: 0x040026C8 RID: 9928
		PassAllDestroyableThingsNotWater
	}
}
