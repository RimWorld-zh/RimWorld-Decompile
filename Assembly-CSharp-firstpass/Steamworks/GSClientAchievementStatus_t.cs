using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000055 RID: 85
	[CallbackIdentity(206)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientAchievementStatus_t
	{
		// Token: 0x040000A5 RID: 165
		public const int k_iCallback = 206;

		// Token: 0x040000A6 RID: 166
		public ulong m_SteamID;

		// Token: 0x040000A7 RID: 167
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_pchAchievement;

		// Token: 0x040000A8 RID: 168
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUnlocked;
	}
}
