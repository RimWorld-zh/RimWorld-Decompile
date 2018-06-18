using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000070 RID: 112
	[CallbackIdentity(4522)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SetCursor_t
	{
		// Token: 0x04000124 RID: 292
		public const int k_iCallback = 4522;

		// Token: 0x04000125 RID: 293
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000126 RID: 294
		public uint eMouseCursor;
	}
}
