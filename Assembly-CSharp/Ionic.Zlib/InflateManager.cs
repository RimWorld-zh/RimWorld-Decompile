namespace Ionic.Zlib
{
	internal sealed class InflateManager
	{
		private enum InflateManagerMode
		{
			METHOD,
			FLAG,
			DICT4,
			DICT3,
			DICT2,
			DICT1,
			DICT0,
			BLOCKS,
			CHECK4,
			CHECK3,
			CHECK2,
			CHECK1,
			DONE,
			BAD
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
			0,
			0,
			255,
			255
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
			this._codec.Message = null;
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
			this._codec.Message = null;
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
			while (true)
			{
				switch (this.mode)
				{
				case InflateManagerMode.METHOD:
					break;
				case InflateManagerMode.FLAG:
					goto IL_016a;
				case InflateManagerMode.DICT4:
					goto IL_0221;
				case InflateManagerMode.DICT3:
					goto IL_029e;
				case InflateManagerMode.DICT2:
					goto IL_031f;
				case InflateManagerMode.DICT1:
					goto IL_039f;
				case InflateManagerMode.DICT0:
					this.mode = InflateManagerMode.BAD;
					this._codec.Message = "need dictionary";
					this.marker = 0;
					return -2;
				case InflateManagerMode.BLOCKS:
					goto IL_044d;
				case InflateManagerMode.CHECK4:
					goto IL_04bb;
				case InflateManagerMode.CHECK3:
					goto IL_0539;
				case InflateManagerMode.CHECK2:
					goto IL_05bb;
				case InflateManagerMode.CHECK1:
					goto IL_063c;
				case InflateManagerMode.DONE:
					return 1;
				case InflateManagerMode.BAD:
					throw new ZlibException(string.Format("Bad state ({0})", this._codec.Message));
				default:
					throw new ZlibException("Stream error.");
				}
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				if (((this.method = this._codec.InputBuffer[this._codec.NextIn++]) & 0xF) != 8)
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
				continue;
				IL_063c:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & 0xFF);
				if (this.computedCheck == this.expectedCheck)
					break;
				this.mode = InflateManagerMode.BAD;
				this._codec.Message = "incorrect data check";
				this.marker = 5;
				continue;
				IL_044d:
				num2 = this.blocks.Process(num2);
				switch (num2)
				{
				case -3:
					break;
				case 0:
					num2 = num;
					goto default;
				default:
					if (num2 != 1)
					{
						return num2;
					}
					goto IL_0487;
				}
				this.mode = InflateManagerMode.BAD;
				this.marker = 0;
				continue;
				IL_039f:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & 0xFF);
				this._codec._Adler32 = this.expectedCheck;
				this.mode = InflateManagerMode.DICT0;
				return 2;
				IL_016a:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				int num3 = this._codec.InputBuffer[this._codec.NextIn++] & 0xFF;
				if (((this.method << 8) + num3) % 31 != 0)
				{
					this.mode = InflateManagerMode.BAD;
					this._codec.Message = "incorrect header check";
					this.marker = 5;
				}
				else
				{
					this.mode = (InflateManagerMode)(((num3 & 0x20) != 0) ? 2 : 7);
				}
				continue;
				IL_05bb:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
				this.mode = InflateManagerMode.CHECK1;
				continue;
				IL_0221:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck = (uint)(this._codec.InputBuffer[this._codec.NextIn++] << 24 & 4278190080u);
				this.mode = InflateManagerMode.DICT3;
				continue;
				IL_031f:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
				this.mode = InflateManagerMode.DICT1;
				continue;
				IL_0539:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
				this.mode = InflateManagerMode.CHECK2;
				continue;
				IL_029e:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
				this.mode = InflateManagerMode.DICT2;
				continue;
				IL_0487:
				num2 = num;
				this.computedCheck = this.blocks.Reset();
				if (!this.HandleRfc1950HeaderBytes)
				{
					this.mode = InflateManagerMode.DONE;
					return 1;
				}
				this.mode = InflateManagerMode.CHECK4;
				continue;
				IL_04bb:
				if (this._codec.AvailableBytesIn == 0)
				{
					return num2;
				}
				num2 = num;
				this._codec.AvailableBytesIn--;
				this._codec.TotalBytesIn += 1L;
				this.expectedCheck = (uint)(this._codec.InputBuffer[this._codec.NextIn++] << 24 & 4278190080u);
				this.mode = InflateManagerMode.CHECK3;
			}
			this.mode = InflateManagerMode.DONE;
			return 1;
		}

		internal int SetDictionary(byte[] dictionary)
		{
			int start = 0;
			int num = dictionary.Length;
			if (this.mode != InflateManagerMode.DICT0)
			{
				throw new ZlibException("Stream error.");
			}
			if (Adler.Adler32(1u, dictionary, 0, dictionary.Length) != this._codec._Adler32)
			{
				return -3;
			}
			this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
			if (num >= 1 << this.wbits)
			{
				num = (1 << this.wbits) - 1;
				start = dictionary.Length - num;
			}
			this.blocks.SetDictionary(dictionary, start, num);
			this.mode = InflateManagerMode.BLOCKS;
			return 0;
		}

		internal int Sync()
		{
			if (this.mode != InflateManagerMode.BAD)
			{
				this.mode = InflateManagerMode.BAD;
				this.marker = 0;
			}
			int num;
			if ((num = this._codec.AvailableBytesIn) == 0)
			{
				return -5;
			}
			int num2 = this._codec.NextIn;
			int num3 = this.marker;
			while (num != 0 && num3 < 4)
			{
				num3 = ((this._codec.InputBuffer[num2] != InflateManager.mark[num3]) ? ((this._codec.InputBuffer[num2] == 0) ? (4 - num3) : 0) : (num3 + 1));
				num2++;
				num--;
			}
			this._codec.TotalBytesIn += num2 - this._codec.NextIn;
			this._codec.NextIn = num2;
			this._codec.AvailableBytesIn = num;
			this.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long totalBytesIn = this._codec.TotalBytesIn;
			long totalBytesOut = this._codec.TotalBytesOut;
			this.Reset();
			this._codec.TotalBytesIn = totalBytesIn;
			this._codec.TotalBytesOut = totalBytesOut;
			this.mode = InflateManagerMode.BLOCKS;
			return 0;
		}

		internal int SyncPoint(ZlibCodec z)
		{
			return this.blocks.SyncPoint();
		}
	}
}
