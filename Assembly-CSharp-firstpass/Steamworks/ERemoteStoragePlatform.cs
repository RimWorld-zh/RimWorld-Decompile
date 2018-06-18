using System;

namespace Steamworks
{
	// Token: 0x020000F9 RID: 249
	[Flags]
	public enum ERemoteStoragePlatform
	{
		// Token: 0x0400040C RID: 1036
		k_ERemoteStoragePlatformNone = 0,
		// Token: 0x0400040D RID: 1037
		k_ERemoteStoragePlatformWindows = 1,
		// Token: 0x0400040E RID: 1038
		k_ERemoteStoragePlatformOSX = 2,
		// Token: 0x0400040F RID: 1039
		k_ERemoteStoragePlatformPS3 = 4,
		// Token: 0x04000410 RID: 1040
		k_ERemoteStoragePlatformLinux = 8,
		// Token: 0x04000411 RID: 1041
		k_ERemoteStoragePlatformReserved2 = 16,
		// Token: 0x04000412 RID: 1042
		k_ERemoteStoragePlatformAll = -1
	}
}
