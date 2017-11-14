using System;

namespace Ionic.Zlib
{
	internal sealed class InflateBlocks
	{
		private enum InflateBlockMode
		{
			TYPE,
			LENS,
			STORED,
			TABLE,
			BTREE,
			DTREE,
			CODES,
			DRY,
			DONE,
			BAD
		}

		private const int MANY = 1440;

		internal static readonly int[] border = new int[19]
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

		private InflateBlockMode mode;

		internal int left;

		internal int table;

		internal int index;

		internal int[] blens;

		internal int[] bb = new int[1];

		internal int[] tb = new int[1];

		internal InflateCodes codes = new InflateCodes();

		internal int last;

		internal ZlibCodec _codec;

		internal int bitk;

		internal int bitb;

		internal int[] hufts;

		internal byte[] window;

		internal int end;

		internal int readAt;

		internal int writeAt;

		internal object checkfn;

		internal uint check;

		internal InfTree inftree = new InfTree();

		internal InflateBlocks(ZlibCodec codec, object checkfn, int w)
		{
			this._codec = codec;
			this.hufts = new int[4320];
			this.window = new byte[w];
			this.end = w;
			this.checkfn = checkfn;
			this.mode = InflateBlockMode.TYPE;
			this.Reset();
		}

		internal uint Reset()
		{
			uint result = this.check;
			this.mode = InflateBlockMode.TYPE;
			this.bitk = 0;
			this.bitb = 0;
			this.readAt = (this.writeAt = 0);
			if (this.checkfn != null)
			{
				this._codec._Adler32 = (this.check = Adler.Adler32(0u, null, 0, 0));
			}
			return result;
		}

