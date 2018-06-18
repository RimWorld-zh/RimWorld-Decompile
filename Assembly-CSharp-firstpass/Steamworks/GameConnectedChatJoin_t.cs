using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000047 RID: 71
	[CallbackIdentity(339)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameConnectedChatJoin_t
	{
		// Token: 0x04000077 RID: 119
		public const int k_iCallback = 339;

		// Token: 0x04000078 RID: 120
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000079 RID: 121
		public CSteamID m_steamIDUser;
	}
}
