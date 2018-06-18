using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000048 RID: 72
	[CallbackIdentity(340)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GameConnectedChatLeave_t
	{
		// Token: 0x0400007A RID: 122
		public const int k_iCallback = 340;

		// Token: 0x0400007B RID: 123
		public CSteamID m_steamIDClanChat;

		// Token: 0x0400007C RID: 124
		public CSteamID m_steamIDUser;

		// Token: 0x0400007D RID: 125
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bKicked;

		// Token: 0x0400007E RID: 126
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDropped;
	}
}
