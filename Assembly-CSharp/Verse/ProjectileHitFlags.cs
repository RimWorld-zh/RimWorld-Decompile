using System;

namespace Verse
{
	// Token: 0x02000DF1 RID: 3569
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x040034EF RID: 13551
		None = 0,
		// Token: 0x040034F0 RID: 13552
		IntendedTarget = 1,
		// Token: 0x040034F1 RID: 13553
		NonTargetPawns = 2,
		// Token: 0x040034F2 RID: 13554
		NonTargetWorld = 4,
		// Token: 0x040034F3 RID: 13555
		All = -1
	}
}
