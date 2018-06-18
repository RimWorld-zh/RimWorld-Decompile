using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000097 RID: 151
	[CallbackIdentity(1203)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct P2PSessionConnectFail_t
	{
		// Token: 0x0400018E RID: 398
		public const int k_iCallback = 1203;

		// Token: 0x0400018F RID: 399
		public CSteamID m_steamIDRemote;

		// Token: 0x04000190 RID: 400
		public byte m_eP2PSessionError;
	}
}
