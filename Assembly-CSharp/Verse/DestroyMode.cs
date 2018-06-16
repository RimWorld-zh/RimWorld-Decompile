using System;

namespace Verse
{
	// Token: 0x02000DF5 RID: 3573
	public enum DestroyMode : byte
	{
		// Token: 0x04003509 RID: 13577
		Vanish,
		// Token: 0x0400350A RID: 13578
		WillReplace,
		// Token: 0x0400350B RID: 13579
		KillFinalize,
		// Token: 0x0400350C RID: 13580
		Deconstruct,
		// Token: 0x0400350D RID: 13581
		FailConstruction,
		// Token: 0x0400350E RID: 13582
		Cancel,
		// Token: 0x0400350F RID: 13583
		Refund
	}
}
