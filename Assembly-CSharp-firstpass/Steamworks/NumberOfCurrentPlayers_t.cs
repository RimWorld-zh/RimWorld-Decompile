using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D2 RID: 210
	[CallbackIdentity(1107)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct NumberOfCurrentPlayers_t
	{
		// Token: 0x04000285 RID: 645
		public const int k_iCallback = 1107;

		// Token: 0x04000286 RID: 646
		public byte m_bSuccess;

		// Token: 0x04000287 RID: 647
		public int m_cPlayers;
	}
}
