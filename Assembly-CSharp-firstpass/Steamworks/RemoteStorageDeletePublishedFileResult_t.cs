using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A0 RID: 160
	[CallbackIdentity(1311)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDeletePublishedFileResult_t
	{
		// Token: 0x040001B2 RID: 434
		public const int k_iCallback = 1311;

		// Token: 0x040001B3 RID: 435
		public EResult m_eResult;

		// Token: 0x040001B4 RID: 436
		public PublishedFileId_t m_nPublishedFileId;
	}
}
