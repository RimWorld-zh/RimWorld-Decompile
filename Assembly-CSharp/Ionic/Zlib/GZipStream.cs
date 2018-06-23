using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x0200000A RID: 10
	public class GZipStream : Stream
	{
		// Token: 0x04000068 RID: 104
		public DateTime? LastModified;

		// Token: 0x04000069 RID: 105
		private int _headerByteCount;

		// Token: 0x0400006A RID: 106
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400006B RID: 107
		private bool _disposed;

		// Token: 0x0400006C RID: 108
		private bool _firstReadDone;

		// Token: 0x0400006D RID: 109
		private string _FileName;

		// Token: 0x0400006E RID: 110
		private string _Comment;

		// Token: 0x0400006F RID: 111
		private int _Crc32;

		// Token: 0x04000070 RID: 112
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04000071 RID: 113
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");

		// Token: 0x06000078 RID: 120 RVA: 0x00005AD0 File Offset: 0x00003ED0
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005ADD File Offset: 0x00003EDD
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005AEA File Offset: 0x00003EEA
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005AF7 File Offset: 0x00003EF7
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00005B18 File Offset: 0x00003F18
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00005B33 File Offset: 0x00003F33
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00005B54 File Offset: 0x00003F54
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00005B70 File Offset: 0x00003F70
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName != null)
				{
					if (this._FileName.IndexOf("/") != -1)
					{
						this._FileName = this._FileName.Replace("/", "\\");
					}
					if (this._FileName.EndsWith("\\"))
					{
						throw new Exception("Illegal filename");
					}
					if (this._FileName.IndexOf("\\") != -1)
					{
						this._FileName = Path.GetFileName(this._FileName);
					}
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00005C28 File Offset: 0x00004028
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00005C44 File Offset: 0x00004044
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00005C64 File Offset: 0x00004064
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00005C8C File Offset: 0x0000408C
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00005CAC File Offset: 0x000040AC
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00005D24 File Offset: 0x00004124
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00005D4C File Offset: 0x0000414C
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005D74 File Offset: 0x00004174
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00005DE8 File Offset: 0x000041E8
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00005E24 File Offset: 0x00004224
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00005E3C File Offset: 0x0000423C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005E77 File Offset: 0x00004277
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005E9B File Offset: 0x0000429B
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00005EA4 File Offset: 0x000042A4
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00005F1D File Offset: 0x0000431D
		public override long Position
		{
			get
			{
				long result;
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					result = this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				else if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					result = this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
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

		// Token: 0x0600008F RID: 143 RVA: 0x00005F28 File Offset: 0x00004328
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005F99 File Offset: 0x00004399
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005FA1 File Offset: 0x000043A1
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005FAC File Offset: 0x000043AC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000601C File Offset: 0x0000441C
		private int EmitHeader()
		{
			byte[] array = (this.Comment != null) ? GZipStream.iso8859dash1.GetBytes(this.Comment) : null;
			byte[] array2 = (this.FileName != null) ? GZipStream.iso8859dash1.GetBytes(this.FileName) : null;
			int num = (this.Comment != null) ? (array.Length + 1) : 0;
			int num2 = (this.FileName != null) ? (array2.Length + 1) : 0;
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			int num4 = 0;
			array3[num4++] = 31;
			array3[num4++] = 139;
			array3[num4++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num4++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			int value = (int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds;
			Array.Copy(BitConverter.GetBytes(value), 0, array3, num4, 4);
			num4 += 4;
			array3[num4++] = 0;
			array3[num4++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num4, num2 - 1);
				num4 += num2 - 1;
				array3[num4++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num4, num - 1);
				num4 += num - 1;
				array3[num4++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000061F4 File Offset: 0x000045F4
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006244 File Offset: 0x00004644
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006294 File Offset: 0x00004694
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000062E0 File Offset: 0x000046E0
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}
	}
}
