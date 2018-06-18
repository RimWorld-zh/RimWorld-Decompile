using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000077 RID: 119
	[CallbackIdentity(2103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestDataReceived_t
	{
		// Token: 0x0400013B RID: 315
		public const int k_iCallback = 2103;

		// Token: 0x0400013C RID: 316
		public HTTPRequestHandle m_hRequest;

		// Token: 0x0400013D RID: 317
		public ulong m_ulContextValue;

		// Token: 0x0400013E RID: 318
		public uint m_cOffset;

		// Token: 0x0400013F RID: 319
		public uint m_cBytesReceived;
	}
}
