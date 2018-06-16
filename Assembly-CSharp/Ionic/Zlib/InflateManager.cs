using System;

namespace Ionic.Zlib
{
	// Token: 0x02000010 RID: 16
	internal sealed class InflateManager
	{
		// Token: 0x060000AD RID: 173 RVA: 0x000093D2 File Offset: 0x000077D2
		public InflateManager()
		{
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000093E2 File Offset: 0x000077E2
		public InflateManager(bool expectRfc1950HeaderBytes)
		{
			this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000093FC File Offset: 0x000077FC
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00009417 File Offset: 0x00007817
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

		// Token: 0x060000B1 RID: 177 RVA: 0x00009424 File Offset: 0x00007824
		internal int Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = null;
			this.mode = ((!this.HandleRfc1950HeaderBytes) ? InflateManager.InflateManagerMode.BLOCKS : InflateManager.InflateManagerMode.METHOD);
			this.blocks.Reset();
			return 0;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00009488 File Offset: 0x00007888
		internal int End()
		{
			if (this.blocks != null)
			{
				this.blocks.Free();
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000094BC File Offset: 0x000078BC
		internal int Initialize(ZlibCodec codec, int w)
		{
			this._codec = codec;
			this._codec.Message = null;
			this.blocks = null;
			if (w < 8 || w > 15)
			{
				this.End();
				throw new ZlibException("Bad window size.");
			}
			this.wbits = w;
			this.blocks = new InflateBlocks(codec, (!this.HandleRfc1950HeaderBytes) ? null : this, 1 << w);
			this.Reset();
			return 0;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00009540 File Offset: 0x00007940
		internal int Inflate(FlushType flush)
		{
			if (this._codec.InputBuffer == null)
			{
				throw new ZlibException("InputBuffer is null. ");
			}
			int num = 0;
			int num2 = -5;
			for (;;)
			{
				switch (this.mode)
				{
				case InflateManager.InflateManagerMode.METHOD:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_3;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					if (((this.method = (int)this._codec.InputBuffer[this._codec.NextIn++]) & 15) != 8)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("unknown compression method (0x{0:X2})", this.method);
						this.marker = 5;
						continue;
					}
					if ((this.method >> 4) + 8 > this.wbits)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("invalid window size ({0})", (this.method >> 4) + 8);
						this.marker = 5;
						continue;
					}
					this.mode = InflateManager.InflateManagerMode.FLAG;
					continue;
				case InflateManager.InflateManagerMode.FLAG:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_6;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					int num3 = (int)(this._codec.InputBuffer[this._codec.NextIn++] & byte.MaxValue);
					if (((this.method << 8) + num3) % 31 != 0)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect header check";
						this.marker = 5;
						continue;
					}
					this.mode = (((num3 & 32) != 0) ? InflateManager.InflateManagerMode.DICT4 : InflateManager.InflateManagerMode.BLOCKS);
					continue;
				}
				case InflateManager.InflateManagerMode.DICT4:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_9;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck = (uint)((long)((long)this._codec.InputBuffer[this._codec.NextIn++] << 24) & (long)((ulong)-16777216));
					this.mode = InflateManager.InflateManagerMode.DICT3;
					continue;
				case InflateManager.InflateManagerMode.DICT3:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_10;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
					this.mode = InflateManager.InflateManagerMode.DICT2;
					continue;
				case InflateManager.InflateManagerMode.DICT2:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_11;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
					this.mode = InflateManager.InflateManagerMode.DICT1;
					continue;
				case InflateManager.InflateManagerMode.DICT1:
					goto IL_3C3;
				case InflateManager.InflateManagerMode.DICT0:
					goto IL_45B;
				case InflateManager.InflateManagerMode.BLOCKS:
					num2 = this.blocks.Process(num2);
					if (num2 == -3)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this.marker = 0;
						continue;
					}
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 != 1)
					{
						goto Block_15;
					}
					num2 = num;
					this.computedCheck = this.blocks.Reset();
					if (!this.HandleRfc1950HeaderBytes)
					{
						goto Block_16;
					}
					this.mode = InflateManager.InflateManagerMode.CHECK4;
					continue;
				case InflateManager.InflateManagerMode.CHECK4:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_17;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck = (uint)((long)((long)this._codec.InputBuffer[this._codec.NextIn++] << 24) & (long)((ulong)-16777216));
					this.mode = InflateManager.InflateManagerMode.CHECK3;
					continue;
				case InflateManager.InflateManagerMode.CHECK3:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_18;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
					this.mode = InflateManager.InflateManagerMode.CHECK2;
					continue;
				case InflateManager.InflateManagerMode.CHECK2:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_19;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
					this.mode = InflateManager.InflateManagerMode.CHECK1;
					continue;
				case InflateManager.InflateManagerMode.CHECK1:
					if (this._codec.AvailableBytesIn == 0)
					{
						goto Block_20;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & byte.MaxValue);
					if (this.computedCheck != this.expectedCheck)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect data check";
						this.marker = 5;
						continue;
					}
					goto IL_740;
				case InflateManager.InflateManagerMode.DONE:
					goto IL_750;
				case InflateManager.InflateManagerMode.BAD:
					goto IL_758;
				}
				break;
			}
			throw new ZlibException("Stream error.");
			Block_3:
			return num2;
			Block_6:
			return num2;
			Block_9:
			return num2;
			Block_10:
			return num2;
			Block_11:
			return num2;
			IL_3C3:
			if (this._codec.AvailableBytesIn == 0)
			{
				return num2;
			}
			this._codec.AvailableBytesIn--;
			this._codec.TotalBytesIn += 1L;
			this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & byte.MaxValue);
			this._codec._Adler32 = this.expectedCheck;
			this.mode = InflateManager.InflateManagerMode.DICT0;
			return 2;
			IL_45B:
			this.mode = InflateManager.InflateManagerMode.BAD;
			this._codec.Message = "need dictionary";
			this.marker = 0;
			return -2;
			Block_15:
			return num2;
			Block_16:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			Block_17:
			return num2;
			Block_18:
			return num2;
			Block_19:
			return num2;
			Block_20:
			return num2;
			IL_740:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_750:
			return 1;
			IL_758:
			throw new ZlibException(string.Format("Bad state ({0})", this._codec.Message));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00009CD4 File Offset: 0x000080D4
		internal int SetDictionary(byte[] dictionary)
		{
			int start = 0;
			int num = dictionary.Length;
			if (this.mode != InflateManager.InflateManagerMode.DICT0)
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
				this.mode = InflateManager.InflateManagerMode.BLOCKS;
				result = 0;
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00009D7C File Offset: 0x0000817C
		internal int Sync()
		{
			if (this.mode != InflateManager.InflateManagerMode.BAD)
			{
				this.mode = InflateManager.InflateManagerMode.BAD;
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
					if (this._codec.InputBuffer[num2] == InflateManager.mark[num3])
					{
						num3++;
					}
					else if (this._codec.InputBuffer[num2] != 0)
					{
						num3 = 0;
					}
					else
					{
						num3 = 4 - num3;
					}
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
					this.mode = InflateManager.InflateManagerMode.BLOCKS;
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00009ED4 File Offset: 0x000082D4
		internal int SyncPoint(ZlibCodec z)
		{
			return this.blocks.SyncPoint();
		}

		// Token: 0x040000C5 RID: 197
		private const int PRESET_DICT = 32;

		// Token: 0x040000C6 RID: 198
		private const int Z_DEFLATED = 8;

		// Token: 0x040000C7 RID: 199
		private InflateManager.InflateManagerMode mode;

		// Token: 0x040000C8 RID: 200
		internal ZlibCodec _codec;

		// Token: 0x040000C9 RID: 201
		internal int method;

		// Token: 0x040000CA RID: 202
		internal uint computedCheck;

		// Token: 0x040000CB RID: 203
		internal uint expectedCheck;

		// Token: 0x040000CC RID: 204
		internal int marker;

		// Token: 0x040000CD RID: 205
		private bool _handleRfc1950HeaderBytes = true;

		// Token: 0x040000CE RID: 206
		internal int wbits;

		// Token: 0x040000CF RID: 207
		internal InflateBlocks blocks;

		// Token: 0x040000D0 RID: 208
		private static readonly byte[] mark = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x02000011 RID: 17
		private enum InflateManagerMode
		{
			// Token: 0x040000D2 RID: 210
			METHOD,
			// Token: 0x040000D3 RID: 211
			FLAG,
			// Token: 0x040000D4 RID: 212
			DICT4,
			// Token: 0x040000D5 RID: 213
			DICT3,
			// Token: 0x040000D6 RID: 214
			DICT2,
			// Token: 0x040000D7 RID: 215
			DICT1,
			// Token: 0x040000D8 RID: 216
			DICT0,
			// Token: 0x040000D9 RID: 217
			BLOCKS,
			// Token: 0x040000DA RID: 218
			CHECK4,
			// Token: 0x040000DB RID: 219
			CHECK3,
			// Token: 0x040000DC RID: 220
			CHECK2,
			// Token: 0x040000DD RID: 221
			CHECK1,
			// Token: 0x040000DE RID: 222
			DONE,
			// Token: 0x040000DF RID: 223
			BAD
		}
	}
}
