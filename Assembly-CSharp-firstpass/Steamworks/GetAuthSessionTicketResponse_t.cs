using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C9 RID: 201
	[CallbackIdentity(163)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAuthSessionTicketResponse_t
	{
		// Token: 0x04000263 RID: 611
		public const int k_iCallback = 163;

		// Token: 0x04000264 RID: 612
		public HAuthTicket m_hAuthTicket;

		// Token: 0x04000265 RID: 613
		public EResult m_eResult;
	}
}
