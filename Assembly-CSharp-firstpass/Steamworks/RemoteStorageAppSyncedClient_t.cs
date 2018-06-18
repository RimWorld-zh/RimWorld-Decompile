using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000099 RID: 153
	[CallbackIdentity(1301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedClient_t
	{
		// Token: 0x04000196 RID: 406
		public const int k_iCallback = 1301;

		// Token: 0x04000197 RID: 407
		public AppId_t m_nAppID;

		// Token: 0x04000198 RID: 408
		public EResult m_eResult;

		// Token: 0x04000199 RID: 409
		public int m_unNumDownloads;
	}
}
