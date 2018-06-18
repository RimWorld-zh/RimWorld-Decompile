using System;

namespace Verse
{
	// Token: 0x02000C97 RID: 3223
	[Flags]
	public enum RegionType
	{
		// Token: 0x04003016 RID: 12310
		None = 0,
		// Token: 0x04003017 RID: 12311
		ImpassableFreeAirExchange = 1,
		// Token: 0x04003018 RID: 12312
		Normal = 2,
		// Token: 0x04003019 RID: 12313
		Portal = 4,
		// Token: 0x0400301A RID: 12314
		Set_Passable = 6,
		// Token: 0x0400301B RID: 12315
		Set_Impassable = 1,
		// Token: 0x0400301C RID: 12316
		Set_All = 7
	}
}
