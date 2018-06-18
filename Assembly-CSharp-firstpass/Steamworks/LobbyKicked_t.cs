using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000083 RID: 131
	[CallbackIdentity(512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyKicked_t
	{
		// Token: 0x0400016C RID: 364
		public const int k_iCallback = 512;

		// Token: 0x0400016D RID: 365
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400016E RID: 366
		public ulong m_ulSteamIDAdmin;

		// Token: 0x0400016F RID: 367
		public byte m_bKickedDueToDisconnect;
	}
}
