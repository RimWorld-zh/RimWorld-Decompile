using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000073 RID: 115
	[CallbackIdentity(4525)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_UpdateToolTip_t
	{
		// Token: 0x0400012D RID: 301
		public const int k_iCallback = 4525;

		// Token: 0x0400012E RID: 302
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400012F RID: 303
		public string pchMsg;
	}
}
