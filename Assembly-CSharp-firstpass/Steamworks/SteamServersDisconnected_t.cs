using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C2 RID: 194
	[CallbackIdentity(103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServersDisconnected_t
	{
		// Token: 0x0400024E RID: 590
		public const int k_iCallback = 103;

		// Token: 0x0400024F RID: 591
		public EResult m_eResult;
	}
}
