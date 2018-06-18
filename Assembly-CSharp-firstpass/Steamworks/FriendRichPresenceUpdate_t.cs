using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000044 RID: 68
	[CallbackIdentity(336)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendRichPresenceUpdate_t
	{
		// Token: 0x0400006D RID: 109
		public const int k_iCallback = 336;

		// Token: 0x0400006E RID: 110
		public CSteamID m_steamIDFriend;

		// Token: 0x0400006F RID: 111
		public AppId_t m_nAppID;
	}
}
