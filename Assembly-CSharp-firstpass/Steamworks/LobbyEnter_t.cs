using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007D RID: 125
	[CallbackIdentity(504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyEnter_t
	{
		// Token: 0x04000152 RID: 338
		public const int k_iCallback = 504;

		// Token: 0x04000153 RID: 339
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000154 RID: 340
		public uint m_rgfChatPermissions;

		// Token: 0x04000155 RID: 341
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocked;

		// Token: 0x04000156 RID: 342
		public uint m_EChatRoomEnterResponse;
	}
}
