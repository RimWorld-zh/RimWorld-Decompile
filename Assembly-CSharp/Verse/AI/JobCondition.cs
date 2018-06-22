using System;

namespace Verse.AI
{
	// Token: 0x02000A2F RID: 2607
	public enum JobCondition : byte
	{
		// Token: 0x040024F0 RID: 9456
		None,
		// Token: 0x040024F1 RID: 9457
		Ongoing,
		// Token: 0x040024F2 RID: 9458
		Succeeded,
		// Token: 0x040024F3 RID: 9459
		Incompletable,
		// Token: 0x040024F4 RID: 9460
		InterruptOptional,
		// Token: 0x040024F5 RID: 9461
		InterruptForced,
		// Token: 0x040024F6 RID: 9462
		QueuedNoLongerValid,
		// Token: 0x040024F7 RID: 9463
		Errored,
		// Token: 0x040024F8 RID: 9464
		ErroredPather
	}
}
