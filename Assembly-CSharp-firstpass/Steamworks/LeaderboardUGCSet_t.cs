using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D6 RID: 214
	[CallbackIdentity(1111)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardUGCSet_t
	{
		// Token: 0x04000292 RID: 658
		public const int k_iCallback = 1111;

		// Token: 0x04000293 RID: 659
		public EResult m_eResult;

		// Token: 0x04000294 RID: 660
		public SteamLeaderboard_t m_hSteamLeaderboard;
	}
}
