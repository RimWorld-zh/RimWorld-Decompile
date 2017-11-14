using Ionic.Crc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ionic.Zlib
{
	public class ParallelDeflateOutputStream : Stream
	{
		[Flags]
		private enum TraceBits : uint
		{
			None = 0u,
			NotUsed1 = 1u,
			EmitLock = 2u,
			EmitEnter = 4u,
			EmitBegin = 8u,
			EmitDone = 0x10,
			EmitSkip = 0x20,
			EmitAll = 58u,
			Flush = 0x40,
			Lifecycle = 0x80,
			Session = 0x100,
			Synch = 0x200,
			Instance = 0x400,
			Compress = 0x800,
			Write = 0x1000,
			WriteEnter = 0x2000,
			WriteTake = 0x4000,
			All = 0xFFFFFFFF
		}

		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		private static readonly int BufferPairsPerCore = 4;

		private List<WorkItem> _pool;

		private bool _leaveOpen;

		private bool emitting;

		private Stream _outStream;

		private int _maxBufferPairs;

		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		private AutoResetEvent _newlyCompressedBlob;

		private object _outputLock = new object();

		private bool _isClosed;

		private bool _firstWriteDone;

		private int _currentlyFilling;

		private int _lastFilled;

		private int _lastWritten;

		private int _latestCompressed;

		private int _Crc32;

		private CRC32 _runningCrc;

		private object _latestLock = new object();

		private Queue<int> _toWrite;

		private Queue<int> _toFill;

		private long _totalBytesProcessed;

		private CompressionLevel _compressLevel;

		private volatile Exception _pendingException;

		private bool _handlingException;

		private object _eLock = new object();

		private TraceBits _DesiredTrace = TraceBits.EmitLock | TraceBits.EmitEnter | TraceBits.EmitBegin | TraceBits.EmitDone | TraceBits.EmitSkip | TraceBits.Session | TraceBits.Compress | TraceBits.WriteEnter | TraceBits.WriteTake;

		public CompressionStrategy Strategy
		{
			get;
			private set;
		}

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

		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

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

		public ParallelDeflateOutputStream(Stream stream)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level)
			: this(stream, level, CompressionStrategy.Default, false)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen)
			: this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int val = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			val = Math.Min(val, this._maxBufferPairs);
			for (int i = 0; i < val; i++)
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
				while (true)
				{
					this.EmitPendingBuffers(false, mustWait);
					mustWait = false;
					int num = -1;
					if (this._currentlyFilling >= 0)
					{
						num = this._currentlyFilling;
					}
					else
					{
						if (this._toFill.Count == 0)
						{
							mustWait = true;
							goto IL_0173;
						}
						num = this._toFill.Dequeue();
						this._lastFilled++;
					}
					WorkItem workItem = this._pool[num];
					int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable <= count) ? (workItem.buffer.Length - workItem.inputBytesAvailable) : count;
					workItem.ordinal = this._lastFilled;
					Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
					count -= num2;
					offset += num2;
					workItem.inputBytesAvailable += num2;
					if (workItem.inputBytesAvailable == workItem.buffer.Length)
					{
						if (ThreadPool.QueueUserWorkItem(this._DeflateOne, workItem))
						{
							this._currentlyFilling = -1;
							goto IL_016c;
						}
						break;
					}
					this._currentlyFilling = num;
					goto IL_016c;
					IL_016c:
					if (count <= 0)
						goto IL_0173;
					goto IL_0173;
					IL_0173:
					if (count <= 0)
						return;
				}
				throw new Exception("Cannot enqueue workitem");
			}
		}

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

		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (!this._handlingException && !this._isClosed)
			{
				this._Flush(true);
				if (!this._leaveOpen)
				{
					this._outStream.Close();
				}
				this._isClosed = true;
			}
		}

		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		public void Reset(Stream stream)
		{
			if (this._firstWriteDone)
			{
				this._toWrite.Clear();
				this._toFill.Clear();
				foreach (WorkItem item in this._pool)
				{
					this._toFill.Enqueue(item.index);
					item.ordinal = -1;
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

		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (!this.emitting)
			{
				this.emitting = true;
				if (doAll || mustWait)
				{
					this._newlyCompressedBlob.WaitOne();
				}
				while (true)
				{
					int num = -1;
					int num2 = (!doAll) ? (mustWait ? (-1) : 0) : 200;
					int num3 = -1;
					while (true)
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
									Monitor.Enter(toWrite);
									try
									{
										this._toWrite.Enqueue(num3);
									}
									finally
									{
										Monitor.Exit(toWrite);
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
									this._totalBytesProcessed += workItem.inputBytesAvailable;
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
						if (num3 < 0)
							break;
					}
					if (!doAll)
						break;
					if (this._lastWritten == this._latestCompressed)
						break;
				}
				this.emitting = false;
			}
		}

		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 cRC = new CRC32();
				cRC.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = cRC.Crc32Result;
				object latestLock = this._latestLock;
				Monitor.Enter(latestLock);
				try
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				finally
				{
					Monitor.Exit(latestLock);
				}
				object toWrite = this._toWrite;
				Monitor.Enter(toWrite);
				try
				{
					this._toWrite.Enqueue(workItem.index);
				}
				finally
				{
					Monitor.Exit(toWrite);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object eLock = this._eLock;
				Monitor.Enter(eLock);
				try
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
				finally
				{
					Monitor.Exit(eLock);
				}
			}
		}

		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			while (true)
			{
				compressor.Deflate(FlushType.None);
				if (compressor.AvailableBytesIn <= 0 && compressor.AvailableBytesOut != 0)
					break;
			}
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		[Conditional("Trace")]
		private void TraceOutput(TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != 0)
			{
				object outputLock = this._outputLock;
				Monitor.Enter(outputLock);
				try
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = (ConsoleColor)(hashCode % 8 + 8);
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
				finally
				{
					Monitor.Exit(outputLock);
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
	}
}
