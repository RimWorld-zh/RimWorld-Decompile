using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02000020 RID: 32
	internal class ZlibBaseStream : Stream
	{
		// Token: 0x0400015A RID: 346
		protected internal ZlibCodec _z = null;

		// Token: 0x0400015B RID: 347
		protected internal ZlibBaseStream.StreamMode _streamMode = ZlibBaseStream.StreamMode.Undefined;

		// Token: 0x0400015C RID: 348
		protected internal FlushType _flushMode;

		// Token: 0x0400015D RID: 349
		protected internal ZlibStreamFlavor _flavor;

		// Token: 0x0400015E RID: 350
		protected internal CompressionMode _compressionMode;

		// Token: 0x0400015F RID: 351
		protected internal CompressionLevel _level;

		// Token: 0x04000160 RID: 352
		protected internal bool _leaveOpen;

		// Token: 0x04000161 RID: 353
		protected internal byte[] _workingBuffer;

		// Token: 0x04000162 RID: 354
		protected internal int _bufferSize = 16384;

		// Token: 0x04000163 RID: 355
		protected internal byte[] _buf1 = new byte[1];

		// Token: 0x04000164 RID: 356
		protected internal Stream _stream;

		// Token: 0x04000165 RID: 357
		protected internal CompressionStrategy Strategy = CompressionStrategy.Default;

		// Token: 0x04000166 RID: 358
		private CRC32 crc;

		// Token: 0x04000167 RID: 359
		protected internal string _GzipFileName;

		// Token: 0x04000168 RID: 360
		protected internal string _GzipComment;

		// Token: 0x04000169 RID: 361
		protected internal DateTime _GzipMtime;

		// Token: 0x0400016A RID: 362
		protected internal int _gzipHeaderByteCount;

		// Token: 0x0400016B RID: 363
		private bool nomoreinput = false;

		// Token: 0x060000F2 RID: 242 RVA: 0x0000B5C0 File Offset: 0x000099C0
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen)
		{
			this._flushMode = FlushType.None;
			this._stream = stream;
			this._leaveOpen = leaveOpen;
			this._compressionMode = compressionMode;
			this._flavor = flavor;
			this._level = level;
			if (flavor == ZlibStreamFlavor.GZIP)
			{
				this.crc = new CRC32();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000B64C File Offset: 0x00009A4C
		internal int Crc32
		{
			get
			{
				int result;
				if (this.crc == null)
				{
					result = 0;
				}
				else
				{
					result = this.crc.Crc32Result;
				}
				return result;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000B680 File Offset: 0x00009A80
		protected internal bool _wantCompress
		{
			get
			{
				return this._compressionMode == CompressionMode.Compress;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000B6A0 File Offset: 0x00009AA0
		private ZlibCodec z
		{
			get
			{
				if (this._z == null)
				{
					bool flag = this._flavor == ZlibStreamFlavor.ZLIB;
					this._z = new ZlibCodec();
					if (this._compressionMode == CompressionMode.Decompress)
					{
						this._z.InitializeInflate(flag);
					}
					else
					{
						this._z.Strategy = this.Strategy;
						this._z.InitializeDeflate(this._level, flag);
					}
				}
				return this._z;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000B728 File Offset: 0x00009B28
		private byte[] workingBuffer
		{
			get
			{
				if (this._workingBuffer == null)
				{
					this._workingBuffer = new byte[this._bufferSize];
				}
				return this._workingBuffer;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000B760 File Offset: 0x00009B60
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, count);
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				this._streamMode = ZlibBaseStream.StreamMode.Writer;
			}
			else if (this._streamMode != ZlibBaseStream.StreamMode.Writer)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count != 0)
			{
				this.z.InputBuffer = buffer;
				this._z.NextIn = offset;
				this._z.AvailableBytesIn = count;
				for (;;)
				{
					this._z.OutputBuffer = this.workingBuffer;
					this._z.NextOut = 0;
					this._z.AvailableBytesOut = this._workingBuffer.Length;
					int num = (!this._wantCompress) ? this._z.Inflate(this._flushMode) : this._z.Deflate(this._flushMode);
					if (num != 0 && num != 1)
					{
						break;
					}
					this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
					bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
					if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
					{
						flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
					}
					if (flag)
					{
						return;
					}
				}
				throw new ZlibException(((!this._wantCompress) ? "in" : "de") + "flating: " + this._z.Message);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000B920 File Offset: 0x00009D20
		private void finish()
		{
			if (this._z != null)
			{
				if (this._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					int num;
					for (;;)
					{
						this._z.OutputBuffer = this.workingBuffer;
						this._z.NextOut = 0;
						this._z.AvailableBytesOut = this._workingBuffer.Length;
						num = ((!this._wantCompress) ? this._z.Inflate(FlushType.Finish) : this._z.Deflate(FlushType.Finish));
						if (num != 1 && num != 0)
						{
							break;
						}
						if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
						{
							this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
						}
						bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
						if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
						{
							flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
						}
						if (flag)
						{
							goto Block_12;
						}
					}
					string text = ((!this._wantCompress) ? "in" : "de") + "flating";
					if (this._z.Message == null)
					{
						throw new ZlibException(string.Format("{0}: (rc = {1})", text, num));
					}
					throw new ZlibException(text + ": " + this._z.Message);
					Block_12:
					this.Flush();
					if (this._flavor == ZlibStreamFlavor.GZIP)
					{
						if (!this._wantCompress)
						{
							throw new ZlibException("Writing with decompression is not supported.");
						}
						int crc32Result = this.crc.Crc32Result;
						this._stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
						int value = (int)(this.crc.TotalBytesRead & (long)((ulong)-1));
						this._stream.Write(BitConverter.GetBytes(value), 0, 4);
					}
				}
				else if (this._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					if (this._flavor == ZlibStreamFlavor.GZIP)
					{
						if (this._wantCompress)
						{
							throw new ZlibException("Reading with compression is not supported.");
						}
						if (this._z.TotalBytesOut != 0L)
						{
							byte[] array = new byte[8];
							if (this._z.AvailableBytesIn < 8)
							{
								Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, this._z.AvailableBytesIn);
								int num2 = 8 - this._z.AvailableBytesIn;
								int num3 = this._stream.Read(array, this._z.AvailableBytesIn, num2);
								if (num2 != num3)
								{
									throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this._z.AvailableBytesIn + num3));
								}
							}
							else
							{
								Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, array.Length);
							}
							int num4 = BitConverter.ToInt32(array, 0);
							int crc32Result2 = this.crc.Crc32Result;
							int num5 = BitConverter.ToInt32(array, 4);
							int num6 = (int)(this._z.TotalBytesOut & (long)((ulong)-1));
							if (crc32Result2 != num4)
							{
								throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32Result2, num4));
							}
							if (num6 != num5)
							{
								throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num6, num5));
							}
						}
					}
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000BCE8 File Offset: 0x0000A0E8
		private void end()
		{
			if (this.z != null)
			{
				if (this._wantCompress)
				{
					this._z.EndDeflate();
				}
				else
				{
					this._z.EndInflate();
				}
				this._z = null;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000BD3C File Offset: 0x0000A13C
		public override void Close()
		{
			if (this._stream != null)
			{
				try
				{
					this.finish();
				}
				finally
				{
					this.end();
					if (!this._leaveOpen)
					{
						this._stream.Close();
					}
					this._stream = null;
				}
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000BDA0 File Offset: 0x0000A1A0
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000BDAE File Offset: 0x0000A1AE
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000BDB6 File Offset: 0x0000A1B6
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000BDC8 File Offset: 0x0000A1C8
		private string ReadZeroTerminatedString()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			for (;;)
			{
				int num = this._stream.Read(this._buf1, 0, 1);
				if (num != 1)
				{
					break;
				}
				if (this._buf1[0] == 0)
				{
					flag = true;
				}
				else
				{
					list.Add(this._buf1[0]);
				}
				if (flag)
				{
					goto Block_3;
				}
			}
			throw new ZlibException("Unexpected EOF reading GZIP header.");
			Block_3:
			byte[] array = list.ToArray();
			return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000BE50 File Offset: 0x0000A250
		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = this._stream.Read(array, 0, array.Length);
			int result;
			if (num2 == 0)
			{
				result = 0;
			}
			else
			{
				if (num2 != 10)
				{
					throw new ZlibException("Not a valid GZIP stream.");
				}
				if (array[0] != 31 || array[1] != 139 || array[2] != 8)
				{
					throw new ZlibException("Bad GZIP header.");
				}
				int num3 = BitConverter.ToInt32(array, 4);
				this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
				num += num2;
				if ((array[3] & 4) == 4)
				{
					num2 = this._stream.Read(array, 0, 2);
					num += num2;
					short num4 = (short)((int)array[0] + (int)array[1] * 256);
					byte[] array2 = new byte[(int)num4];
					num2 = this._stream.Read(array2, 0, array2.Length);
					if (num2 != (int)num4)
					{
						throw new ZlibException("Unexpected end-of-file reading GZIP header.");
					}
					num += num2;
				}
				if ((array[3] & 8) == 8)
				{
					this._GzipFileName = this.ReadZeroTerminatedString();
				}
				if ((array[3] & 16) == 16)
				{
					this._GzipComment = this.ReadZeroTerminatedString();
				}
				if ((array[3] & 2) == 2)
				{
					this.Read(this._buf1, 0, 1);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000BFA0 File Offset: 0x0000A3A0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._stream.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this._streamMode = ZlibBaseStream.StreamMode.Reader;
				this.z.AvailableBytesIn = 0;
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
					if (this._gzipHeaderByteCount == 0)
					{
						return 0;
					}
				}
			}
			if (this._streamMode != ZlibBaseStream.StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			int result;
			if (count == 0)
			{
				result = 0;
			}
			else if (this.nomoreinput && this._wantCompress)
			{
				result = 0;
			}
			else
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (offset < buffer.GetLowerBound(0))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (offset + count > buffer.GetLength(0))
				{
					throw new ArgumentOutOfRangeException("count");
				}
				this._z.OutputBuffer = buffer;
				this._z.NextOut = offset;
				this._z.AvailableBytesOut = count;
				this._z.InputBuffer = this.workingBuffer;
				int num;
				for (;;)
				{
					if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
					{
						this._z.NextIn = 0;
						this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
						if (this._z.AvailableBytesIn == 0)
						{
							this.nomoreinput = true;
						}
					}
					num = ((!this._wantCompress) ? this._z.Inflate(this._flushMode) : this._z.Deflate(this._flushMode));
					if (this.nomoreinput && num == -5)
					{
						break;
					}
					if (num != 0 && num != 1)
					{
						goto Block_20;
					}
					if ((this.nomoreinput || num == 1) && this._z.AvailableBytesOut == count)
					{
						goto Block_23;
					}
					if (this._z.AvailableBytesOut <= 0 || this.nomoreinput || num != 0)
					{
						goto IL_280;
					}
				}
				return 0;
				Block_20:
				throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", (!this._wantCompress) ? "in" : "de", num, this._z.Message));
				Block_23:
				IL_280:
				if (this._z.AvailableBytesOut > 0)
				{
					if (num != 0 || this._z.AvailableBytesIn == 0)
					{
					}
					if (this.nomoreinput)
					{
						if (this._wantCompress)
						{
							num = this._z.Deflate(FlushType.Finish);
							if (num != 0 && num != 1)
							{
								throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this._z.Message));
							}
						}
					}
				}
				num = count - this._z.AvailableBytesOut;
				if (this.crc != null)
				{
					this.crc.SlurpBlock(buffer, offset, num);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000C2DC File Offset: 0x0000A6DC
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000C2FC File Offset: 0x0000A6FC
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000C31C File Offset: 0x0000A71C
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000C33C File Offset: 0x0000A73C
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000C35C File Offset: 0x0000A75C
		// (set) Token: 0x06000106 RID: 262 RVA: 0x0000C364 File Offset: 0x0000A764
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000C36C File Offset: 0x0000A76C
		public static void CompressString(string s, Stream compressor)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			try
			{
				compressor.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000C3B8 File Offset: 0x0000A7B8
		public static void CompressBuffer(byte[] b, Stream compressor)
		{
			try
			{
				compressor.Write(b, 0, b.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000C3F8 File Offset: 0x0000A7F8
		public static string UncompressString(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			Encoding utf = Encoding.UTF8;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(memoryStream, utf);
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000C4A4 File Offset: 0x0000A8A4
		public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x02000021 RID: 33
		internal enum StreamMode
		{
			// Token: 0x0400016D RID: 365
			Writer,
			// Token: 0x0400016E RID: 366
			Reader,
			// Token: 0x0400016F RID: 367
			Undefined
		}
	}
}
