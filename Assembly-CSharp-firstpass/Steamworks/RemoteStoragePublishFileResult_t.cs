using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009F RID: 159
	[CallbackIdentity(1309)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileResult_t
	{
		// Token: 0x040001AE RID: 430
		public const int k_iCallback = 1309;

		// Token: 0x040001AF RID: 431
		public EResult m_eResult;

		// Token: 0x040001B0 RID: 432
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001B1 RID: 433
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
