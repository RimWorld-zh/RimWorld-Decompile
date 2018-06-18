using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BE RID: 190
	[CallbackIdentity(3409)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetUserItemVoteResult_t
	{
		// Token: 0x04000240 RID: 576
		public const int k_iCallback = 3409;

		// Token: 0x04000241 RID: 577
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000242 RID: 578
		public EResult m_eResult;

		// Token: 0x04000243 RID: 579
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedUp;

		// Token: 0x04000244 RID: 580
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedDown;

		// Token: 0x04000245 RID: 581
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteSkipped;
	}
}
