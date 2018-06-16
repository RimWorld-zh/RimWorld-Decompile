using System;

namespace Ionic.Zlib
{
	// Token: 0x0200000C RID: 12
	internal sealed class InflateBlocks
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00006C18 File Offset: 0x00005018
		internal InflateBlocks(ZlibCodec codec, object checkfn, int w)
		{
			this._codec = codec;
			this.hufts = new int[4320];
			this.window = new byte[w];
			this.end = w;
			this.checkfn = checkfn;
			this.mode = InflateBlocks.InflateBlockMode.TYPE;
			this.Reset();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006C9C File Offset: 0x0000509C
		internal uint Reset()
		{
			uint result = this.check;
			this.mode = InflateBlocks.InflateBlockMode.TYPE;
			this.bitk = 0;
			this.bitb = 0;
			this.readAt = (this.writeAt = 0);
			if (this.checkfn != null)
			{
				this._codec._Adler32 = (this.check = Adler.Adler32(0u, null, 0, 0));
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006D08 File Offset: 0x00005108
		internal int Process(int r)
		{
			int num = this._codec.NextIn;
			int num2 = this._codec.AvailableBytesIn;
			int num3 = this.bitb;
			int i = this.bitk;
			int num4 = this.writeAt;
			int num5 = (num4 >= this.readAt) ? (this.end - num4) : (this.readAt - num4 - 1);
			int num6;
			for (;;)
			{
				switch (this.mode)
				{
				case InflateBlocks.InflateBlockMode.TYPE:
					while (i < 3)
					{
						if (num2 == 0)
						{
							goto IL_AD;
						}
						r = 0;
						num2--;
						num3 |= (int)(this._codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = (num3 & 7);
					this.last = (num6 & 1);
					switch ((uint)num6 >> 1)
					{
					case 0u:
						num3 >>= 3;
						i -= 3;
						num6 = (i & 7);
						num3 >>= num6;
						i -= num6;
						this.mode = InflateBlocks.InflateBlockMode.LENS;
						break;
					case 1u:
					{
						int[] array = new int[1];
						int[] array2 = new int[1];
						int[][] array3 = new int[1][];
						int[][] array4 = new int[1][];
						InfTree.inflate_trees_fixed(array, array2, array3, array4, this._codec);
						this.codes.Init(array[0], array2[0], array3[0], 0, array4[0], 0);
						num3 >>= 3;
						i -= 3;
						this.mode = InflateBlocks.InflateBlockMode.CODES;
						break;
					}
					case 2u:
						num3 >>= 3;
						i -= 3;
						this.mode = InflateBlocks.InflateBlockMode.TABLE;
						break;
					case 3u:
						goto IL_206;
					}
					continue;
				case InflateBlocks.InflateBlockMode.LENS:
					while (i < 32)
					{
						if (num2 == 0)
						{
							goto IL_2A3;
						}
						r = 0;
						num2--;
						num3 |= (int)(this._codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					if ((~num3 >> 16 & 65535) != (num3 & 65535))
					{
						goto Block_8;
					}
					this.left = (num3 & 65535);
					i = (num3 = 0);
					this.mode = ((this.left == 0) ? ((this.last == 0) ? InflateBlocks.InflateBlockMode.TYPE : InflateBlocks.InflateBlockMode.DRY) : InflateBlocks.InflateBlockMode.STORED);
					continue;
				case InflateBlocks.InflateBlockMode.STORED:
					if (num2 == 0)
					{
						goto Block_11;
					}
					if (num5 == 0)
					{
						if (num4 == this.end && this.readAt != 0)
						{
							num4 = 0;
							num5 = ((num4 >= this.readAt) ? (this.end - num4) : (this.readAt - num4 - 1));
						}
						if (num5 == 0)
						{
							this.writeAt = num4;
							r = this.Flush(r);
							num4 = this.writeAt;
							num5 = ((num4 >= this.readAt) ? (this.end - num4) : (this.readAt - num4 - 1));
							if (num4 == this.end && this.readAt != 0)
							{
								num4 = 0;
								num5 = ((num4 >= this.readAt) ? (this.end - num4) : (this.readAt - num4 - 1));
							}
							if (num5 == 0)
							{
								goto Block_21;
							}
						}
					}
					r = 0;
					num6 = this.left;
					if (num6 > num2)
					{
						num6 = num2;
					}
					if (num6 > num5)
					{
						num6 = num5;
					}
					Array.Copy(this._codec.InputBuffer, num, this.window, num4, num6);
					num += num6;
					num2 -= num6;
					num4 += num6;
					num5 -= num6;
					if ((this.left -= num6) != 0)
					{
						continue;
					}
					this.mode = ((this.last == 0) ? InflateBlocks.InflateBlockMode.TYPE : InflateBlocks.InflateBlockMode.DRY);
					continue;
				case InflateBlocks.InflateBlockMode.TABLE:
					while (i < 14)
					{
						if (num2 == 0)
						{
							goto IL_64B;
						}
						r = 0;
						num2--;
						num3 |= (int)(this._codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = (this.table = (num3 & 16383));
					if ((num6 & 31) > 29 || (num6 >> 5 & 31) > 29)
					{
						goto IL_702;
					}
					num6 = 258 + (num6 & 31) + (num6 >> 5 & 31);
					if (this.blens == null || this.blens.Length < num6)
					{
						this.blens = new int[num6];
					}
					else
					{
						Array.Clear(this.blens, 0, num6);
					}
					num3 >>= 14;
					i -= 14;
					this.index = 0;
					this.mode = InflateBlocks.InflateBlockMode.BTREE;
					goto IL_7E6;
				case InflateBlocks.InflateBlockMode.BTREE:
					goto IL_7E6;
				case InflateBlocks.InflateBlockMode.DTREE:
					goto IL_9CF;
				case InflateBlocks.InflateBlockMode.CODES:
					goto IL_E42;
				case InflateBlocks.InflateBlockMode.DRY:
					goto IL_F34;
				case InflateBlocks.InflateBlockMode.DONE:
					goto IL_FF0;
				case InflateBlocks.InflateBlockMode.BAD:
					goto IL_1050;
				}
				break;
				for (;;)
				{
					IL_9CF:
					num6 = this.table;
					if (this.index >= 258 + (num6 & 31) + (num6 >> 5 & 31))
					{
						break;
					}
					num6 = this.bb[0];
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_A1A;
						}
						r = 0;
						num2--;
						num3 |= (int)(this._codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = this.hufts[(this.tb[0] + (num3 & InternalInflateConstants.InflateMask[num6])) * 3 + 1];
					int num7 = this.hufts[(this.tb[0] + (num3 & InternalInflateConstants.InflateMask[num6])) * 3 + 2];
					if (num7 < 16)
					{
						num3 >>= num6;
						i -= num6;
						this.blens[this.index++] = num7;
					}
					else
					{
						int num8 = (num7 != 18) ? (num7 - 14) : 7;
						int num9 = (num7 != 18) ? 3 : 11;
						while (i < num6 + num8)
						{
							if (num2 == 0)
							{
								goto IL_B5E;
							}
							r = 0;
							num2--;
							num3 |= (int)(this._codec.InputBuffer[num++] & byte.MaxValue) << i;
							i += 8;
						}
						num3 >>= num6;
						i -= num6;
						num9 += (num3 & InternalInflateConstants.InflateMask[num8]);
						num3 >>= num8;
						i -= num8;
						num8 = this.index;
						num6 = this.table;
						if (num8 + num9 > 258 + (num6 & 31) + (num6 >> 5 & 31) || (num7 == 16 && num8 < 1))
						{
							goto IL_C52;
						}
						num7 = ((num7 != 16) ? 0 : this.blens[num8 - 1]);
						do
						{
							this.blens[num8++] = num7;
						}
						while (--num9 != 0);
						this.index = num8;
					}
				}
				this.tb[0] = -1;
				int[] array5 = new int[]
				{
					9
				};
				int[] array6 = new int[]
				{
					6
				};
				int[] array7 = new int[1];
				int[] array8 = new int[1];
				num6 = this.table;
				num6 = this.inftree.inflate_trees_dynamic(257 + (num6 & 31), 1 + (num6 >> 5 & 31), this.blens, array5, array6, array7, array8, this.hufts, this._codec);
				if (num6 != 0)
				{
					goto Block_48;
				}
				this.codes.Init(array5[0], array6[0], this.hufts, array7[0], this.hufts, array8[0]);
				this.mode = InflateBlocks.InflateBlockMode.CODES;
				goto IL_E42;
				continue;
				IL_E42:
				this.bitb = num3;
				this.bitk = i;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num4;
				r = this.codes.Process(this, r);
				if (r != 1)
				{
					goto Block_50;
				}
				r = 0;
				num = this._codec.NextIn;
				num2 = this._codec.AvailableBytesIn;
				num3 = this.bitb;
				i = this.bitk;
				num4 = this.writeAt;
				num5 = ((num4 >= this.readAt) ? (this.end - num4) : (this.readAt - num4 - 1));
				if (this.last == 0)
				{
					this.mode = InflateBlocks.InflateBlockMode.TYPE;
					continue;
				}
				goto IL_F28;
				IL_7E6:
				while (this.index < 4 + (this.table >> 10))
				{
					while (i < 3)
					{
						if (num2 == 0)
						{
							goto IL_803;
						}
						r = 0;
						num2--;
						num3 |= (int)(this._codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.blens[InflateBlocks.border[this.index++]] = (num3 & 7);
					num3 >>= 3;
					i -= 3;
				}
				while (this.index < 19)
				{
					this.blens[InflateBlocks.border[this.index++]] = 0;
				}
				this.bb[0] = 7;
				num6 = this.inftree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, this._codec);
				if (num6 != 0)
				{
					goto Block_34;
				}
				this.index = 0;
				this.mode = InflateBlocks.InflateBlockMode.DTREE;
				goto IL_9CF;
			}
			r = -2;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_AD:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_206:
			num3 >>= 3;
			i -= 3;
			this.mode = InflateBlocks.InflateBlockMode.BAD;
			this._codec.Message = "invalid block type";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_2A3:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			Block_8:
			this.mode = InflateBlocks.InflateBlockMode.BAD;
			this._codec.Message = "invalid stored block lengths";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			Block_11:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			Block_21:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_64B:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_702:
			this.mode = InflateBlocks.InflateBlockMode.BAD;
			this._codec.Message = "too many length or distance symbols";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_803:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			Block_34:
			r = num6;
			if (r == -3)
			{
				this.blens = null;
				this.mode = InflateBlocks.InflateBlockMode.BAD;
			}
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_A1A:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_B5E:
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_C52:
			this.blens = null;
			this.mode = InflateBlocks.InflateBlockMode.BAD;
			this._codec.Message = "invalid bit length repeat";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			Block_48:
			if (num6 == -3)
			{
				this.blens = null;
				this.mode = InflateBlocks.InflateBlockMode.BAD;
			}
			r = num6;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			Block_50:
			return this.Flush(r);
			IL_F28:
			this.mode = InflateBlocks.InflateBlockMode.DRY;
			IL_F34:
			this.writeAt = num4;
			r = this.Flush(r);
			num4 = this.writeAt;
			int num10 = (num4 >= this.readAt) ? (this.end - num4) : (this.readAt - num4 - 1);
			if (this.readAt != this.writeAt)
			{
				this.bitb = num3;
				this.bitk = i;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num4;
				return this.Flush(r);
			}
			this.mode = InflateBlocks.InflateBlockMode.DONE;
			IL_FF0:
			r = 1;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
			IL_1050:
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
			this._codec.NextIn = num;
			this.writeAt = num4;
			return this.Flush(r);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007E2F File Offset: 0x0000622F
		internal void Free()
		{
			this.Reset();
			this.window = null;
			this.hufts = null;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007E48 File Offset: 0x00006248
		internal void SetDictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, this.window, 0, n);
			this.writeAt = n;
			this.readAt = n;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007E78 File Offset: 0x00006278
		internal int SyncPoint()
		{
			return (this.mode != InflateBlocks.InflateBlockMode.LENS) ? 0 : 1;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007EA0 File Offset: 0x000062A0
		internal int Flush(int r)
		{
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (i == 0)
				{
					num = ((this.readAt > this.writeAt) ? this.end : this.writeAt) - this.readAt;
				}
				else
				{
					num = this.writeAt - this.readAt;
				}
				if (num == 0)
				{
					if (r == -5)
					{
						r = 0;
					}
					return r;
				}
				if (num > this._codec.AvailableBytesOut)
				{
					num = this._codec.AvailableBytesOut;
				}
				if (num != 0 && r == -5)
				{
					r = 0;
				}
				this._codec.AvailableBytesOut -= num;
				this._codec.TotalBytesOut += (long)num;
				if (this.checkfn != null)
				{
					this._codec._Adler32 = (this.check = Adler.Adler32(this.check, this.window, this.readAt, num));
				}
				Array.Copy(this.window, this.readAt, this._codec.OutputBuffer, this._codec.NextOut, num);
				this._codec.NextOut += num;
				this.readAt += num;
				if (this.readAt == this.end && i == 0)
				{
					this.readAt = 0;
					if (this.writeAt == this.end)
					{
						this.writeAt = 0;
					}
				}
				else
				{
					i++;
				}
			}
			return r;
		}

		// Token: 0x0400008B RID: 139
		private const int MANY = 1440;

		// Token: 0x0400008C RID: 140
		internal static readonly int[] border = new int[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x0400008D RID: 141
		private InflateBlocks.InflateBlockMode mode;

		// Token: 0x0400008E RID: 142
		internal int left;

		// Token: 0x0400008F RID: 143
		internal int table;

		// Token: 0x04000090 RID: 144
		internal int index;

		// Token: 0x04000091 RID: 145
		internal int[] blens;

		// Token: 0x04000092 RID: 146
		internal int[] bb = new int[1];

		// Token: 0x04000093 RID: 147
		internal int[] tb = new int[1];

		// Token: 0x04000094 RID: 148
		internal InflateCodes codes = new InflateCodes();

		// Token: 0x04000095 RID: 149
		internal int last;

		// Token: 0x04000096 RID: 150
		internal ZlibCodec _codec;

		// Token: 0x04000097 RID: 151
		internal int bitk;

		// Token: 0x04000098 RID: 152
		internal int bitb;

		// Token: 0x04000099 RID: 153
		internal int[] hufts;

		// Token: 0x0400009A RID: 154
		internal byte[] window;

		// Token: 0x0400009B RID: 155
		internal int end;

		// Token: 0x0400009C RID: 156
		internal int readAt;

		// Token: 0x0400009D RID: 157
		internal int writeAt;

		// Token: 0x0400009E RID: 158
		internal object checkfn;

		// Token: 0x0400009F RID: 159
		internal uint check;

		// Token: 0x040000A0 RID: 160
		internal InfTree inftree = new InfTree();

		// Token: 0x0200000D RID: 13
		private enum InflateBlockMode
		{
			// Token: 0x040000A2 RID: 162
			TYPE,
			// Token: 0x040000A3 RID: 163
			LENS,
			// Token: 0x040000A4 RID: 164
			STORED,
			// Token: 0x040000A5 RID: 165
			TABLE,
			// Token: 0x040000A6 RID: 166
			BTREE,
			// Token: 0x040000A7 RID: 167
			DTREE,
			// Token: 0x040000A8 RID: 168
			CODES,
			// Token: 0x040000A9 RID: 169
			DRY,
			// Token: 0x040000AA RID: 170
			DONE,
			// Token: 0x040000AB RID: 171
			BAD
		}
	}
}
