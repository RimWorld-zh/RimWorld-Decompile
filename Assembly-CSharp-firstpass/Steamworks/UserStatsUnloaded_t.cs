using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D3 RID: 211
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsUnloaded_t
	{
		// Token: 0x04000288 RID: 648
		public const int k_iCallback = 1108;

		// Token: 0x04000289 RID: 649
		public CSteamID m_steamIDUser;
	}
}
