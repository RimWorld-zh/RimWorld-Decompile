using System;

namespace Verse.AI
{
	// Token: 0x02000ADE RID: 2782
	[Flags]
	public enum TargetScanFlags : byte
	{
		// Token: 0x040026CF RID: 9935
		None = 0,
		// Token: 0x040026D0 RID: 9936
		NeedLOSToPawns = 1,
		// Token: 0x040026D1 RID: 9937
		NeedLOSToNonPawns = 2,
		// Token: 0x040026D2 RID: 9938
		NeedLOSToAll = 3,
		// Token: 0x040026D3 RID: 9939
		NeedReachable = 4,
		// Token: 0x040026D4 RID: 9940
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x040026D5 RID: 9941
		NeedNonBurning = 16,
		// Token: 0x040026D6 RID: 9942
		NeedThreat = 32,
		// Token: 0x040026D7 RID: 9943
		LOSBlockableByGas = 64
	}
}
