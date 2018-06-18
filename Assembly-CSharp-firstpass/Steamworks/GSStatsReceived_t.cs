using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005C RID: 92
	[CallbackIdentity(1800)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsReceived_t
	{
		// Token: 0x040000C5 RID: 197
		public const int k_iCallback = 1800;

		// Token: 0x040000C6 RID: 198
		public EResult m_eResult;

		// Token: 0x040000C7 RID: 199
		public CSteamID m_steamIDUser;
	}
}
