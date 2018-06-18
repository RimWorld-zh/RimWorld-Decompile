using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000064 RID: 100
	[CallbackIdentity(4506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FinishedRequest_t
	{
		// Token: 0x040000EB RID: 235
		public const int k_iCallback = 4506;

		// Token: 0x040000EC RID: 236
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000ED RID: 237
		public string pchURL;

		// Token: 0x040000EE RID: 238
		public string pchPageTitle;
	}
}
