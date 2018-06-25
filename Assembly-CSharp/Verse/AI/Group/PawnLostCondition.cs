using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E4 RID: 2532
	public enum PawnLostCondition : byte
	{
		// Token: 0x0400243A RID: 9274
		Undefined,
		// Token: 0x0400243B RID: 9275
		Vanished,
		// Token: 0x0400243C RID: 9276
		IncappedOrKilled,
		// Token: 0x0400243D RID: 9277
		MadePrisoner,
		// Token: 0x0400243E RID: 9278
		ChangedFaction,
		// Token: 0x0400243F RID: 9279
		ExitedMap,
		// Token: 0x04002440 RID: 9280
		LeftVoluntarily,
		// Token: 0x04002441 RID: 9281
		Drafted,
		// Token: 0x04002442 RID: 9282
		ForcedToJoinOtherLord,
		// Token: 0x04002443 RID: 9283
		ForcedByPlayerAction
	}
}
