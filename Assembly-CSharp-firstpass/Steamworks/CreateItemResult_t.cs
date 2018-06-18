using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B8 RID: 184
	[CallbackIdentity(3403)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CreateItemResult_t
	{
		// Token: 0x0400022A RID: 554
		public const int k_iCallback = 3403;

		// Token: 0x0400022B RID: 555
		public EResult m_eResult;

		// Token: 0x0400022C RID: 556
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400022D RID: 557
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
