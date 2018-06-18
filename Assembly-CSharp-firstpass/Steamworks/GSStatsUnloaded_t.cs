using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005E RID: 94
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSStatsUnloaded_t
	{
		// Token: 0x040000CB RID: 203
		public const int k_iCallback = 1108;

		// Token: 0x040000CC RID: 204
		public CSteamID m_steamIDUser;
	}
}
