using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CF RID: 207
	[CallbackIdentity(1104)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardFindResult_t
	{
		// Token: 0x04000277 RID: 631
		public const int k_iCallback = 1104;

		// Token: 0x04000278 RID: 632
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x04000279 RID: 633
		public byte m_bLeaderboardFound;
	}
}
