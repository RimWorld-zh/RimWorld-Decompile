using System;

namespace Ionic.Zlib
{
	// Token: 0x02000012 RID: 18
	internal class WorkItem
	{
		// Token: 0x040000E0 RID: 224
		public byte[] buffer;

		// Token: 0x040000E1 RID: 225
		public byte[] compressed;

		// Token: 0x040000E2 RID: 226
		public int crc;

		// Token: 0x040000E3 RID: 227
		public int index;

		// Token: 0x040000E4 RID: 228
		public int ordinal;

		// Token: 0x040000E5 RID: 229
		public int inputBytesAvailable;

		// Token: 0x040000E6 RID: 230
		public int compressedBytesAvailable;

		// Token: 0x040000E7 RID: 231
		public ZlibCodec compressor;

		// Token: 0x060000B9 RID: 185 RVA: 0x00009F14 File Offset: 0x00008314
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}
	}
}
