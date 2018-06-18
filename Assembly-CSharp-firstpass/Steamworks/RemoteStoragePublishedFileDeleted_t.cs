using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AC RID: 172
	[CallbackIdentity(1323)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileDeleted_t
	{
		// Token: 0x040001FC RID: 508
		public const int k_iCallback = 1323;

		// Token: 0x040001FD RID: 509
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001FE RID: 510
		public AppId_t m_nAppID;
	}
}
