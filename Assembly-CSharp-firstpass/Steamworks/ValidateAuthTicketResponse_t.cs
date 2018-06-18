using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C6 RID: 198
	[CallbackIdentity(143)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ValidateAuthTicketResponse_t
	{
		// Token: 0x04000259 RID: 601
		public const int k_iCallback = 143;

		// Token: 0x0400025A RID: 602
		public CSteamID m_SteamID;

		// Token: 0x0400025B RID: 603
		public EAuthSessionResponse m_eAuthSessionResponse;

		// Token: 0x0400025C RID: 604
		public CSteamID m_OwnerSteamID;
	}
}
