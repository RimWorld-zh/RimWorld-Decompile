using System;

namespace Verse.AI
{
	// Token: 0x02000AE0 RID: 2784
	[Flags]
	public enum TargetScanFlags : byte
	{
		// Token: 0x040026D3 RID: 9939
		None = 0,
		// Token: 0x040026D4 RID: 9940
		NeedLOSToPawns = 1,
		// Token: 0x040026D5 RID: 9941
		NeedLOSToNonPawns = 2,
		// Token: 0x040026D6 RID: 9942
		NeedLOSToAll = 3,
		// Token: 0x040026D7 RID: 9943
		NeedReachable = 4,
		// Token: 0x040026D8 RID: 9944
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x040026D9 RID: 9945
		NeedNonBurning = 16,
		// Token: 0x040026DA RID: 9946
		NeedThreat = 32,
		// Token: 0x040026DB RID: 9947
		LOSBlockableByGas = 64
	}
}
