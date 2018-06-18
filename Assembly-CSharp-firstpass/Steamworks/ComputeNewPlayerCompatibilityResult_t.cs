using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005B RID: 91
	[CallbackIdentity(211)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ComputeNewPlayerCompatibilityResult_t
	{
		// Token: 0x040000BF RID: 191
		public const int k_iCallback = 211;

		// Token: 0x040000C0 RID: 192
		public EResult m_eResult;

		// Token: 0x040000C1 RID: 193
		public int m_cPlayersThatDontLikeCandidate;

		// Token: 0x040000C2 RID: 194
		public int m_cPlayersThatCandidateDoesntLike;

		// Token: 0x040000C3 RID: 195
		public int m_cClanPlayersThatDontLikeCandidate;

		// Token: 0x040000C4 RID: 196
		public CSteamID m_SteamIDCandidate;
	}
}
