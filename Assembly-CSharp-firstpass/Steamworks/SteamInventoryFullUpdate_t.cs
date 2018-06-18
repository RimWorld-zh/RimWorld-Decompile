using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000079 RID: 121
	[CallbackIdentity(4701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryFullUpdate_t
	{
		// Token: 0x04000143 RID: 323
		public const int k_iCallback = 4701;

		// Token: 0x04000144 RID: 324
		public SteamInventoryResult_t m_handle;
	}
}
