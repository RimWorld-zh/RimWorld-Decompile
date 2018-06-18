using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006C RID: 108
	[CallbackIdentity(4514)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSAlert_t
	{
		// Token: 0x04000112 RID: 274
		public const int k_iCallback = 4514;

		// Token: 0x04000113 RID: 275
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000114 RID: 276
		public string pchMessage;
	}
}
