using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006E RID: 110
	[CallbackIdentity(4516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FileOpenDialog_t
	{
		// Token: 0x04000118 RID: 280
		public const int k_iCallback = 4516;

		// Token: 0x04000119 RID: 281
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400011A RID: 282
		public string pchTitle;

		// Token: 0x0400011B RID: 283
		public string pchInitialFile;
	}
}
