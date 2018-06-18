using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000084 RID: 132
	[CallbackIdentity(513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyCreated_t
	{
		// Token: 0x04000170 RID: 368
		public const int k_iCallback = 513;

		// Token: 0x04000171 RID: 369
		public EResult m_eResult;

		// Token: 0x04000172 RID: 370
		public ulong m_ulSteamIDLobby;
	}
}
