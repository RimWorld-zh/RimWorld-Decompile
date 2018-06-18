using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A4 RID: 164
	[CallbackIdentity(1315)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUnsubscribePublishedFileResult_t
	{
		// Token: 0x040001C3 RID: 451
		public const int k_iCallback = 1315;

		// Token: 0x040001C4 RID: 452
		public EResult m_eResult;

		// Token: 0x040001C5 RID: 453
		public PublishedFileId_t m_nPublishedFileId;
	}
}
