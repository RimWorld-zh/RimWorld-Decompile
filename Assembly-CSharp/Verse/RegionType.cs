using System;

namespace Verse
{
	// Token: 0x02000C97 RID: 3223
	[Flags]
	public enum RegionType
	{
		// Token: 0x04003028 RID: 12328
		None = 0,
		// Token: 0x04003029 RID: 12329
		ImpassableFreeAirExchange = 1,
		// Token: 0x0400302A RID: 12330
		Normal = 2,
		// Token: 0x0400302B RID: 12331
		Portal = 4,
		// Token: 0x0400302C RID: 12332
		Set_Passable = 6,
		// Token: 0x0400302D RID: 12333
		Set_Impassable = 1,
		// Token: 0x0400302E RID: 12334
		Set_All = 7
	}
}
