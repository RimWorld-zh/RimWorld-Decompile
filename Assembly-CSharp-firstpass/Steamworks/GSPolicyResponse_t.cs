using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000056 RID: 86
	[CallbackIdentity(115)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSPolicyResponse_t
	{
		// Token: 0x040000A9 RID: 169
		public const int k_iCallback = 115;

		// Token: 0x040000AA RID: 170
		public byte m_bSecure;
	}
}
