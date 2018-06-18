using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DA RID: 218
	[CallbackIdentity(703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAPICallCompleted_t
	{
		// Token: 0x0400029B RID: 667
		public const int k_iCallback = 703;

		// Token: 0x0400029C RID: 668
		public SteamAPICall_t m_hAsyncCall;
	}
}
