using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009C RID: 156
	[CallbackIdentity(1305)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncStatusCheck_t
	{
		// Token: 0x040001A4 RID: 420
		public const int k_iCallback = 1305;

		// Token: 0x040001A5 RID: 421
		public AppId_t m_nAppID;

		// Token: 0x040001A6 RID: 422
		public EResult m_eResult;
	}
}
