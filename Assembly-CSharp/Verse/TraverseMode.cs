using System;

namespace Verse
{
	// Token: 0x02000ADC RID: 2780
	public enum TraverseMode : byte
	{
		// Token: 0x040026C4 RID: 9924
		ByPawn,
		// Token: 0x040026C5 RID: 9925
		PassDoors,
		// Token: 0x040026C6 RID: 9926
		NoPassClosedDoors,
		// Token: 0x040026C7 RID: 9927
		PassAllDestroyableThings,
		// Token: 0x040026C8 RID: 9928
		NoPassClosedDoorsOrWater,
		// Token: 0x040026C9 RID: 9929
		PassAllDestroyableThingsNotWater
	}
}
