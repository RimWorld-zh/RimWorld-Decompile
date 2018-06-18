using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AE RID: 174
	[CallbackIdentity(1325)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUserVoteDetails_t
	{
		// Token: 0x04000202 RID: 514
		public const int k_iCallback = 1325;

		// Token: 0x04000203 RID: 515
		public EResult m_eResult;

		// Token: 0x04000204 RID: 516
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000205 RID: 517
		public EWorkshopVote m_eVote;
	}
}
