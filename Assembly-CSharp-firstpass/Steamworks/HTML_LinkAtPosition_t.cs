using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006B RID: 107
	[CallbackIdentity(4513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_LinkAtPosition_t
	{
		// Token: 0x0400010B RID: 267
		public const int k_iCallback = 4513;

		// Token: 0x0400010C RID: 268
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400010D RID: 269
		public uint x;

		// Token: 0x0400010E RID: 270
		public uint y;

		// Token: 0x0400010F RID: 271
		public string pchURL;

		// Token: 0x04000110 RID: 272
		[MarshalAs(UnmanagedType.I1)]
		public bool bInput;

		// Token: 0x04000111 RID: 273
		[MarshalAs(UnmanagedType.I1)]
		public bool bLiveLink;
	}
}
