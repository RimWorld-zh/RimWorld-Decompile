using System;

namespace Verse
{
	// Token: 0x02000DF1 RID: 3569
	public enum DestroyMode : byte
	{
		// Token: 0x0400350E RID: 13582
		Vanish,
		// Token: 0x0400350F RID: 13583
		WillReplace,
		// Token: 0x04003510 RID: 13584
		KillFinalize,
		// Token: 0x04003511 RID: 13585
		Deconstruct,
		// Token: 0x04003512 RID: 13586
		FailConstruction,
		// Token: 0x04003513 RID: 13587
		Cancel,
		// Token: 0x04003514 RID: 13588
		Refund
	}
}
