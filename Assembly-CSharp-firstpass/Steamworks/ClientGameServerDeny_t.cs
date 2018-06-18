using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C3 RID: 195
	[CallbackIdentity(113)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClientGameServerDeny_t
	{
		// Token: 0x04000250 RID: 592
		public const int k_iCallback = 113;

		// Token: 0x04000251 RID: 593
		public uint m_uAppID;

		// Token: 0x04000252 RID: 594
		public uint m_unGameServerIP;

		// Token: 0x04000253 RID: 595
		public ushort m_usGameServerPort;

		// Token: 0x04000254 RID: 596
		public ushort m_bSecure;

		// Token: 0x04000255 RID: 597
		public uint m_uReason;
	}
}
