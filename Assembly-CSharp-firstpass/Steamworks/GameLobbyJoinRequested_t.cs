using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000041 RID: 65
	[CallbackIdentity(333)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameLobbyJoinRequested_t
	{
		// Token: 0x04000061 RID: 97
		public const int k_iCallback = 333;

		// Token: 0x04000062 RID: 98
		public CSteamID m_steamIDLobby;

		// Token: 0x04000063 RID: 99
		public CSteamID m_steamIDFriend;
	}
}
