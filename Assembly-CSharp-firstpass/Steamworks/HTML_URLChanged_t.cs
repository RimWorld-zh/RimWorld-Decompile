using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000063 RID: 99
	[CallbackIdentity(4505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_URLChanged_t
	{
		// Token: 0x040000E4 RID: 228
		public const int k_iCallback = 4505;

		// Token: 0x040000E5 RID: 229
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000E6 RID: 230
		public string pchURL;

		// Token: 0x040000E7 RID: 231
		public string pchPostData;

		// Token: 0x040000E8 RID: 232
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;

		// Token: 0x040000E9 RID: 233
		public string pchPageTitle;

		// Token: 0x040000EA RID: 234
		[MarshalAs(UnmanagedType.I1)]
		public bool bNewNavigation;
	}
}
