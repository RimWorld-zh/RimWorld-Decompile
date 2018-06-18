using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000045 RID: 69
	[CallbackIdentity(337)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameRichPresenceJoinRequested_t
	{
		// Token: 0x04000070 RID: 112
		public const int k_iCallback = 337;

		// Token: 0x04000071 RID: 113
		public CSteamID m_steamIDFriend;

		// Token: 0x04000072 RID: 114
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchConnect;
	}
}
