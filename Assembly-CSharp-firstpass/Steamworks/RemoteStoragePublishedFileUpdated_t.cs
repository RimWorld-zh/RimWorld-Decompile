using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B3 RID: 179
	[CallbackIdentity(1330)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUpdated_t
	{
		// Token: 0x04000219 RID: 537
		public const int k_iCallback = 1330;

		// Token: 0x0400021A RID: 538
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400021B RID: 539
		public AppId_t m_nAppID;

		// Token: 0x0400021C RID: 540
		public UGCHandle_t m_hFile;
	}
}
