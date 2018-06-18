using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D1 RID: 209
	[CallbackIdentity(1106)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoreUploaded_t
	{
		// Token: 0x0400027E RID: 638
		public const int k_iCallback = 1106;

		// Token: 0x0400027F RID: 639
		public byte m_bSuccess;

		// Token: 0x04000280 RID: 640
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x04000281 RID: 641
		public int m_nScore;

		// Token: 0x04000282 RID: 642
		public byte m_bScoreChanged;

		// Token: 0x04000283 RID: 643
		public int m_nGlobalRankNew;

		// Token: 0x04000284 RID: 644
		public int m_nGlobalRankPrevious;
	}
}
