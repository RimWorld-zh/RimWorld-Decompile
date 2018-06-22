using System;

namespace Verse.AI
{
	// Token: 0x02000ADC RID: 2780
	[Flags]
	public enum TargetScanFlags : byte
	{
		// Token: 0x040026CE RID: 9934
		None = 0,
		// Token: 0x040026CF RID: 9935
		NeedLOSToPawns = 1,
		// Token: 0x040026D0 RID: 9936
		NeedLOSToNonPawns = 2,
		// Token: 0x040026D1 RID: 9937
		NeedLOSToAll = 3,
		// Token: 0x040026D2 RID: 9938
		NeedReachable = 4,
		// Token: 0x040026D3 RID: 9939
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x040026D4 RID: 9940
		NeedNonBurning = 16,
		// Token: 0x040026D5 RID: 9941
		NeedThreat = 32,
		// Token: 0x040026D6 RID: 9942
		LOSBlockableByGas = 64
	}
}
