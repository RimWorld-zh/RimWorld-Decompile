using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D7 RID: 215
	[CallbackIdentity(1112)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalStatsReceived_t
	{
		// Token: 0x04000295 RID: 661
		public const int k_iCallback = 1112;

		// Token: 0x04000296 RID: 662
		public ulong m_nGameID;

		// Token: 0x04000297 RID: 663
		public EResult m_eResult;
	}
}
