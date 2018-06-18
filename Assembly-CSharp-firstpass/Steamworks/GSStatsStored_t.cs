using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005D RID: 93
	[CallbackIdentity(1801)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsStored_t
	{
		// Token: 0x040000C8 RID: 200
		public const int k_iCallback = 1801;

		// Token: 0x040000C9 RID: 201
		public EResult m_eResult;

		// Token: 0x040000CA RID: 202
		public CSteamID m_steamIDUser;
	}
}
