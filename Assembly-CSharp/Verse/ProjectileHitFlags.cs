using System;

namespace Verse
{
	// Token: 0x02000DF0 RID: 3568
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x040034FA RID: 13562
		None = 0,
		// Token: 0x040034FB RID: 13563
		IntendedTarget = 1,
		// Token: 0x040034FC RID: 13564
		NonTargetPawns = 2,
		// Token: 0x040034FD RID: 13565
		NonTargetWorld = 4,
		// Token: 0x040034FE RID: 13566
		All = -1
	}
}
