using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B4 RID: 180
	[CallbackIdentity(2301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ScreenshotReady_t
	{
		// Token: 0x0400021D RID: 541
		public const int k_iCallback = 2301;

		// Token: 0x0400021E RID: 542
		public ScreenshotHandle m_hLocal;

		// Token: 0x0400021F RID: 543
		public EResult m_eResult;
	}
}
