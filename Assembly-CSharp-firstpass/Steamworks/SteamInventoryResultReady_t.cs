using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000078 RID: 120
	[CallbackIdentity(4700)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryResultReady_t
	{
		// Token: 0x04000140 RID: 320
		public const int k_iCallback = 4700;

		// Token: 0x04000141 RID: 321
		public SteamInventoryResult_t m_handle;

		// Token: 0x04000142 RID: 322
		public EResult m_result;
	}
}
