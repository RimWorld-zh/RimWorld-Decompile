using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000043 RID: 67
	[CallbackIdentity(335)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClanOfficerListResponse_t
	{
		// Token: 0x04000069 RID: 105
		public const int k_iCallback = 335;

		// Token: 0x0400006A RID: 106
		public CSteamID m_steamIDClan;

		// Token: 0x0400006B RID: 107
		public int m_cOfficers;

		// Token: 0x0400006C RID: 108
		public byte m_bSuccess;
	}
}
