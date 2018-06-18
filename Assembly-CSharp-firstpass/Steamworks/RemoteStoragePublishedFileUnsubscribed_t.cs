using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AB RID: 171
	[CallbackIdentity(1322)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUnsubscribed_t
	{
		// Token: 0x040001F9 RID: 505
		public const int k_iCallback = 1322;

		// Token: 0x040001FA RID: 506
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001FB RID: 507
		public AppId_t m_nAppID;
	}
}
