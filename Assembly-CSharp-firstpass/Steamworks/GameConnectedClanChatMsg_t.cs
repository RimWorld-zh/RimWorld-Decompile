using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000046 RID: 70
	[CallbackIdentity(338)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedClanChatMsg_t
	{
		// Token: 0x04000073 RID: 115
		public const int k_iCallback = 338;

		// Token: 0x04000074 RID: 116
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000075 RID: 117
		public CSteamID m_steamIDUser;

		// Token: 0x04000076 RID: 118
		public int m_iMessageID;
	}
}
