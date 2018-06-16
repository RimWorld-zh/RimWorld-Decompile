using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A10 RID: 2576
	public enum TriggerSignalType : byte
	{
		// Token: 0x04002498 RID: 9368
		Undefined,
		// Token: 0x04002499 RID: 9369
		Tick,
		// Token: 0x0400249A RID: 9370
		Memo,
		// Token: 0x0400249B RID: 9371
		PawnDamaged,
		// Token: 0x0400249C RID: 9372
		PawnArrestAttempted,
		// Token: 0x0400249D RID: 9373
		PawnLost,
		// Token: 0x0400249E RID: 9374
		BuildingDamaged,
		// Token: 0x0400249F RID: 9375
		FactionRelationsChanged
	}
}
