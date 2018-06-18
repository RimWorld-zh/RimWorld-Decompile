using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007F RID: 127
	[CallbackIdentity(506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatUpdate_t
	{
		// Token: 0x0400015B RID: 347
		public const int k_iCallback = 506;

		// Token: 0x0400015C RID: 348
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400015D RID: 349
		public ulong m_ulSteamIDUserChanged;

		// Token: 0x0400015E RID: 350
		public ulong m_ulSteamIDMakingChange;

		// Token: 0x0400015F RID: 351
		public uint m_rgfChatMemberStateChange;
	}
}
