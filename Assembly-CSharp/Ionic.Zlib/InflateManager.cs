namespace Ionic.Zlib
{
	internal sealed class InflateManager
	{
		private enum InflateManagerMode
		{
			METHOD = 0,
			FLAG = 1,
			DICT4 = 2,
			DICT3 = 3,
			DICT2 = 4,
			DICT1 = 5,
			DICT0 = 6,
			BLOCKS = 7,
			CHECK4 = 8,
			CHECK3 = 9,
			CHECK2 = 10,
			CHECK1 = 11,
			DONE = 12,
			BAD = 13
		}

		private const int PRESET_DICT = 32;

		private const int Z_DEFLATED = 8;

		private InflateManagerMode mode;

		internal ZlibCodec _codec;

		internal int method;

		internal uint computedCheck;

		internal uint expectedCheck;

		internal int marker;

		private bool _handleRfc1950HeaderBytes = true;

		internal int wbits;

		internal InflateBlocks blocks;

		private static readonly byte[] mark = new byte[4]
		{
			(byte)0,
			(byte)0,
			(byte)255,
			(byte)255
		};

		internal bool HandleRfc1950HeaderBytes
		{
			get
			{
				return this._handleRfc1950HeaderBytes;
			}
			set
			{
				this._handleRfc1950HeaderBytes = value;
			}
		}

		public InflateManager()
		{
		}

		public InflateManager(bool expectRfc1950HeaderBytes)
		{
			this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
		}

		internal int Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = (string)null;
			this.mode = (InflateManagerMode)((!this.HandleRfc1950HeaderBytes) ? 7 : 0);
			this.blocks.Reset();
			return 0;
		}

		internal int End()
		{
			if (this.blocks != null)
			{
				this.blocks.Free();
			}
			this.blocks = null;
			return 0;
		}

		internal int Initialize(ZlibCodec codec, int w)
		{
			this._codec = codec;
			this._codec.Message = (string)null;
			this.blocks = null;
			if (w >= 8 && w <= 15)
			{
				this.wbits = w;
				this.blocks = new InflateBlocks(codec, (!this.HandleRfc1950HeaderBytes) ? null : this, 1 << w);
				this.Reset();
				return 0;
			}
			this.End();
			throw new ZlibException("Bad window size.");
		}

		internal int Inflate(FlushType flush)
		{
			if (this._codec.InputBuffer == null)
			{
				throw new ZlibException("InputBuffer is null. ");
			}
			int num = 0;
			int num2 = -5;
			int result;
			while (true)
			{
				switch (this.mode)
				{
				case InflateManagerMode.METHOD:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer = this._codec.InputBuffer;
					ZlibCodec codec = this._codec;
					int nextIn = codec.NextIn;
					int num3 = nextIn;
					codec.NextIn = nextIn + 1;
					if (((this.method = inputBuffer[num3]) & 15) != 8)
					{
						this.mode = InflateManagerMode.BAD;
						this._codec.Message = string.Format("unknown compression method (0x{0:X2})", this.method);
						this.marker = 5;
					}
					else if ((this.method >> 4) + 8 > this.wbits)
					{
						this.mode = InflateManagerMode.BAD;
						this._codec.Message = string.Format("invalid window size ({0})", (this.method >> 4) + 8);
						this.marker = 5;
					}
					else
					{
						this.mode = InflateManagerMode.FLAG;
					}
					break;
				}
				case InflateManagerMode.FLAG:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer2 = this._codec.InputBuffer;
					ZlibCodec codec2 = this._codec;
					int nextIn2 = codec2.NextIn;
					int num3 = nextIn2;
					codec2.NextIn = nextIn2 + 1;
					int num4 = inputBuffer2[num3] & 255;
					if (((this.method << 8) + num4) % 31 != 0)
					{
						this.mode = InflateManagerMode.BAD;
						this._codec.Message = "incorrect header check";
						this.marker = 5;
					}
					else
					{
						this.mode = (InflateManagerMode)(((num4 & 32) != 0) ? 2 : 7);
					}
					break;
				}
				case InflateManagerMode.DICT4:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer3 = this._codec.InputBuffer;
					ZlibCodec codec3 = this._codec;
					int nextIn3 = codec3.NextIn;
					int num3 = nextIn3;
					codec3.NextIn = nextIn3 + 1;
					this.expectedCheck = (uint)(inputBuffer3[num3] << 24 & 4278190080u);
					this.mode = InflateManagerMode.DICT3;
					break;
				}
				case InflateManagerMode.DICT3:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num5 = this.expectedCheck;
					byte[] inputBuffer4 = this._codec.InputBuffer;
					ZlibCodec codec4 = this._codec;
					int nextIn4 = codec4.NextIn;
					int num3 = nextIn4;
					codec4.NextIn = nextIn4 + 1;
					this.expectedCheck = (uint)((int)num5 + (inputBuffer4[num3] << 16 & 16711680));
					this.mode = InflateManagerMode.DICT2;
					break;
				}
				case InflateManagerMode.DICT2:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num6 = this.expectedCheck;
					byte[] inputBuffer5 = this._codec.InputBuffer;
					ZlibCodec codec5 = this._codec;
					int nextIn5 = codec5.NextIn;
					int num3 = nextIn5;
					codec5.NextIn = nextIn5 + 1;
					this.expectedCheck = (uint)((int)num6 + (inputBuffer5[num3] << 8 & 65280));
					this.mode = InflateManagerMode.DICT1;
					break;
				}
				case InflateManagerMode.DICT1:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
					}
					else
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						uint num7 = this.expectedCheck;
						byte[] inputBuffer6 = this._codec.InputBuffer;
						ZlibCodec codec6 = this._codec;
						int nextIn6 = codec6.NextIn;
						int num3 = nextIn6;
						codec6.NextIn = nextIn6 + 1;
						this.expectedCheck = (uint)((int)num7 + (inputBuffer6[num3] & 255));
						this._codec._Adler32 = this.expectedCheck;
						this.mode = InflateManagerMode.DICT0;
						result = 2;
					}
					goto end_IL_0021;
				}
				case InflateManagerMode.DICT0:
				{
					this.mode = InflateManagerMode.BAD;
					this._codec.Message = "need dictionary";
					this.marker = 0;
					result = -2;
					goto end_IL_0021;
				}
				case InflateManagerMode.BLOCKS:
				{
					num2 = this.blocks.Process(num2);
					switch (num2)
					{
					case -3:
					{
						this.mode = InflateManagerMode.BAD;
						this.marker = 0;
						continue;
					}
					case 0:
					{
						num2 = num;
						break;
					}
					}
					if (num2 != 1)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this.computedCheck = this.blocks.Reset();
					if (!this.HandleRfc1950HeaderBytes)
					{
						this.mode = InflateManagerMode.DONE;
						result = 1;
						goto end_IL_0021;
					}
					this.mode = InflateManagerMode.CHECK4;
					break;
				}
				case InflateManagerMode.CHECK4:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					byte[] inputBuffer7 = this._codec.InputBuffer;
					ZlibCodec codec7 = this._codec;
					int nextIn7 = codec7.NextIn;
					int num3 = nextIn7;
					codec7.NextIn = nextIn7 + 1;
					this.expectedCheck = (uint)(inputBuffer7[num3] << 24 & 4278190080u);
					this.mode = InflateManagerMode.CHECK3;
					break;
				}
				case InflateManagerMode.CHECK3:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num8 = this.expectedCheck;
					byte[] inputBuffer8 = this._codec.InputBuffer;
					ZlibCodec codec8 = this._codec;
					int nextIn8 = codec8.NextIn;
					int num3 = nextIn8;
					codec8.NextIn = nextIn8 + 1;
					this.expectedCheck = (uint)((int)num8 + (inputBuffer8[num3] << 16 & 16711680));
					this.mode = InflateManagerMode.CHECK2;
					break;
				}
				case InflateManagerMode.CHECK2:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
						goto end_IL_0021;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					uint num9 = this.expectedCheck;
					byte[] inputBuffer9 = this._codec.InputBuffer;
					ZlibCodec codec9 = this._codec;
					int nextIn9 = codec9.NextIn;
					int num3 = nextIn9;
					codec9.NextIn = nextIn9 + 1;
					this.expectedCheck = (uint)((int)num9 + (inputBuffer9[num3] << 8 & 65280));
					this.mode = InflateManagerMode.CHECK1;
					break;
				}
				case InflateManagerMode.CHECK1:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						result = num2;
					}
					else
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						uint num10 = this.expectedCheck;
						byte[] inputBuffer10 = this._codec.InputBuffer;
						ZlibCodec codec10 = this._codec;
						int nextIn10 = codec10.NextIn;
						int num3 = nextIn10;
						codec10.NextIn = nextIn10 + 1;
						this.expectedCheck = (uint)((int)num10 + (inputBuffer10[num3] & 255));
						if (this.computedCheck != this.expectedCheck)
						{
							this.mode = InflateManagerMode.BAD;
							this._codec.Message = "incorrect data check";
							this.marker = 5;
							continue;
						}
						this.mode = InflateManagerMode.DONE;
						result = 1;
					}
					goto end_IL_0021;
				}
				case InflateManagerMode.DONE:
				{
					result = 1;
					goto end_IL_0021;
				}
				case InflateManagerMode.BAD:
				{
					throw new ZlibException(string.Format("Bad state ({0})", this._codec.Message));
				}
				default:
				{
					throw new ZlibException("Stream error.");
				}
				}
				continue;
				continue;
				end_IL_0021:
				break;
			}
			return result;
		}

		internal int SetDictionary(byte[] dictionary)
		{
			int start = 0;
			int num = dictionary.Length;
			if (this.mode != InflateManagerMode.DICT0)
			{
				throw new ZlibException("Stream error.");
			}
			int result;
			if (Adler.Adler32(1u, dictionary, 0, dictionary.Length) != this._codec._Adler32)
			{
				result = -3;
			}
			else
			{
				this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
				if (num >= 1 << this.wbits)
				{
					num = (1 << this.wbits) - 1;
					start = dictionary.Length - num;
				}
				this.blocks.SetDictionary(dictionary, start, num);
				this.mode = InflateManagerMode.BLOCKS;
				result = 0;
			}
			return result;
		}

		internal int Sync()
		{
			if (this.mode != InflateManagerMode.BAD)
			{
				this.mode = InflateManagerMode.BAD;
				this.marker = 0;
			}
			int num;
			int result;
			if ((num = this._codec.AvailableBytesIn) == 0)
			{
				result = -5;
			}
			else
			{
				int num2 = this._codec.NextIn;
				int num3 = this.marker;
				while (num != 0 && num3 < 4)
				{
					num3 = ((this._codec.InputBuffer[num2] != InflateManager.mark[num3]) ? ((this._codec.InputBuffer[num2] == 0) ? (4 - num3) : 0) : (num3 + 1));
					num2++;
					num--;
				}
				this._codec.TotalBytesIn += (long)(num2 - this._codec.NextIn);
				this._codec.NextIn = num2;
				this._codec.AvailableBytesIn = num;
				this.marker = num3;
				if (num3 != 4)
				{
					result = -3;
				}
				else
				{
					long totalBytesIn = this._codec.TotalBytesIn;
					long totalBytesOut = this._codec.TotalBytesOut;
					this.Reset();
					this._codec.TotalBytesIn = totalBytesIn;
					this._codec.TotalBytesOut = totalBytesOut;
					this.mode = InflateManagerMode.BLOCKS;
					result = 0;
				}
			}
			return result;
		}

		internal int SyncPoint(ZlibCodec z)
		{
			return this.blocks.SyncPoint();
		}
	}
}
