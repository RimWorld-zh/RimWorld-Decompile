using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000067 RID: 103
	[CallbackIdentity(4509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SearchResults_t
	{
		// Token: 0x040000F5 RID: 245
		public const int k_iCallback = 4509;

		// Token: 0x040000F6 RID: 246
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F7 RID: 247
		public uint unResults;

		// Token: 0x040000F8 RID: 248
		public uint unCurrentMatch;
	}
}
