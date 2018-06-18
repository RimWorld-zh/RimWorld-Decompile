using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000096 RID: 150
	[CallbackIdentity(1202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionRequest_t
	{
		// Token: 0x0400018C RID: 396
		public const int k_iCallback = 1202;

		// Token: 0x0400018D RID: 397
		public CSteamID m_steamIDRemote;
	}
}
