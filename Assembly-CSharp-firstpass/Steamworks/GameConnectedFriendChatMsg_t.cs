using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004B RID: 75
	[CallbackIdentity(343)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedFriendChatMsg_t
	{
		// Token: 0x04000084 RID: 132
		public const int k_iCallback = 343;

		// Token: 0x04000085 RID: 133
		public CSteamID m_steamIDUser;

		// Token: 0x04000086 RID: 134
		public int m_iMessageID;
	}
}
