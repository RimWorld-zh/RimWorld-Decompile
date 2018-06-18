using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000054 RID: 84
	[CallbackIdentity(203)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientKick_t
	{
		// Token: 0x040000A2 RID: 162
		public const int k_iCallback = 203;

		// Token: 0x040000A3 RID: 163
		public CSteamID m_SteamID;

		// Token: 0x040000A4 RID: 164
		public EDenyReason m_eDenyReason;
	}
}
