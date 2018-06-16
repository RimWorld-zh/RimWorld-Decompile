using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02000009 RID: 9
	public class DeflateStream : Stream
	{
		// Token: 0x0600005C RID: 92 RVA: 0x000055DC File Offset: 0x000039DC
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000055E9 File Offset: 0x000039E9
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000055F6 File Offset: 0x000039F6
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005603 File Offset: 0x00003A03
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00005628 File Offset: 0x00003A28
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00005648 File Offset: 0x00003A48
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00005670 File Offset: 0x00003A70
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00005690 File Offset: 0x00003A90
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00005708 File Offset: 0x00003B08
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00005728 File Offset: 0x00003B28
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00005750 File Offset: 0x00003B50
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00005778 File Offset: 0x00003B78
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000057A0 File Offset: 0x00003BA0
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00005800 File Offset: 0x00003C00
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000583C File Offset: 0x00003C3C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00005854 File Offset: 0x00003C54
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000588F File Offset: 0x00003C8F
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000058B3 File Offset: 0x00003CB3
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000058BC File Offset: 0x00003CBC
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00005920 File Offset: 0x00003D20
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
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005928 File Offset: 0x00003D28
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005961 File Offset: 0x00003D61
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005969 File Offset: 0x00003D69
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005971 File Offset: 0x00003D71
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005998 File Offset: 0x00003D98
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000059E8 File Offset: 0x00003DE8
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005A38 File Offset: 0x00003E38
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005A84 File Offset: 0x00003E84
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000065 RID: 101
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000066 RID: 102
		internal Stream _innerStream;

		// Token: 0x04000067 RID: 103
		private bool _disposed;
	}
}
