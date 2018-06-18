using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BF RID: 191
	[CallbackIdentity(2501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUnifiedMessagesSendMethodResult_t
	{
		// Token: 0x04000246 RID: 582
		public const int k_iCallback = 2501;

		// Token: 0x04000247 RID: 583
		public ClientUnifiedMessageHandle m_hHandle;

		// Token: 0x04000248 RID: 584
		public ulong m_unContext;

		// Token: 0x04000249 RID: 585
		public EResult m_eResult;

		// Token: 0x0400024A RID: 586
		public uint m_unResponseSize;
	}
}
