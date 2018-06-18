using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007E RID: 126
	[CallbackIdentity(505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyDataUpdate_t
	{
		// Token: 0x04000157 RID: 343
		public const int k_iCallback = 505;

		// Token: 0x04000158 RID: 344
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000159 RID: 345
		public ulong m_ulSteamIDMember;

		// Token: 0x0400015A RID: 346
		public byte m_bSuccess;
	}
}
