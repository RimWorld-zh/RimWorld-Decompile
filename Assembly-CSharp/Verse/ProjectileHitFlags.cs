using System;

namespace Verse
{
	// Token: 0x02000DF2 RID: 3570
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x040034F1 RID: 13553
		None = 0,
		// Token: 0x040034F2 RID: 13554
		IntendedTarget = 1,
		// Token: 0x040034F3 RID: 13555
		NonTargetPawns = 2,
		// Token: 0x040034F4 RID: 13556
		NonTargetWorld = 4,
		// Token: 0x040034F5 RID: 13557
		All = -1
	}
}
