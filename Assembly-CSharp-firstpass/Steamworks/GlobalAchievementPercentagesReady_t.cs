using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D5 RID: 213
	[CallbackIdentity(1110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalAchievementPercentagesReady_t
	{
		// Token: 0x0400028F RID: 655
		public const int k_iCallback = 1110;

		// Token: 0x04000290 RID: 656
		public ulong m_nGameID;

		// Token: 0x04000291 RID: 657
		public EResult m_eResult;
	}
}
