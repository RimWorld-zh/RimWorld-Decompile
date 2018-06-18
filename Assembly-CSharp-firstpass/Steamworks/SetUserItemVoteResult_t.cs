using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BD RID: 189
	[CallbackIdentity(3408)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetUserItemVoteResult_t
	{
		// Token: 0x0400023C RID: 572
		public const int k_iCallback = 3408;

		// Token: 0x0400023D RID: 573
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400023E RID: 574
		public EResult m_eResult;

		// Token: 0x0400023F RID: 575
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteUp;
	}
}
