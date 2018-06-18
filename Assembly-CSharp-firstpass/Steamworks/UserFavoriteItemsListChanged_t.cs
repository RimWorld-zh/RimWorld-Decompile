using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BC RID: 188
	[CallbackIdentity(3407)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserFavoriteItemsListChanged_t
	{
		// Token: 0x04000238 RID: 568
		public const int k_iCallback = 3407;

		// Token: 0x04000239 RID: 569
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400023A RID: 570
		public EResult m_eResult;

		// Token: 0x0400023B RID: 571
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bWasAddRequest;
	}
}
