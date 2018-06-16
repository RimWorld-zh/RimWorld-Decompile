using System;

namespace Verse.AI
{
	// Token: 0x02000A33 RID: 2611
	public enum JobCondition : byte
	{
		// Token: 0x040024F5 RID: 9461
		None,
		// Token: 0x040024F6 RID: 9462
		Ongoing,
		// Token: 0x040024F7 RID: 9463
		Succeeded,
		// Token: 0x040024F8 RID: 9464
		Incompletable,
		// Token: 0x040024F9 RID: 9465
		InterruptOptional,
		// Token: 0x040024FA RID: 9466
		InterruptForced,
		// Token: 0x040024FB RID: 9467
		QueuedNoLongerValid,
		// Token: 0x040024FC RID: 9468
		Errored,
		// Token: 0x040024FD RID: 9469
		ErroredPather
	}
}
