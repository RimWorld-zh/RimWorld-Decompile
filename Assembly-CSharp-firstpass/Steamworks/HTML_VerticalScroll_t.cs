using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006A RID: 106
	[CallbackIdentity(4512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_VerticalScroll_t
	{
		// Token: 0x04000104 RID: 260
		public const int k_iCallback = 4512;

		// Token: 0x04000105 RID: 261
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000106 RID: 262
		public uint unScrollMax;

		// Token: 0x04000107 RID: 263
		public uint unScrollCurrent;

		// Token: 0x04000108 RID: 264
		public float flPageScale;

		// Token: 0x04000109 RID: 265
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x0400010A RID: 266
		public uint unPageSize;
	}
}
