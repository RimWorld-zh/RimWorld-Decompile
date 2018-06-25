using System;

namespace Verse
{
	// Token: 0x02000ADD RID: 2781
	public enum TraverseMode : byte
	{
		// Token: 0x040026CB RID: 9931
		ByPawn,
		// Token: 0x040026CC RID: 9932
		PassDoors,
		// Token: 0x040026CD RID: 9933
		NoPassClosedDoors,
		// Token: 0x040026CE RID: 9934
		PassAllDestroyableThings,
		// Token: 0x040026CF RID: 9935
		NoPassClosedDoorsOrWater,
		// Token: 0x040026D0 RID: 9936
		PassAllDestroyableThingsNotWater
	}
}
