using System;

namespace Verse
{
	// Token: 0x02000DF4 RID: 3572
	public enum DestroyMode : byte
	{
		// Token: 0x04003515 RID: 13589
		Vanish,
		// Token: 0x04003516 RID: 13590
		WillReplace,
		// Token: 0x04003517 RID: 13591
		KillFinalize,
		// Token: 0x04003518 RID: 13592
		Deconstruct,
		// Token: 0x04003519 RID: 13593
		FailConstruction,
		// Token: 0x0400351A RID: 13594
		Cancel,
		// Token: 0x0400351B RID: 13595
		Refund
	}
}
