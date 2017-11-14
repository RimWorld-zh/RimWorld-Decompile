using System;

namespace Verse.AI
{
	[Flags]
	public enum TargetScanFlags : byte
	{
		None = 0,
		NeedLOSToPawns = 1,
		NeedLOSToNonPawns = 2,
		NeedLOSToAll = 3,
		NeedReachable = 4,
		NeedReachableIfCantHitFromMyPos = 8,
		NeedNonBurning = 0x10,
		NeedThreat = 0x20,
		LOSBlockableByGas = 0x40
	}
}
