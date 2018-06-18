using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B0 RID: 176
	[CallbackIdentity(1327)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSetUserPublishedFileActionResult_t
	{
		// Token: 0x0400020B RID: 523
		public const int k_iCallback = 1327;

		// Token: 0x0400020C RID: 524
		public EResult m_eResult;

		// Token: 0x0400020D RID: 525
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400020E RID: 526
		public EWorkshopFileAction m_eAction;
	}
}
