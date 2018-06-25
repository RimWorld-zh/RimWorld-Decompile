using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A0F RID: 2575
	public enum TriggerSignalType : byte
	{
		// Token: 0x040024A4 RID: 9380
		Undefined,
		// Token: 0x040024A5 RID: 9381
		Tick,
		// Token: 0x040024A6 RID: 9382
		Memo,
		// Token: 0x040024A7 RID: 9383
		PawnDamaged,
		// Token: 0x040024A8 RID: 9384
		PawnArrestAttempted,
		// Token: 0x040024A9 RID: 9385
		PawnLost,
		// Token: 0x040024AA RID: 9386
		BuildingDamaged,
		// Token: 0x040024AB RID: 9387
		FactionRelationsChanged
	}
}
