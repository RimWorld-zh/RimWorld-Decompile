using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A2 RID: 162
	[CallbackIdentity(1313)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSubscribePublishedFileResult_t
	{
		// Token: 0x040001BA RID: 442
		public const int k_iCallback = 1313;

		// Token: 0x040001BB RID: 443
		public EResult m_eResult;

		// Token: 0x040001BC RID: 444
		public PublishedFileId_t m_nPublishedFileId;
	}
}
