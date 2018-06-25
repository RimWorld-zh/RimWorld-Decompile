using System;

namespace Verse.AI
{
	// Token: 0x02000A32 RID: 2610
	public enum JobCondition : byte
	{
		// Token: 0x04002501 RID: 9473
		None,
		// Token: 0x04002502 RID: 9474
		Ongoing,
		// Token: 0x04002503 RID: 9475
		Succeeded,
		// Token: 0x04002504 RID: 9476
		Incompletable,
		// Token: 0x04002505 RID: 9477
		InterruptOptional,
		// Token: 0x04002506 RID: 9478
		InterruptForced,
		// Token: 0x04002507 RID: 9479
		QueuedNoLongerValid,
		// Token: 0x04002508 RID: 9480
		Errored,
		// Token: 0x04002509 RID: 9481
		ErroredPather
	}
}
