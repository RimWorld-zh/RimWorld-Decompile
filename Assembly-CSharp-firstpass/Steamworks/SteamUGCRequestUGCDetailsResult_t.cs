using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B7 RID: 183
	[CallbackIdentity(3402)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCRequestUGCDetailsResult_t
	{
		// Token: 0x04000227 RID: 551
		public const int k_iCallback = 3402;

		// Token: 0x04000228 RID: 552
		public SteamUGCDetails_t m_details;

		// Token: 0x04000229 RID: 553
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
