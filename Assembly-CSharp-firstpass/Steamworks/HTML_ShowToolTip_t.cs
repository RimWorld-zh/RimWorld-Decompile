using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000072 RID: 114
	[CallbackIdentity(4524)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ShowToolTip_t
	{
		// Token: 0x0400012A RID: 298
		public const int k_iCallback = 4524;

		// Token: 0x0400012B RID: 299
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400012C RID: 300
		public string pchMsg;
	}
}
