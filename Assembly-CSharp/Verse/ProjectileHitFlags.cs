using System;

namespace Verse
{
	// Token: 0x02000DF1 RID: 3569
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x04003501 RID: 13569
		None = 0,
		// Token: 0x04003502 RID: 13570
		IntendedTarget = 1,
		// Token: 0x04003503 RID: 13571
		NonTargetPawns = 2,
		// Token: 0x04003504 RID: 13572
		NonTargetWorld = 4,
		// Token: 0x04003505 RID: 13573
		All = -1
	}
}
