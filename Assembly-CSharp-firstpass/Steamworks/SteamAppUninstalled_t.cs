using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000039 RID: 57
	[CallbackIdentity(3902)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppUninstalled_t
	{
		// Token: 0x0400004D RID: 77
		public const int k_iCallback = 3902;

		// Token: 0x0400004E RID: 78
		public AppId_t m_nAppID;
	}
}