		internal int Process(int r)
		{
			int num = this._codec.NextIn;
			int num2 = this._codec.AvailableBytesIn;
			int num3 = this.bitb;
			int num4 = this.bitk;
			int num5 = this.writeAt;
			int num6 = (num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1);
			while (true)
			{
				switch (this.mode)
				{
				case InflateBlockMode.TYPE:
					break;
				case InflateBlockMode.LENS:
					goto IL_030c;
				case InflateBlockMode.STORED:
					goto IL_03dd;
				case InflateBlockMode.TABLE:
					goto IL_068c;
				case InflateBlockMode.BTREE:
					goto IL_085f;
				case InflateBlockMode.DTREE:
					goto IL_0965;
				case InflateBlockMode.CODES:
					goto IL_0da6;
				case InflateBlockMode.DRY:
					goto IL_0e90;
				case InflateBlockMode.DONE:
					goto end_IL_0057;
				case InflateBlockMode.BAD:
					r = -3;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				default:
					r = -2;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				while (num4 < 3)
				{
					if (num2 != 0)
					{
						r = 0;
						num2--;
						num3 |= (this._codec.InputBuffer[num++] & 0xFF) << num4;
						num4 += 8;
						continue;
					}
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				int num8 = num3 & 7;
				this.last = (num8 & 1);
				switch ((uint)num8 >> 1)
				{
				case 0u:
					num3 >>= 3;
					num4 -= 3;
					num8 = (num4 & 7);
					num3 >>= num8;
					num4 -= num8;
					this.mode = InflateBlockMode.LENS;
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
					num4 -= 3;
					this.mode = InflateBlockMode.CODES;
					break;
				}
				case 2u:
					num3 >>= 3;
					num4 -= 3;
					this.mode = InflateBlockMode.TABLE;
					break;
				case 3u:
					num3 >>= 3;
					num4 -= 3;
					this.mode = InflateBlockMode.BAD;
					this._codec.Message = "invalid block type";
					r = -3;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				continue;
				IL_068c:
				while (num4 < 14)
				{
					if (num2 != 0)
					{
						r = 0;
						num2--;
						num3 |= (this._codec.InputBuffer[num++] & 0xFF) << num4;
						num4 += 8;
						continue;
					}
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				num8 = (this.table = (num3 & 0x3FFF));
				if ((num8 & 0x1F) <= 29 && (num8 >> 5 & 0x1F) <= 29)
				{
					num8 = 258 + (num8 & 0x1F) + (num8 >> 5 & 0x1F);
					if (this.blens == null || this.blens.Length < num8)
					{
						this.blens = new int[num8];
					}
					else
					{
						Array.Clear(this.blens, 0, num8);
					}
					num3 >>= 14;
					num4 -= 14;
					this.index = 0;
					this.mode = InflateBlockMode.BTREE;
					goto IL_085f;
				}
				this.mode = InflateBlockMode.BAD;
				this._codec.Message = "too many length or distance symbols";
				r = -3;
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += num - this._codec.NextIn;
				this._codec.NextIn = num;
				this.writeAt = num5;
				return this.Flush(r);
				IL_030c:
				while (num4 < 32)
				{
					if (num2 != 0)
					{
						r = 0;
						num2--;
						num3 |= (this._codec.InputBuffer[num++] & 0xFF) << num4;
						num4 += 8;
						continue;
					}
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				if ((~num3 >> 16 & 0xFFFF) != (num3 & 0xFFFF))
				{
					this.mode = InflateBlockMode.BAD;
					this._codec.Message = "invalid stored block lengths";
					r = -3;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				this.left = (num3 & 0xFFFF);
				num3 = (num4 = 0);
				this.mode = (InflateBlockMode)((this.left == 0) ? ((this.last != 0) ? 7 : 0) : 2);
				continue;
				IL_0da6:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += num - this._codec.NextIn;
				this._codec.NextIn = num;
				this.writeAt = num5;
				r = this.codes.Process(this, r);
				if (r != 1)
				{
					return this.Flush(r);
				}
				r = 0;
				num = this._codec.NextIn;
				num2 = this._codec.AvailableBytesIn;
				num3 = this.bitb;
				num4 = this.bitk;
				num5 = this.writeAt;
				num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
				if (this.last == 0)
				{
					this.mode = InflateBlockMode.TYPE;
					continue;
				}
				this.mode = InflateBlockMode.DRY;
				goto IL_0e90;
				IL_03dd:
				if (num2 == 0)
				{
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				if (num6 == 0)
				{
					if (num5 == this.end && this.readAt != 0)
					{
						num5 = 0;
						num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
					}
					if (num6 == 0)
					{
						this.writeAt = num5;
						r = this.Flush(r);
						num5 = this.writeAt;
						num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
						if (num5 == this.end && this.readAt != 0)
						{
							num5 = 0;
							num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
						}
						if (num6 == 0)
						{
							this.bitb = num3;
							this.bitk = num4;
							this._codec.AvailableBytesIn = num2;
							this._codec.TotalBytesIn += num - this._codec.NextIn;
							this._codec.NextIn = num;
							this.writeAt = num5;
							return this.Flush(r);
						}
					}
				}
				r = 0;
				num8 = this.left;
				if (num8 > num2)
				{
					num8 = num2;
				}
				if (num8 > num6)
				{
					num8 = num6;
				}
				Array.Copy(this._codec.InputBuffer, num, this.window, num5, num8);
				num += num8;
				num2 -= num8;
				num5 += num8;
				num6 -= num8;
				if ((this.left -= num8) == 0)
				{
					this.mode = (InflateBlockMode)((this.last != 0) ? 7 : 0);
				}
				continue;
				IL_0965:
				while (true)
				{
					num8 = this.table;
					if (this.index < 258 + (num8 & 0x1F) + (num8 >> 5 & 0x1F))
					{
						num8 = this.bb[0];
						while (num4 < num8)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (this._codec.InputBuffer[num++] & 0xFF) << num4;
								num4 += 8;
								continue;
							}
							this.bitb = num3;
							this.bitk = num4;
							this._codec.AvailableBytesIn = num2;
							this._codec.TotalBytesIn += num - this._codec.NextIn;
							this._codec.NextIn = num;
							this.writeAt = num5;
							return this.Flush(r);
						}
						num8 = this.hufts[(this.tb[0] + (num3 & InternalInflateConstants.InflateMask[num8])) * 3 + 1];
						int num12 = this.hufts[(this.tb[0] + (num3 & InternalInflateConstants.InflateMask[num8])) * 3 + 2];
						if (num12 < 16)
						{
							num3 >>= num8;
							num4 -= num8;
							this.blens[this.index++] = num12;
							continue;
						}
						int num13 = (num12 != 18) ? (num12 - 14) : 7;
						int num14 = (num12 != 18) ? 3 : 11;
						while (num4 < num8 + num13)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (this._codec.InputBuffer[num++] & 0xFF) << num4;
								num4 += 8;
								continue;
							}
							this.bitb = num3;
							this.bitk = num4;
							this._codec.AvailableBytesIn = num2;
							this._codec.TotalBytesIn += num - this._codec.NextIn;
							this._codec.NextIn = num;
							this.writeAt = num5;
							return this.Flush(r);
						}
						num3 >>= num8;
						num4 -= num8;
						num14 += (num3 & InternalInflateConstants.InflateMask[num13]);
						num3 >>= num13;
						num4 -= num13;
						num13 = this.index;
						num8 = this.table;
						if (num13 + num14 <= 258 + (num8 & 0x1F) + (num8 >> 5 & 0x1F) && (num12 != 16 || num13 >= 1))
						{
							num12 = ((num12 == 16) ? this.blens[num13 - 1] : 0);
							while (true)
							{
								this.blens[num13++] = num12;
								if (--num14 == 0)
									break;
							}
							this.index = num13;
							continue;
						}
						this.blens = null;
						this.mode = InflateBlockMode.BAD;
						this._codec.Message = "invalid bit length repeat";
						r = -3;
						this.bitb = num3;
						this.bitk = num4;
						this._codec.AvailableBytesIn = num2;
						this._codec.TotalBytesIn += num - this._codec.NextIn;
						this._codec.NextIn = num;
						this.writeAt = num5;
						return this.Flush(r);
					}
					break;
				}
				this.tb[0] = -1;
				int[] array5 = new int[1]
				{
					9
				};
				int[] array6 = new int[1]
				{
					6
				};
				int[] array7 = new int[1];
				int[] array8 = new int[1];
				num8 = this.table;
				num8 = this.inftree.inflate_trees_dynamic(257 + (num8 & 0x1F), 1 + (num8 >> 5 & 0x1F), this.blens, array5, array6, array7, array8, this.hufts, this._codec);
				switch (num8)
				{
				case -3:
					this.blens = null;
					this.mode = InflateBlockMode.BAD;
					goto default;
				default:
					r = num8;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				case 0:
					break;
				}
				this.codes.Init(array5[0], array6[0], this.hufts, array7[0], this.hufts, array8[0]);
				this.mode = InflateBlockMode.CODES;
				goto IL_0da6;
				IL_085f:
				while (this.index < 4 + (this.table >> 10))
				{
					while (num4 < 3)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (this._codec.InputBuffer[num++] & 0xFF) << num4;
							num4 += 8;
							continue;
						}
						this.bitb = num3;
						this.bitk = num4;
						this._codec.AvailableBytesIn = num2;
						this._codec.TotalBytesIn += num - this._codec.NextIn;
						this._codec.NextIn = num;
						this.writeAt = num5;
						return this.Flush(r);
					}
					this.blens[InflateBlocks.border[this.index++]] = (num3 & 7);
					num3 >>= 3;
					num4 -= 3;
				}
				while (this.index < 19)
				{
					this.blens[InflateBlocks.border[this.index++]] = 0;
				}
				this.bb[0] = 7;
				num8 = this.inftree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, this._codec);
				if (num8 != 0)
				{
					r = num8;
					if (r == -3)
					{
						this.blens = null;
						this.mode = InflateBlockMode.BAD;
					}
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				this.index = 0;
				this.mode = InflateBlockMode.DTREE;
				goto IL_0965;
				IL_0e90:
				this.writeAt = num5;
				r = this.Flush(r);
				num5 = this.writeAt;
				num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
				if (this.readAt != this.writeAt)
				{
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += num - this._codec.NextIn;
					this._codec.NextIn = num;
					this.writeAt = num5;
					return this.Flush(r);
				}
				this.mode = InflateBlockMode.DONE;
				break;
				continue;
				end_IL_0057:
				break;
			}
			r = 1;
			this.bitb = num3;
			this.bitk = num4;
			this._codec.AvailableBytesIn = num2;
			this._codec.TotalBytesIn += num - this._codec.NextIn;
			this._codec.NextIn = num;
			this.writeAt = num5;
			return this.Flush(r);
		}

		internal void Free()
		{
			this.Reset();
			this.window = null;
			this.hufts = null;
		}

		internal void SetDictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, this.window, 0, n);
			this.readAt = (this.writeAt = n);
		}

		internal int SyncPoint()
		{
			return (this.mode == InflateBlockMode.LENS) ? 1 : 0;
		}

		internal int Flush(int r)
		{
			for (int i = 0; i < 2; i++)
			{
				int num = (i != 0) ? (this.writeAt - this.readAt) : (((this.readAt > this.writeAt) ? this.end : this.writeAt) - this.readAt);
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
				this._codec.TotalBytesOut += num;
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
	}
}
