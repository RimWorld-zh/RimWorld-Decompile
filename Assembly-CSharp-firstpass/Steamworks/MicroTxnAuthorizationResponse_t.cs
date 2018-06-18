using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C7 RID: 199
	[CallbackIdentity(152)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MicroTxnAuthorizationResponse_t
	{
		// Token: 0x0400025D RID: 605
		public const int k_iCallback = 152;

		// Token: 0x0400025E RID: 606
		public uint m_unAppID;

		// Token: 0x0400025F RID: 607
		public ulong m_ulOrderID;

		// Token: 0x04000260 RID: 608
		public byte m_bAuthorized;
	}
}
