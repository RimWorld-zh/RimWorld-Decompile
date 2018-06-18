using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000128 RID: 296
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FriendGameInfo_t
	{
		// Token: 0x04000606 RID: 1542
		public CGameID m_gameID;

		// Token: 0x04000607 RID: 1543
		public uint m_unGameIP;

		// Token: 0x04000608 RID: 1544
		public ushort m_usGamePort;

		// Token: 0x04000609 RID: 1545
		public ushort m_usQueryPort;

		// Token: 0x0400060A RID: 1546
		public CSteamID m_steamIDLobby;
	}
}
