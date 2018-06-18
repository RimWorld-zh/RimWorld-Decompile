using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BB RID: 187
	[CallbackIdentity(3406)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadItemResult_t
	{
		// Token: 0x04000234 RID: 564
		public const int k_iCallback = 3406;

		// Token: 0x04000235 RID: 565
		public AppId_t m_unAppID;

		// Token: 0x04000236 RID: 566
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000237 RID: 567
		public EResult m_eResult;
	}
}
