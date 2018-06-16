using System;

namespace Verse
{
	// Token: 0x02000C98 RID: 3224
	[Flags]
	public enum RegionType
	{
		// Token: 0x04003018 RID: 12312
		None = 0,
		// Token: 0x04003019 RID: 12313
		ImpassableFreeAirExchange = 1,
		// Token: 0x0400301A RID: 12314
		Normal = 2,
		// Token: 0x0400301B RID: 12315
		Portal = 4,
		// Token: 0x0400301C RID: 12316
		Set_Passable = 6,
		// Token: 0x0400301D RID: 12317
		Set_Impassable = 1,
		// Token: 0x0400301E RID: 12318
		Set_All = 7
	}
}
