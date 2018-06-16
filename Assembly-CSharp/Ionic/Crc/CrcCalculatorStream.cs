using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x02000003 RID: 3
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000283D File Offset: 0x00000C3D
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000284E File Offset: 0x00000C4E
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000285F File Offset: 0x00000C5F
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000287F File Offset: 0x00000C7F
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000289F File Offset: 0x00000C9F
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000028C0 File Offset: 0x00000CC0
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._innerStream = stream;
			this._crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002918 File Offset: 0x00000D18
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002938 File Offset: 0x00000D38
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002958 File Offset: 0x00000D58
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002973 File Offset: 0x00000D73
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002980 File Offset: 0x00000D80
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A09 File Offset: 0x00000E09
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002A30 File Offset: 0x00000E30
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002A50 File Offset: 0x00000E50
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002A68 File Offset: 0x00000E68
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A88 File Offset: 0x00000E88
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002A98 File Offset: 0x00000E98
		public override long Length
		{
			get
			{
				long result;
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					result = this._innerStream.Length;
				}
				else
				{
					result = this._lengthLimit;
				}
				return result;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002AD4 File Offset: 0x00000ED4
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002AF4 File Offset: 0x00000EF4
		public override long Position
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AFC File Offset: 0x00000EFC
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B04 File Offset: 0x00000F04
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B0C File Offset: 0x00000F0C
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B15 File Offset: 0x00000F15
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x04000007 RID: 7
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x04000008 RID: 8
		private readonly Stream _innerStream;

		// Token: 0x04000009 RID: 9
		private readonly CRC32 _crc32;

		// Token: 0x0400000A RID: 10
		private readonly long _lengthLimit = -99L;

		// Token: 0x0400000B RID: 11
		private bool _leaveOpen;
	}
}
