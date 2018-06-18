using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009A RID: 154
	[CallbackIdentity(1302)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedServer_t
	{
		// Token: 0x0400019A RID: 410
		public const int k_iCallback = 1302;

		// Token: 0x0400019B RID: 411
		public AppId_t m_nAppID;

		// Token: 0x0400019C RID: 412
		public EResult m_eResult;

		// Token: 0x0400019D RID: 413
		public int m_unNumUploads;
	}
}
