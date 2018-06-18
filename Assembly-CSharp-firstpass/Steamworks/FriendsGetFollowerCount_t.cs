using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004C RID: 76
	[CallbackIdentity(344)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsGetFollowerCount_t
	{
		// Token: 0x04000087 RID: 135
		public const int k_iCallback = 344;

		// Token: 0x04000088 RID: 136
		public EResult m_eResult;

		// Token: 0x04000089 RID: 137
		public CSteamID m_steamID;

		// Token: 0x0400008A RID: 138
		public int m_nCount;
	}
}
