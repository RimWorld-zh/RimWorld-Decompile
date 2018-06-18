using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012B RID: 299
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionState_t
	{
		// Token: 0x04000611 RID: 1553
		public byte m_bConnectionActive;

		// Token: 0x04000612 RID: 1554
		public byte m_bConnecting;

		// Token: 0x04000613 RID: 1555
		public byte m_eP2PSessionError;

		// Token: 0x04000614 RID: 1556
		public byte m_bUsingRelay;

		// Token: 0x04000615 RID: 1557
		public int m_nBytesQueuedForSend;

		// Token: 0x04000616 RID: 1558
		public int m_nPacketsQueuedForSend;

		// Token: 0x04000617 RID: 1559
		public uint m_nRemoteIP;

		// Token: 0x04000618 RID: 1560
		public ushort m_nRemotePort;
	}
}
