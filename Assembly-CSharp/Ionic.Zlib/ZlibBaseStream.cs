using Ionic.Crc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	internal class ZlibBaseStream : Stream
	{
		internal enum StreamMode
		{
			Writer,
			Reader,
			Undefined
		}

		protected internal ZlibCodec _z;

		protected internal StreamMode _streamMode = StreamMode.Undefined;

		protected internal FlushType _flushMode;

		protected internal ZlibStreamFlavor _flavor;

		protected internal CompressionMode _compressionMode;

		protected internal CompressionLevel _level;

		protected internal bool _leaveOpen;

		protected internal byte[] _workingBuffer;

		protected internal int _bufferSize = 16384;

		protected internal byte[] _buf1 = new byte[1];

		protected internal Stream _stream;

		protected internal CompressionStrategy Strategy;

		private CRC32 crc;

		protected internal string _GzipFileName;

		protected internal string _GzipComment;

		protected internal DateTime _GzipMtime;

		protected internal int _gzipHeaderByteCount;

		private bool nomoreinput;

		internal int Crc32
		{
			get
			{
				if (this.crc == null)
				{
					return 0;
				}
				return this.crc.Crc32Result;
			}
		}

		protected internal bool _wantCompress
		{
			get
			{
				return this._compressionMode == CompressionMode.Compress;
			}
		}

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

		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

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

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, count);
			}
			if (this._streamMode == StreamMode.Undefined)
			{
				this._streamMode = StreamMode.Writer;
			}
			else if (this._streamMode != 0)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count != 0)
			{
				this.z.InputBuffer = buffer;
				this._z.NextIn = offset;
				this._z.AvailableBytesIn = count;
				bool flag = false;
				while (true)
				{
					this._z.OutputBuffer = this.workingBuffer;
					this._z.NextOut = 0;
					this._z.AvailableBytesOut = this._workingBuffer.Length;
					int num = (!this._wantCompress) ? this._z.Inflate(this._flushMode) : this._z.Deflate(this._flushMode);
					if (num != 0 && num != 1)
						break;
					this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
					flag = (this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0);
					if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
					{
						flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
					}
					if (flag)
						return;
				}
				throw new ZlibException(((!this._wantCompress) ? "in" : "de") + "flating: " + this._z.Message);
			}
		}

		private void finish()
		{
			if (this._z != null)
			{
				if (this._streamMode == StreamMode.Writer)
				{
					bool flag = false;
					while (true)
					{
						this._z.OutputBuffer = this.workingBuffer;
						this._z.NextOut = 0;
						this._z.AvailableBytesOut = this._workingBuffer.Length;
						int num = (!this._wantCompress) ? this._z.Inflate(FlushType.Finish) : this._z.Deflate(FlushType.Finish);
						if (num != 1 && num != 0)
						{
							string text = ((!this._wantCompress) ? "in" : "de") + "flating";
							if (this._z.Message == null)
							{
								throw new ZlibException(string.Format("{0}: (rc = {1})", text, num));
							}
							throw new ZlibException(text + ": " + this._z.Message);
						}
						if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
						{
							this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
						}
						flag = (this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0);
						if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
						{
							flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
						}
						if (flag)
							break;
					}
					this.Flush();
					if (this._flavor != ZlibStreamFlavor.GZIP)
						return;
					if (this._wantCompress)
					{
						int crc32Result = this.crc.Crc32Result;
						this._stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
						int value = (int)(this.crc.TotalBytesRead & 4294967295u);
						this._stream.Write(BitConverter.GetBytes(value), 0, 4);
						return;
					}
					throw new ZlibException("Writing with decompression is not supported.");
				}
				if (this._streamMode != StreamMode.Reader)
					return;
				if (this._flavor != ZlibStreamFlavor.GZIP)
					return;
				if (!this._wantCompress)
				{
					if (this._z.TotalBytesOut != 0)
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
						int num6 = (int)(this._z.TotalBytesOut & 4294967295u);
						if (crc32Result2 != num4)
						{
							throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32Result2, num4));
						}
						if (num6 == num5)
							return;
						throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num6, num5));
					}
					return;
				}
				throw new ZlibException("Reading with compression is not supported.");
			}
		}

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

		public override void Flush()
		{
			this._stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		private string ReadZeroTerminatedString()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			while (true)
			{
				int num = this._stream.Read(this._buf1, 0, 1);
				if (num != 1)
				{
					throw new ZlibException("Unexpected EOF reading GZIP header.");
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
					break;
			}
			byte[] array = list.ToArray();
			return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
		}

		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = this._stream.Read(array, 0, array.Length);
			switch (num2)
			{
			case 0:
				return 0;
			default:
				throw new ZlibException("Not a valid GZIP stream.");
			case 10:
				if (array[0] == 31 && array[1] == 139 && array[2] == 8)
				{
					int num3 = BitConverter.ToInt32(array, 4);
					this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
					num += num2;
					if ((array[3] & 4) == 4)
					{
						num2 = this._stream.Read(array, 0, 2);
						num += num2;
						short num4 = (short)(array[0] + array[1] * 256);
						byte[] array2 = new byte[num4];
						num2 = this._stream.Read(array2, 0, array2.Length);
						if (num2 != num4)
						{
							throw new ZlibException("Unexpected end-of-file reading GZIP header.");
						}
						num += num2;
					}
					if ((array[3] & 8) == 8)
					{
						this._GzipFileName = this.ReadZeroTerminatedString();
					}
					if ((array[3] & 0x10) == 16)
					{
						this._GzipComment = this.ReadZeroTerminatedString();
					}
					if ((array[3] & 2) == 2)
					{
						this.Read(this._buf1, 0, 1);
					}
					return num;
				}
				throw new ZlibException("Bad GZIP header.");
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._streamMode == StreamMode.Undefined)
			{
				if (!this._stream.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this._streamMode = StreamMode.Reader;
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
			if (this._streamMode != StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.nomoreinput && this._wantCompress)
			{
				return 0;
			}
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
			int num = 0;
			this._z.OutputBuffer = buffer;
			this._z.NextOut = offset;
			this._z.AvailableBytesOut = count;
			this._z.InputBuffer = this.workingBuffer;
			while (true)
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
					return 0;
				}
				if (num != 0 && num != 1)
				{
					throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", (!this._wantCompress) ? "in" : "de", num, this._z.Message));
				}
				if ((this.nomoreinput || num == 1) && this._z.AvailableBytesOut == count)
					break;
				if (this._z.AvailableBytesOut <= 0)
					break;
				if (this.nomoreinput)
					break;
				if (num != 0)
					break;
			}
			if (this._z.AvailableBytesOut > 0)
			{
				if (num != 0 || this._z.AvailableBytesIn != 0)
					;
				if (this.nomoreinput && this._wantCompress)
				{
					num = this._z.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this._z.Message));
					}
				}
			}
			num = count - this._z.AvailableBytesOut;
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		public static void CompressString(string s, Stream compressor)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			using (compressor)
			{
				compressor.Write(bytes, 0, bytes.Length);
			}
		}

		public static void CompressBuffer(byte[] b, Stream compressor)
		{
			using (compressor)
			{
				compressor.Write(b, 0, b.Length);
			}
		}

		public static string UncompressString(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			Encoding uTF = Encoding.UTF8;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (decompressor)
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(memoryStream, uTF);
				return streamReader.ReadToEnd();
			}
		}

		public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (decompressor)
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				return memoryStream.ToArray();
			}
		}
	}
}
