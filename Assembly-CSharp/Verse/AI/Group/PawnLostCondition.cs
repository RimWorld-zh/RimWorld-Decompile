using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E6 RID: 2534
	public enum PawnLostCondition : byte
	{
		// Token: 0x0400243E RID: 9278
		Undefined,
		// Token: 0x0400243F RID: 9279
		Vanished,
		// Token: 0x04002440 RID: 9280
		IncappedOrKilled,
		// Token: 0x04002441 RID: 9281
		MadePrisoner,
		// Token: 0x04002442 RID: 9282
		ChangedFaction,
		// Token: 0x04002443 RID: 9283
		ExitedMap,
		// Token: 0x04002444 RID: 9284
		LeftVoluntarily,
		// Token: 0x04002445 RID: 9285
		Drafted,
		// Token: 0x04002446 RID: 9286
		ForcedToJoinOtherLord,
		// Token: 0x04002447 RID: 9287
		ForcedByPlayerAction
	}
}
