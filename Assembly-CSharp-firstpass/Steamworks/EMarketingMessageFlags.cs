using System;

namespace Steamworks
{
	// Token: 0x02000121 RID: 289
	[Flags]
	public enum EMarketingMessageFlags
	{
		// Token: 0x040005AB RID: 1451
		k_EMarketingMessageFlagsNone = 0,
		// Token: 0x040005AC RID: 1452
		k_EMarketingMessageFlagsHighPriority = 1,
		// Token: 0x040005AD RID: 1453
		k_EMarketingMessageFlagsPlatformWindows = 2,
		// Token: 0x040005AE RID: 1454
		k_EMarketingMessageFlagsPlatformMac = 4,
		// Token: 0x040005AF RID: 1455
		k_EMarketingMessageFlagsPlatformLinux = 8,
		// Token: 0x040005B0 RID: 1456
		k_EMarketingMessageFlagsPlatformRestrictions = 14
	}
}
