using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A6 RID: 166
	[CallbackIdentity(1317)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDownloadUGCResult_t
	{
		// Token: 0x040001CA RID: 458
		public const int k_iCallback = 1317;

		// Token: 0x040001CB RID: 459
		public EResult m_eResult;

		// Token: 0x040001CC RID: 460
		public UGCHandle_t m_hFile;

		// Token: 0x040001CD RID: 461
		public AppId_t m_nAppID;

		// Token: 0x040001CE RID: 462
		public int m_nSizeInBytes;

		// Token: 0x040001CF RID: 463
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x040001D0 RID: 464
		public ulong m_ulSteamIDOwner;
	}
}
