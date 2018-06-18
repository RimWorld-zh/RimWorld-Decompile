using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000098 RID: 152
	[CallbackIdentity(1201)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SocketStatusCallback_t
	{
		// Token: 0x04000191 RID: 401
		public const int k_iCallback = 1201;

		// Token: 0x04000192 RID: 402
		public SNetSocket_t m_hSocket;

		// Token: 0x04000193 RID: 403
		public SNetListenSocket_t m_hListenSocket;

		// Token: 0x04000194 RID: 404
		public CSteamID m_steamIDRemote;

		// Token: 0x04000195 RID: 405
		public int m_eSNetSocketState;
	}
}
