using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006D RID: 109
	[CallbackIdentity(4515)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSConfirm_t
	{
		// Token: 0x04000115 RID: 277
		public const int k_iCallback = 4515;

		// Token: 0x04000116 RID: 278
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000117 RID: 279
		public string pchMessage;
	}
}
