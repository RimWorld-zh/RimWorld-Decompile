using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004D RID: 77
	[CallbackIdentity(345)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsIsFollowing_t
	{
		// Token: 0x0400008B RID: 139
		public const int k_iCallback = 345;

		// Token: 0x0400008C RID: 140
		public EResult m_eResult;

		// Token: 0x0400008D RID: 141
		public CSteamID m_steamID;

		// Token: 0x0400008E RID: 142
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsFollowing;
	}
}
