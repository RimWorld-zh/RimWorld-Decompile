using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012F RID: 303
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardEntry_t
	{
		// Token: 0x04000639 RID: 1593
		public CSteamID m_steamIDUser;

		// Token: 0x0400063A RID: 1594
		public int m_nGlobalRank;

		// Token: 0x0400063B RID: 1595
		public int m_nScore;

		// Token: 0x0400063C RID: 1596
		public int m_cDetails;

		// Token: 0x0400063D RID: 1597
		public UGCHandle_t m_hUGC;
	}
}
