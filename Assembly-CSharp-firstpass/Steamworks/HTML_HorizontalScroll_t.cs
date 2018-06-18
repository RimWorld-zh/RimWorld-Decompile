using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000069 RID: 105
	[CallbackIdentity(4511)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HorizontalScroll_t
	{
		// Token: 0x040000FD RID: 253
		public const int k_iCallback = 4511;

		// Token: 0x040000FE RID: 254
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000FF RID: 255
		public uint unScrollMax;

		// Token: 0x04000100 RID: 256
		public uint unScrollCurrent;

		// Token: 0x04000101 RID: 257
		public float flPageScale;

		// Token: 0x04000102 RID: 258
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x04000103 RID: 259
		public uint unPageSize;
	}
}
