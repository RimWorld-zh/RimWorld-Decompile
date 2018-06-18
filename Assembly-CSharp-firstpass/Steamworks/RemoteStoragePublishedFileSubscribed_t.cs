using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AA RID: 170
	[CallbackIdentity(1321)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileSubscribed_t
	{
		// Token: 0x040001F6 RID: 502
		public const int k_iCallback = 1321;

		// Token: 0x040001F7 RID: 503
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001F8 RID: 504
		public AppId_t m_nAppID;
	}
}
