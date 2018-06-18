using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A5 RID: 165
	[CallbackIdentity(1316)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdatePublishedFileResult_t
	{
		// Token: 0x040001C6 RID: 454
		public const int k_iCallback = 1316;

		// Token: 0x040001C7 RID: 455
		public EResult m_eResult;

		// Token: 0x040001C8 RID: 456
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001C9 RID: 457
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
