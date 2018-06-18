using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000060 RID: 96
	[CallbackIdentity(4502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NeedsPaint_t
	{
		// Token: 0x040000CF RID: 207
		public const int k_iCallback = 4502;

		// Token: 0x040000D0 RID: 208
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040000D1 RID: 209
		public IntPtr pBGRA;

		// Token: 0x040000D2 RID: 210
		public uint unWide;

		// Token: 0x040000D3 RID: 211
		public uint unTall;

		// Token: 0x040000D4 RID: 212
		public uint unUpdateX;

		// Token: 0x040000D5 RID: 213
		public uint unUpdateY;

		// Token: 0x040000D6 RID: 214
		public uint unUpdateWide;

		// Token: 0x040000D7 RID: 215
		public uint unUpdateTall;

		// Token: 0x040000D8 RID: 216
		public uint unScrollX;

		// Token: 0x040000D9 RID: 217
		public uint unScrollY;

		// Token: 0x040000DA RID: 218
		public float flPageScale;

		// Token: 0x040000DB RID: 219
		public uint unPageSerial;
	}
}
