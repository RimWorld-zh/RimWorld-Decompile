using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004A RID: 74
	[CallbackIdentity(342)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct JoinClanChatRoomCompletionResult_t
	{
		// Token: 0x04000081 RID: 129
		public const int k_iCallback = 342;

		// Token: 0x04000082 RID: 130
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000083 RID: 131
		public EChatRoomEnterResponse m_eChatRoomEnterResponse;
	}
}
