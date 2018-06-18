using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000065 RID: 101
	[CallbackIdentity(4507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_OpenLinkInNewTab_t
	{
		// Token: 0x040000EF RID: 239
		public const int k_iCallback = 4507;

		// Token: 0x040000F0 RID: 240
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F1 RID: 241
		public string pchURL;
	}
}
