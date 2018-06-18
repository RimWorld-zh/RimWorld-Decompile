using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CE RID: 206
	[CallbackIdentity(1103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementStored_t
	{
		// Token: 0x04000271 RID: 625
		public const int k_iCallback = 1103;

		// Token: 0x04000272 RID: 626
		public ulong m_nGameID;

		// Token: 0x04000273 RID: 627
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bGroupAchievement;

		// Token: 0x04000274 RID: 628
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		// Token: 0x04000275 RID: 629
		public uint m_nCurProgress;

		// Token: 0x04000276 RID: 630
		public uint m_nMaxProgress;
	}
}
