using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E2 RID: 2530
	public enum PawnLostCondition : byte
	{
		// Token: 0x04002439 RID: 9273
		Undefined,
		// Token: 0x0400243A RID: 9274
		Vanished,
		// Token: 0x0400243B RID: 9275
		IncappedOrKilled,
		// Token: 0x0400243C RID: 9276
		MadePrisoner,
		// Token: 0x0400243D RID: 9277
		ChangedFaction,
		// Token: 0x0400243E RID: 9278
		ExitedMap,
		// Token: 0x0400243F RID: 9279
		LeftVoluntarily,
		// Token: 0x04002440 RID: 9280
		Drafted,
		// Token: 0x04002441 RID: 9281
		ForcedToJoinOtherLord,
		// Token: 0x04002442 RID: 9282
		ForcedByPlayerAction
	}
}
