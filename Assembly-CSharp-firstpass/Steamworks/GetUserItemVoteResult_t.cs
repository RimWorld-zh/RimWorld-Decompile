using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(3409)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetUserItemVoteResult_t
	{
		public const int k_iCallback = 3409;

		public PublishedFileId_t m_nPublishedFileId;

		public EResult m_eResult;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedUp;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedDown;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteSkipped;
	}
}
