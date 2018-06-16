using System;

namespace Ionic.Zlib
{
	// Token: 0x0200001C RID: 28
	internal static class InternalConstants
	{
		// Token: 0x04000140 RID: 320
		internal static readonly int MAX_BITS = 15;

		// Token: 0x04000141 RID: 321
		internal static readonly int BL_CODES = 19;

		// Token: 0x04000142 RID: 322
		internal static readonly int D_CODES = 30;

		// Token: 0x04000143 RID: 323
		internal static readonly int LITERALS = 256;

		// Token: 0x04000144 RID: 324
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x04000145 RID: 325
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x04000146 RID: 326
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x04000147 RID: 327
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04000148 RID: 328
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04000149 RID: 329
		internal static readonly int REPZ_11_138 = 18;
	}
}
