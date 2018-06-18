using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000074 RID: 116
	[CallbackIdentity(4526)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HideToolTip_t
	{
		// Token: 0x04000130 RID: 304
		public const int k_iCallback = 4526;

		// Token: 0x04000131 RID: 305
		public HHTMLBrowser unBrowserHandle;
	}
}
