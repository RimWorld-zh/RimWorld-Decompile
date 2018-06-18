using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000081 RID: 129
	[CallbackIdentity(509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyGameCreated_t
	{
		// Token: 0x04000165 RID: 357
		public const int k_iCallback = 509;

		// Token: 0x04000166 RID: 358
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000167 RID: 359
		public ulong m_ulSteamIDGameServer;

		// Token: 0x04000168 RID: 360
		public uint m_unIP;

		// Token: 0x04000169 RID: 361
		public ushort m_usPort;
	}
}
