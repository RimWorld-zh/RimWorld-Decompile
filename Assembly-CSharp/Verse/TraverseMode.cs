using System;

namespace Verse
{
	// Token: 0x02000ADE RID: 2782
	public enum TraverseMode : byte
	{
		// Token: 0x040026C8 RID: 9928
		ByPawn,
		// Token: 0x040026C9 RID: 9929
		PassDoors,
		// Token: 0x040026CA RID: 9930
		NoPassClosedDoors,
		// Token: 0x040026CB RID: 9931
		PassAllDestroyableThings,
		// Token: 0x040026CC RID: 9932
		NoPassClosedDoorsOrWater,
		// Token: 0x040026CD RID: 9933
		PassAllDestroyableThingsNotWater
	}
}
