using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C8 RID: 200
	[CallbackIdentity(154)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EncryptedAppTicketResponse_t
	{
		// Token: 0x04000261 RID: 609
		public const int k_iCallback = 154;

		// Token: 0x04000262 RID: 610
		public EResult m_eResult;
	}
}
