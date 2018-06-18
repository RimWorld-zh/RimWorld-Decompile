using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005F RID: 95
	[CallbackIdentity(4501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_BrowserReady_t
	{
		// Token: 0x040000CD RID: 205
		public const int k_iCallback = 4501;

		// Token: 0x040000CE RID: 206
		public HHTMLBrowser unBrowserHandle;
	}
}
