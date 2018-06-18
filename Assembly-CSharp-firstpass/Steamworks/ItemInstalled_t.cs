using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BA RID: 186
	[CallbackIdentity(3405)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ItemInstalled_t
	{
		// Token: 0x04000231 RID: 561
		public const int k_iCallback = 3405;

		// Token: 0x04000232 RID: 562
		public AppId_t m_unAppID;

		// Token: 0x04000233 RID: 563
		public PublishedFileId_t m_nPublishedFileId;
	}
}
