using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000038 RID: 56
	[CallbackIdentity(3901)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppInstalled_t
	{
		// Token: 0x0400004B RID: 75
		public const int k_iCallback = 3901;

		// Token: 0x0400004C RID: 76
		public AppId_t m_nAppID;
	}
}
