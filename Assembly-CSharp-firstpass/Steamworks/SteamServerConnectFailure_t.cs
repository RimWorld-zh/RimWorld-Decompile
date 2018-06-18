using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C1 RID: 193
	[CallbackIdentity(102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServerConnectFailure_t
	{
		// Token: 0x0400024C RID: 588
		public const int k_iCallback = 102;

		// Token: 0x0400024D RID: 589
		public EResult m_eResult;
	}
}
