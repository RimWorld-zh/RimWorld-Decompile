using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000049 RID: 73
	[CallbackIdentity(341)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadClanActivityCountsResult_t
	{
		// Token: 0x0400007F RID: 127
		public const int k_iCallback = 341;

		// Token: 0x04000080 RID: 128
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;
	}
}
