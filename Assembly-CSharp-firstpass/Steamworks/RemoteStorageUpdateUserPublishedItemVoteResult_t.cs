using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AD RID: 173
	[CallbackIdentity(1324)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdateUserPublishedItemVoteResult_t
	{
		// Token: 0x040001FF RID: 511
		public const int k_iCallback = 1324;

		// Token: 0x04000200 RID: 512
		public EResult m_eResult;

		// Token: 0x04000201 RID: 513
		public PublishedFileId_t m_nPublishedFileId;
	}
}
