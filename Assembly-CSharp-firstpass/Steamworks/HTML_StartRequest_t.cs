using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000061 RID: 97
	[CallbackIdentity(4503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StartRequest_t
	{
		// Token: 0x040000DC RID: 220
		public const int k_iCallback = 4503;

		// Token: 0x040000DD RID: 221
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000DE RID: 222
		public string pchURL;

		// Token: 0x040000DF RID: 223
		public string pchTarget;

		// Token: 0x040000E0 RID: 224
		public string pchPostData;

		// Token: 0x040000E1 RID: 225
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;
	}
}
