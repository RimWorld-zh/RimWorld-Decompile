using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E5 RID: 2533
	public enum PawnLostCondition : byte
	{
		// Token: 0x0400244A RID: 9290
		Undefined,
		// Token: 0x0400244B RID: 9291
		Vanished,
		// Token: 0x0400244C RID: 9292
		IncappedOrKilled,
		// Token: 0x0400244D RID: 9293
		MadePrisoner,
		// Token: 0x0400244E RID: 9294
		ChangedFaction,
		// Token: 0x0400244F RID: 9295
		ExitedMap,
		// Token: 0x04002450 RID: 9296
		LeftVoluntarily,
		// Token: 0x04002451 RID: 9297
		Drafted,
		// Token: 0x04002452 RID: 9298
		ForcedToJoinOtherLord,
		// Token: 0x04002453 RID: 9299
		ForcedByPlayerAction
	}
}
