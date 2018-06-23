using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02000013 RID: 19
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x040000E8 RID: 232
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x040000E9 RID: 233
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x040000EA RID: 234
		private List<WorkItem> _pool;

		// Token: 0x040000EB RID: 235
		private bool _leaveOpen;

		// Token: 0x040000EC RID: 236
		private bool emitting;

		// Token: 0x040000ED RID: 237
		private Stream _outStream;

		// Token: 0x040000EE RID: 238
		private int _maxBufferPairs;

		// Token: 0x040000EF RID: 239
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x040000F0 RID: 240
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x040000F1 RID: 241
		private object _outputLock = new object();

		// Token: 0x040000F2 RID: 242
		private bool _isClosed;

		// Token: 0x040000F3 RID: 243
		private bool _firstWriteDone;

		// Token: 0x040000F4 RID: 244
		private int _currentlyFilling;

		// Token: 0x040000F5 RID: 245
		private int _lastFilled;

		// Token: 0x040000F6 RID: 246
		private int _lastWritten;

		// Token: 0x040000F7 RID: 247
		private int _latestCompressed;

		// Token: 0x040000F8 RID: 248
		private int _Crc32;

		// Token: 0x040000F9 RID: 249
		private CRC32 _runningCrc;

		// Token: 0x040000FA RID: 250
		private object _latestLock = new object();

		// Token: 0x040000FB RID: 251
		private Queue<int> _toWrite;

		// Token: 0x040000FC RID: 252
		private Queue<int> _toFill;

		// Token: 0x040000FD RID: 253
		private long _totalBytesProcessed;

		// Token: 0x040000FE RID: 254
		private CompressionLevel _compressLevel;

		// Token: 0x040000FF RID: 255
		private volatile Exception _pendingException;

		// Token: 0x04000100 RID: 256
		private bool _handlingException;

		// Token: 0x04000101 RID: 257
		private object _eLock = new object();

		// Token: 0x04000102 RID: 258
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x060000BA RID: 186 RVA: 0x00009F93 File Offset: 0x00008393
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00009FA0 File Offset: 0x000083A0
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00009FAD File Offset: 0x000083AD
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00009FBA File Offset: 0x000083BA
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00009FC8 File Offset: 0x000083C8
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000A038 File Offset: 0x00008438
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000A052 File Offset: 0x00008452
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000A05C File Offset: 0x0000845C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000A077 File Offset: 0x00008477
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000A098 File Offset: 0x00008498
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000A0B3 File Offset: 0x000084B3
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000A0D8 File Offset: 0x000084D8
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000A0F4 File Offset: 0x000084F4
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000A110 File Offset: 0x00008510
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000A1D0 File Offset: 0x000085D0
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count != 0)
			{
				if (!this._firstWriteDone)
				{
					this._InitializePoolOfWorkItems();
					this._firstWriteDone = true;
				}
				for (;;)
				{
					this.EmitPendingBuffers(false, mustWait);
					mustWait = false;
					int num;
					if (this._currentlyFilling >= 0)
					{
						num = this._currentlyFilling;
						goto IL_BD;
					}
					if (this._toFill.Count != 0)
					{
						num = this._toFill.Dequeue();
						this._lastFilled++;
						goto IL_BD;
					}
					mustWait = true;
					IL_184:
					if (count <= 0)
					{
						return;
					}
					continue;
					IL_BD:
					WorkItem workItem = this._pool[num];
					int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable <= count) ? (workItem.buffer.Length - workItem.inputBytesAvailable) : count;
					workItem.ordinal = this._lastFilled;
					Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
					count -= num2;
					offset += num2;
					workItem.inputBytesAvailable += num2;
					if (workItem.inputBytesAvailable == workItem.buffer.Length)
					{
						if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
						{
							break;
						}
						this._currentlyFilling = -1;
					}
					else
					{
						this._currentlyFilling = num;
					}
					if (count > 0)
					{
					}
					goto IL_184;
				}
				throw new Exception("Cannot enqueue workitem");
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000A370 File Offset: 0x00008770
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000A438 File Offset: 0x00008838
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (!this.emitting)
			{
				if (this._currentlyFilling >= 0)
				{
					WorkItem wi = this._pool[this._currentlyFilling];
					this._DeflateOne(wi);
					this._currentlyFilling = -1;
				}
				if (lastInput)
				{
					this.EmitPendingBuffers(true, false);
					this._FlushFinish();
				}
				else
				{
					this.EmitPendingBuffers(false, false);
				}
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000A4BC File Offset: 0x000088BC
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (!this._handlingException)
			{
				this._Flush(false);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000A50C File Offset: 0x0000890C
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (!this._handlingException)
			{
				if (!this._isClosed)
				{
					this._Flush(true);
					if (!this._leaveOpen)
					{
						this._outStream.Close();
					}
					this._isClosed = true;
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000A587 File Offset: 0x00008987
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000A59E File Offset: 0x0000899E
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000A5A8 File Offset: 0x000089A8
		public void Reset(Stream stream)
		{
			if (this._firstWriteDone)
			{
				this._toWrite.Clear();
				this._toFill.Clear();
				foreach (WorkItem workItem in this._pool)
				{
					this._toFill.Enqueue(workItem.index);
					workItem.ordinal = -1;
				}
				this._firstWriteDone = false;
				this._totalBytesProcessed = 0L;
				this._runningCrc = new CRC32();
				this._isClosed = false;
				this._currentlyFilling = -1;
				this._lastFilled = -1;
				this._lastWritten = -1;
				this._latestCompressed = -1;
				this._outStream = stream;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000A684 File Offset: 0x00008A84
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (!this.emitting)
			{
				this.emitting = true;
				if (doAll || mustWait)
				{
					this._newlyCompressedBlob.WaitOne();
				}
				do
				{
					int num = -1;
					int num2 = (!doAll) ? ((!mustWait) ? 0 : -1) : 200;
					int num3 = -1;
					for (;;)
					{
						if (Monitor.TryEnter(this._toWrite, num2))
						{
							num3 = -1;
							try
							{
								if (this._toWrite.Count > 0)
								{
									num3 = this._toWrite.Dequeue();
								}
							}
							finally
							{
								Monitor.Exit(this._toWrite);
							}
							if (num3 >= 0)
							{
								WorkItem workItem = this._pool[num3];
								if (workItem.ordinal != this._lastWritten + 1)
								{
									object toWrite = this._toWrite;
									lock (toWrite)
									{
										this._toWrite.Enqueue(num3);
									}
									if (num == num3)
									{
										this._newlyCompressedBlob.WaitOne();
										num = -1;
									}
									else if (num == -1)
									{
										num = num3;
									}
								}
								else
								{
									num = -1;
									this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
									this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
									this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
									workItem.inputBytesAvailable = 0;
									this._lastWritten = workItem.ordinal;
									this._toFill.Enqueue(workItem.index);
									if (num2 == -1)
									{
										num2 = 0;
									}
								}
							}
						}
						else
						{
							num3 = -1;
						}
						IL_193:
						if (num3 < 0)
						{
							break;
						}
						continue;
						goto IL_193;
					}
				}
				while (doAll && this._lastWritten != this._latestCompressed);
				this.emitting = false;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000A868 File Offset: 0x00008C68
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object latestLock = this._latestLock;
				lock (latestLock)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				object toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object eLock = this._eLock;
				lock (eLock)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000A994 File Offset: 0x00008D94
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000AA18 File Offset: 0x00008E18
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000AA94 File Offset: 0x00008E94
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000AAAC File Offset: 0x00008EAC
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000AAC4 File Offset: 0x00008EC4
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000AAE4 File Offset: 0x00008EE4
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000AAEC File Offset: 0x00008EEC
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x0000AB0C File Offset: 0x00008F0C
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000AB14 File Offset: 0x00008F14
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000AB1C File Offset: 0x00008F1C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000AB24 File Offset: 0x00008F24
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x02000014 RID: 20
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x04000105 RID: 261
			None = 0u,
			// Token: 0x04000106 RID: 262
			NotUsed1 = 1u,
			// Token: 0x04000107 RID: 263
			EmitLock = 2u,
			// Token: 0x04000108 RID: 264
			EmitEnter = 4u,
			// Token: 0x04000109 RID: 265
			EmitBegin = 8u,
			// Token: 0x0400010A RID: 266
			EmitDone = 16u,
			// Token: 0x0400010B RID: 267
			EmitSkip = 32u,
			// Token: 0x0400010C RID: 268
			EmitAll = 58u,
			// Token: 0x0400010D RID: 269
			Flush = 64u,
			// Token: 0x0400010E RID: 270
			Lifecycle = 128u,
			// Token: 0x0400010F RID: 271
			Session = 256u,
			// Token: 0x04000110 RID: 272
			Synch = 512u,
			// Token: 0x04000111 RID: 273
			Instance = 1024u,
			// Token: 0x04000112 RID: 274
			Compress = 2048u,
			// Token: 0x04000113 RID: 275
			Write = 4096u,
			// Token: 0x04000114 RID: 276
			WriteEnter = 8192u,
			// Token: 0x04000115 RID: 277
			WriteTake = 16384u,
			// Token: 0x04000116 RID: 278
			All = 4294967295u
		}
	}
}
