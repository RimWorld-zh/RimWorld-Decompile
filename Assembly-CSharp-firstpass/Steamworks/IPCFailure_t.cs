using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C4 RID: 196
	[CallbackIdentity(117)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct IPCFailure_t
	{
		// Token: 0x04000256 RID: 598
		public const int k_iCallback = 117;

		// Token: 0x04000257 RID: 599
		public byte m_eFailureType;
	}
}
