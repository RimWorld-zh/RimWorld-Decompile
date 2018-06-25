using System;

namespace Verse
{
	// Token: 0x02000C96 RID: 3222
	[Flags]
	public enum RegionType
	{
		// Token: 0x04003021 RID: 12321
		None = 0,
		// Token: 0x04003022 RID: 12322
		ImpassableFreeAirExchange = 1,
		// Token: 0x04003023 RID: 12323
		Normal = 2,
		// Token: 0x04003024 RID: 12324
		Portal = 4,
		// Token: 0x04003025 RID: 12325
		Set_Passable = 6,
		// Token: 0x04003026 RID: 12326
		Set_Impassable = 1,
		// Token: 0x04003027 RID: 12327
		Set_All = 7
	}
}
