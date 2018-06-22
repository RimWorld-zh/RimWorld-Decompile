using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A0C RID: 2572
	public enum TriggerSignalType : byte
	{
		// Token: 0x04002493 RID: 9363
		Undefined,
		// Token: 0x04002494 RID: 9364
		Tick,
		// Token: 0x04002495 RID: 9365
		Memo,
		// Token: 0x04002496 RID: 9366
		PawnDamaged,
		// Token: 0x04002497 RID: 9367
		PawnArrestAttempted,
		// Token: 0x04002498 RID: 9368
		PawnLost,
		// Token: 0x04002499 RID: 9369
		BuildingDamaged,
		// Token: 0x0400249A RID: 9370
		FactionRelationsChanged
	}
}
