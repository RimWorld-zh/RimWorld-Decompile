using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000068 RID: 104
	[CallbackIdentity(4510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CanGoBackAndForward_t
	{
		// Token: 0x040000F9 RID: 249
		public const int k_iCallback = 4510;

		// Token: 0x040000FA RID: 250
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000FB RID: 251
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoBack;

		// Token: 0x040000FC RID: 252
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoForward;
	}
}
