using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D4 RID: 212
	[CallbackIdentity(1109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementIconFetched_t
	{
		// Token: 0x0400028A RID: 650
		public const int k_iCallback = 1109;

		// Token: 0x0400028B RID: 651
		public CGameID m_nGameID;

		// Token: 0x0400028C RID: 652
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		// Token: 0x0400028D RID: 653
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAchieved;

		// Token: 0x0400028E RID: 654
		public int m_nIconHandle;
	}
}
