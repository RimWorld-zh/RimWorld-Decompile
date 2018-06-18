using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000076 RID: 118
	[CallbackIdentity(2102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestHeadersReceived_t
	{
		// Token: 0x04000138 RID: 312
		public const int k_iCallback = 2102;

		// Token: 0x04000139 RID: 313
		public HTTPRequestHandle m_hRequest;

		// Token: 0x0400013A RID: 314
		public ulong m_ulContextValue;
	}
}
