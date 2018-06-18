using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000071 RID: 113
	[CallbackIdentity(4523)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StatusText_t
	{
		// Token: 0x04000127 RID: 295
		public const int k_iCallback = 4523;

		// Token: 0x04000128 RID: 296
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000129 RID: 297
		public string pchMsg;
	}
}
