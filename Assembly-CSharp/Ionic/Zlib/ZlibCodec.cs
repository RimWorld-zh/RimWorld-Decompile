using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02000022 RID: 34
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x04000170 RID: 368
		public byte[] InputBuffer;

		// Token: 0x04000171 RID: 369
		public int NextIn;

		// Token: 0x04000172 RID: 370
		public int AvailableBytesIn;

		// Token: 0x04000173 RID: 371
		public long TotalBytesIn;

		// Token: 0x04000174 RID: 372
		public byte[] OutputBuffer;

		// Token: 0x04000175 RID: 373
		public int NextOut;

		// Token: 0x04000176 RID: 374
		public int AvailableBytesOut;

		// Token: 0x04000177 RID: 375
		public long TotalBytesOut;

		// Token: 0x04000178 RID: 376
		public string Message;

		// Token: 0x04000179 RID: 377
		internal DeflateManager dstate;

		// Token: 0x0400017A RID: 378
		internal InflateManager istate;

		// Token: 0x0400017B RID: 379
		internal uint _Adler32;

		// Token: 0x0400017C RID: 380
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x0400017D RID: 381
		public int WindowBits = 15;

		// Token: 0x0400017E RID: 382
		public CompressionStrategy Strategy = CompressionStrategy.Default;

		// Token: 0x0600010B RID: 267 RVA: 0x0000C534 File Offset: 0x0000A934
		public ZlibCodec()
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000C554 File Offset: 0x0000A954
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				int num = this.InitializeDeflate();
				if (num != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				int num2 = this.InitializeInflate();
				if (num2 != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000C5D4 File Offset: 0x0000A9D4
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000C5F0 File Offset: 0x0000A9F0
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000C614 File Offset: 0x0000AA14
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000C638 File Offset: 0x0000AA38
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000C65C File Offset: 0x0000AA5C
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000C6A8 File Offset: 0x0000AAA8
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000C6E0 File Offset: 0x0000AAE0
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000C720 File Offset: 0x0000AB20
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000C758 File Offset: 0x0000AB58
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000C774 File Offset: 0x0000AB74
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000C798 File Offset: 0x0000AB98
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000C7BC File Offset: 0x0000ABBC
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000C7E8 File Offset: 0x0000ABE8
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000C814 File Offset: 0x0000AC14
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000C874 File Offset: 0x0000AC74
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000C8AC File Offset: 0x0000ACAC
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000C8DF File Offset: 0x0000ACDF
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000C904 File Offset: 0x0000AD04
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000C93C File Offset: 0x0000AD3C
		public int SetDictionary(byte[] dictionary)
		{
			int result;
			if (this.istate != null)
			{
				result = this.istate.SetDictionary(dictionary);
			}
			else
			{
				if (this.dstate == null)
				{
					throw new ZlibException("No Inflate or Deflate state!");
				}
				result = this.dstate.SetDictionary(dictionary);
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000C990 File Offset: 0x0000AD90
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num != 0)
			{
				if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
				{
					throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
				}
				Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
				this.NextOut += num;
				this.dstate.nextPending += num;
				this.TotalBytesOut += (long)num;
				this.AvailableBytesOut -= num;
				this.dstate.pendingCount -= num;
				if (this.dstate.pendingCount == 0)
				{
					this.dstate.nextPending = 0;
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000CAF8 File Offset: 0x0000AEF8
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			int result;
			if (num == 0)
			{
				result = 0;
			}
			else
			{
				this.AvailableBytesIn -= num;
				if (this.dstate.WantRfc1950HeaderBytes)
				{
					this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
				}
				Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
				this.NextIn += num;
				this.TotalBytesIn += (long)num;
				result = num;
			}
			return result;
		}
	}
}
