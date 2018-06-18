using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006F RID: 111
	[CallbackIdentity(4521)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NewWindow_t
	{
		// Token: 0x0400011C RID: 284
		public const int k_iCallback = 4521;

		// Token: 0x0400011D RID: 285
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400011E RID: 286
		public string pchURL;

		// Token: 0x0400011F RID: 287
		public uint unX;

		// Token: 0x04000120 RID: 288
		public uint unY;

		// Token: 0x04000121 RID: 289
		public uint unWide;

		// Token: 0x04000122 RID: 290
		public uint unTall;

		// Token: 0x04000123 RID: 291
		public HHTMLBrowser unNewWindow_BrowserHandle;
	}
}
