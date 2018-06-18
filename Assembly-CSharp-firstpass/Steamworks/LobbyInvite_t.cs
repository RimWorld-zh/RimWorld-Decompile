using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007C RID: 124
	[CallbackIdentity(503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyInvite_t
	{
		// Token: 0x0400014E RID: 334
		public const int k_iCallback = 503;

		// Token: 0x0400014F RID: 335
		public ulong m_ulSteamIDUser;

		// Token: 0x04000150 RID: 336
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000151 RID: 337
		public ulong m_ulGameID;
	}
}
