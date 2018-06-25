using System;

namespace Verse.AI
{
	// Token: 0x02000ADF RID: 2783
	[Flags]
	public enum TargetScanFlags : byte
	{
		// Token: 0x040026D6 RID: 9942
		None = 0,
		// Token: 0x040026D7 RID: 9943
		NeedLOSToPawns = 1,
		// Token: 0x040026D8 RID: 9944
		NeedLOSToNonPawns = 2,
		// Token: 0x040026D9 RID: 9945
		NeedLOSToAll = 3,
		// Token: 0x040026DA RID: 9946
		NeedReachable = 4,
		// Token: 0x040026DB RID: 9947
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x040026DC RID: 9948
		NeedNonBurning = 16,
		// Token: 0x040026DD RID: 9949
		NeedThreat = 32,
		// Token: 0x040026DE RID: 9950
		LOSBlockableByGas = 64
	}
}
