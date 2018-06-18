using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000052 RID: 82
	[CallbackIdentity(201)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientApprove_t
	{
		// Token: 0x0400009B RID: 155
		public const int k_iCallback = 201;

		// Token: 0x0400009C RID: 156
		public CSteamID m_SteamID;

		// Token: 0x0400009D RID: 157
		public CSteamID m_OwnerSteamID;
	}
}
