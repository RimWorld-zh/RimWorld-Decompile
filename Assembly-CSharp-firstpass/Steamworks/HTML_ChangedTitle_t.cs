using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000066 RID: 102
	[CallbackIdentity(4508)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ChangedTitle_t
	{
		// Token: 0x040000F2 RID: 242
		public const int k_iCallback = 4508;

		// Token: 0x040000F3 RID: 243
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000F4 RID: 244
		public string pchTitle;
	}
}
