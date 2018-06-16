using System;

namespace Ionic.Zlib
{
	// Token: 0x02000023 RID: 35
	public static class ZlibConstants
	{
		// Token: 0x0400017F RID: 383
		public const int WindowBitsMax = 15;

		// Token: 0x04000180 RID: 384
		public const int WindowBitsDefault = 15;

		// Token: 0x04000181 RID: 385
		public const int Z_OK = 0;

		// Token: 0x04000182 RID: 386
		public const int Z_STREAM_END = 1;

		// Token: 0x04000183 RID: 387
		public const int Z_NEED_DICT = 2;

		// Token: 0x04000184 RID: 388
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x04000185 RID: 389
		public const int Z_DATA_ERROR = -3;

		// Token: 0x04000186 RID: 390
		public const int Z_BUF_ERROR = -5;

		// Token: 0x04000187 RID: 391
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x04000188 RID: 392
		public const int WorkingBufferSizeMin = 1024;
	}
}
