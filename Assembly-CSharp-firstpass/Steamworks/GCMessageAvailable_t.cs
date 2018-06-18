using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000050 RID: 80
	[CallbackIdentity(1701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GCMessageAvailable_t
	{
		// Token: 0x04000098 RID: 152
		public const int k_iCallback = 1701;

		// Token: 0x04000099 RID: 153
		public uint m_nMessageSize;
	}
}
