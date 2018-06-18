using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A9 RID: 169
	[CallbackIdentity(1320)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
	{
		// Token: 0x040001EF RID: 495
		public const int k_iCallback = 1320;

		// Token: 0x040001F0 RID: 496
		public EResult m_eResult;

		// Token: 0x040001F1 RID: 497
		public PublishedFileId_t m_unPublishedFileId;

		// Token: 0x040001F2 RID: 498
		public int m_nVotesFor;

		// Token: 0x040001F3 RID: 499
		public int m_nVotesAgainst;

		// Token: 0x040001F4 RID: 500
		public int m_nReports;

		// Token: 0x040001F5 RID: 501
		public float m_fScore;
	}
}
