using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000080 RID: 128
	[CallbackIdentity(507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatMsg_t
	{
		// Token: 0x04000160 RID: 352
		public const int k_iCallback = 507;

		// Token: 0x04000161 RID: 353
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000162 RID: 354
		public ulong m_ulSteamIDUser;

		// Token: 0x04000163 RID: 355
		public byte m_eChatEntryType;

		// Token: 0x04000164 RID: 356
		public uint m_iChatID;
	}
}
