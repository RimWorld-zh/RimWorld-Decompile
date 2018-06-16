using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02000024 RID: 36
	public class ZlibStream : Stream
	{
		// Token: 0x06000122 RID: 290 RVA: 0x0000CB9A File Offset: 0x0000AF9A
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000CBA7 File Offset: 0x0000AFA7
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000CBB4 File Offset: 0x0000AFB4
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000CBC1 File Offset: 0x0000AFC1
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000CBE0 File Offset: 0x0000AFE0
		// (set) Token: 0x06000127 RID: 295 RVA: 0x0000CC00 File Offset: 0x0000B000
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000CC28 File Offset: 0x0000B028
		// (set) Token: 0x06000129 RID: 297 RVA: 0x0000CC48 File Offset: 0x0000B048
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600012A RID: 298 RVA: 0x0000CCC0 File Offset: 0x0000B0C0
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000CCE8 File Offset: 0x0000B0E8
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000CD10 File Offset: 0x0000B110
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000CD70 File Offset: 0x0000B170
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000CDAC File Offset: 0x0000B1AC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000CDC4 File Offset: 0x0000B1C4
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000CDFF File Offset: 0x0000B1FF
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000CE23 File Offset: 0x0000B223
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000CE2C File Offset: 0x0000B22C
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000CE90 File Offset: 0x0000B290
		public override long Position
		{
			get
			{
				long result;
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					result = this._baseStream._z.TotalBytesOut;
				}
				else if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					result = this._baseStream._z.TotalBytesIn;
				}
				else
				{
					result = 0L;
				}
				return result;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000CE98 File Offset: 0x0000B298
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000CED1 File Offset: 0x0000B2D1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000CED9 File Offset: 0x0000B2D9
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000CEE1 File Offset: 0x0000B2E1
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000CF08 File Offset: 0x0000B308
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000CF58 File Offset: 0x0000B358
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000CFA8 File Offset: 0x0000B3A8
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000CFF4 File Offset: 0x0000B3F4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000189 RID: 393
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400018A RID: 394
		private bool _disposed;
	}
}
