using System;

namespace Verse
{
	// Token: 0x02000DF4 RID: 3572
	public enum DestroyMode : byte
	{
		// Token: 0x04003507 RID: 13575
		Vanish,
		// Token: 0x04003508 RID: 13576
		WillReplace,
		// Token: 0x04003509 RID: 13577
		KillFinalize,
		// Token: 0x0400350A RID: 13578
		Deconstruct,
		// Token: 0x0400350B RID: 13579
		FailConstruction,
		// Token: 0x0400350C RID: 13580
		Cancel,
		// Token: 0x0400350D RID: 13581
		Refund
	}
}
