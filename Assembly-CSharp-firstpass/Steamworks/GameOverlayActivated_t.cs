using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003F RID: 63
	[CallbackIdentity(331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameOverlayActivated_t
	{
		// Token: 0x0400005C RID: 92
		public const int k_iCallback = 331;

		// Token: 0x0400005D RID: 93
		public byte m_bActive;
	}
}
