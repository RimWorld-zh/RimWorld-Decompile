using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B6 RID: 182
	[CallbackIdentity(3401)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCQueryCompleted_t
	{
		// Token: 0x04000221 RID: 545
		public const int k_iCallback = 3401;

		// Token: 0x04000222 RID: 546
		public UGCQueryHandle_t m_handle;

		// Token: 0x04000223 RID: 547
		public EResult m_eResult;

		// Token: 0x04000224 RID: 548
		public uint m_unNumResultsReturned;

		// Token: 0x04000225 RID: 549
		public uint m_unTotalMatchingResults;

		// Token: 0x04000226 RID: 550
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
