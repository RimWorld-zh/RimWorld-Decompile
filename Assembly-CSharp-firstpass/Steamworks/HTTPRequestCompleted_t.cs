using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000075 RID: 117
	[CallbackIdentity(2101)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestCompleted_t
	{
		// Token: 0x04000132 RID: 306
		public const int k_iCallback = 2101;

		// Token: 0x04000133 RID: 307
		public HTTPRequestHandle m_hRequest;

		// Token: 0x04000134 RID: 308
		public ulong m_ulContextValue;

		// Token: 0x04000135 RID: 309
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bRequestSuccessful;

		// Token: 0x04000136 RID: 310
		public EHTTPStatusCode m_eStatusCode;

		// Token: 0x04000137 RID: 311
		public uint m_unBodySize;
	}
}
