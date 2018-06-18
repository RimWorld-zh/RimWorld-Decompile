using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CD RID: 205
	[CallbackIdentity(1102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsStored_t
	{
		// Token: 0x0400026E RID: 622
		public const int k_iCallback = 1102;

		// Token: 0x0400026F RID: 623
		public ulong m_nGameID;

		// Token: 0x04000270 RID: 624
		public EResult m_eResult;
	}
}
