using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D0 RID: 208
	[CallbackIdentity(1105)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoresDownloaded_t
	{
		// Token: 0x0400027A RID: 634
		public const int k_iCallback = 1105;

		// Token: 0x0400027B RID: 635
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x0400027C RID: 636
		public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;

		// Token: 0x0400027D RID: 637
		public int m_cEntryCount;
	}
}
