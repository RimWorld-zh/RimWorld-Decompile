using System;

namespace Steamworks
{
	// Token: 0x02000106 RID: 262
	[Flags]
	public enum EItemState
	{
		// Token: 0x04000477 RID: 1143
		k_EItemStateNone = 0,
		// Token: 0x04000478 RID: 1144
		k_EItemStateSubscribed = 1,
		// Token: 0x04000479 RID: 1145
		k_EItemStateLegacyItem = 2,
		// Token: 0x0400047A RID: 1146
		k_EItemStateInstalled = 4,
		// Token: 0x0400047B RID: 1147
		k_EItemStateNeedsUpdate = 8,
		// Token: 0x0400047C RID: 1148
		k_EItemStateDownloading = 16,
		// Token: 0x0400047D RID: 1149
		k_EItemStateDownloadPending = 32
	}
}
