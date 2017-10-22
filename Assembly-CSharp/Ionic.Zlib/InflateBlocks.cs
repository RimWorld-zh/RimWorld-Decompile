using System;

namespace Ionic.Zlib
{
	internal sealed class InflateBlocks
	{
		private enum InflateBlockMode
		{
			TYPE = 0,
			LENS = 1,
			STORED = 2,
			TABLE = 3,
			BTREE = 4,
			DTREE = 5,
			CODES = 6,
			DRY = 7,
			DONE = 8,
			BAD = 9
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
			int result;
			while (true)
			{
				int num8;
				switch (this.mode)
				{
				case InflateBlockMode.DTREE:
					goto IL_09cf;
				case InflateBlockMode.CODES:
					goto IL_0e42;
				case InflateBlockMode.DRY:
					goto IL_0f34;
				case InflateBlockMode.DONE:
					goto IL_0ff0;
				case InflateBlockMode.TYPE:
				{
					while (num4 < 3)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (this._codec.InputBuffer[num++] & 255) << num4;
							num4 += 8;
							continue;
						}
						goto IL_00ad;
					}
					num8 = (num3 & 7);
					this.last = (num8 & 1);
					switch ((uint)num8 >> 1)
					{
					case 0u:
					{
						num3 >>= 3;
						num4 -= 3;
						num8 = (num4 & 7);
						num3 >>= num8;
						num4 -= num8;
						this.mode = InflateBlockMode.LENS;
						break;
					}
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
					{
						num3 >>= 3;
						num4 -= 3;
						this.mode = InflateBlockMode.TABLE;
						break;
					}
					case 3u:
					{
						num3 >>= 3;
						num4 -= 3;
						this.mode = InflateBlockMode.BAD;
						this._codec.Message = "invalid block type";
						r = -3;
						this.bitb = num3;
						this.bitk = num4;
						this._codec.AvailableBytesIn = num2;
						this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
						this._codec.NextIn = num;
						this.writeAt = num5;
						result = this.Flush(r);
						goto end_IL_0058;
					}
					}
					continue;
				}
				case InflateBlockMode.LENS:
				{
					while (num4 < 32)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (this._codec.InputBuffer[num++] & 255) << num4;
							num4 += 8;
							continue;
						}
						goto IL_02a3;
					}
					if ((~num3 >> 16 & 65535) != (num3 & 65535))
					{
						this.mode = InflateBlockMode.BAD;
						this._codec.Message = "invalid stored block lengths";
						r = -3;
						this.bitb = num3;
						this.bitk = num4;
						this._codec.AvailableBytesIn = num2;
						this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
						this._codec.NextIn = num;
						this.writeAt = num5;
						result = this.Flush(r);
						goto end_IL_0058;
					}
					this.left = (num3 & 65535);
					num3 = (num4 = 0);
					this.mode = (InflateBlockMode)((this.left == 0) ? ((this.last != 0) ? 7 : 0) : 2);
					continue;
				}
				case InflateBlockMode.STORED:
				{
					if (num2 == 0)
					{
						this.bitb = num3;
						this.bitk = num4;
						this._codec.AvailableBytesIn = num2;
						this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
						this._codec.NextIn = num;
						this.writeAt = num5;
						result = this.Flush(r);
						goto end_IL_0058;
					}
					if (num6 == 0)
					{
						if (((num5 == this.end) ? this.readAt : 0) != 0)
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
							if (((num5 == this.end) ? this.readAt : 0) != 0)
							{
								num5 = 0;
								num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
							}
							if (num6 == 0)
							{
								this.bitb = num3;
								this.bitk = num4;
								this._codec.AvailableBytesIn = num2;
								this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
								this._codec.NextIn = num;
								this.writeAt = num5;
								result = this.Flush(r);
								goto end_IL_0058;
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
				}
				case InflateBlockMode.TABLE:
				{
					while (num4 < 14)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (this._codec.InputBuffer[num++] & 255) << num4;
							num4 += 8;
							continue;
						}
						goto IL_064b;
					}
					num8 = (this.table = (num3 & 16383));
					if ((num8 & 31) <= 29 && (num8 >> 5 & 31) <= 29)
					{
						num8 = 258 + (num8 & 31) + (num8 >> 5 & 31);
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
						goto IL_08be;
					}
					this.mode = InflateBlockMode.BAD;
					this._codec.Message = "too many length or distance symbols";
					r = -3;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
					this._codec.NextIn = num;
					this.writeAt = num5;
					result = this.Flush(r);
					break;
				}
				case InflateBlockMode.BTREE:
					goto IL_08be;
				case InflateBlockMode.BAD:
				{
					r = -3;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
					this._codec.NextIn = num;
					this.writeAt = num5;
					result = this.Flush(r);
					break;
				}
				default:
				{
					r = -2;
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
					this._codec.NextIn = num;
					this.writeAt = num5;
					result = this.Flush(r);
					break;
				}
				}
				break;
				IL_0b5e:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_02a3:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_09cf:
				while (true)
				{
					num8 = this.table;
					if (this.index < 258 + (num8 & 31) + (num8 >> 5 & 31))
					{
						num8 = this.bb[0];
						while (num4 < num8)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (this._codec.InputBuffer[num++] & 255) << num4;
								num4 += 8;
								continue;
							}
							goto IL_0a1a;
						}
						num8 = this.hufts[(this.tb[0] + (num3 & InternalInflateConstants.InflateMask[num8])) * 3 + 1];
						int num12 = this.hufts[(this.tb[0] + (num3 & InternalInflateConstants.InflateMask[num8])) * 3 + 2];
						if (num12 < 16)
						{
							num3 >>= num8;
							num4 -= num8;
							int[] obj = this.blens;
							int num13 = this.index;
							int num14 = num13;
							this.index = num13 + 1;
							obj[num14] = num12;
							continue;
						}
						int num15 = (num12 != 18) ? (num12 - 14) : 7;
						int num16 = (num12 != 18) ? 3 : 11;
						while (num4 < num8 + num15)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (this._codec.InputBuffer[num++] & 255) << num4;
								num4 += 8;
								continue;
							}
							goto IL_0b5e;
						}
						num3 >>= num8;
						num4 -= num8;
						num16 += (num3 & InternalInflateConstants.InflateMask[num15]);
						num3 >>= num15;
						num4 -= num15;
						num15 = this.index;
						num8 = this.table;
						if (num15 + num16 <= 258 + (num8 & 31) + (num8 >> 5 & 31) && (num12 != 16 || num15 >= 1))
						{
							num12 = ((num12 == 16) ? this.blens[num15 - 1] : 0);
							while (true)
							{
								this.blens[num15++] = num12;
								if (--num16 == 0)
									break;
							}
							this.index = num15;
							continue;
						}
						goto IL_0c52;
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
				num8 = this.inftree.inflate_trees_dynamic(257 + (num8 & 31), 1 + (num8 >> 5 & 31), this.blens, array5, array6, array7, array8, this.hufts, this._codec);
				switch (num8)
				{
				case -3:
				{
					this.blens = null;
					this.mode = InflateBlockMode.BAD;
					break;
				}
				case 0:
				{
					this.codes.Init(array5[0], array6[0], this.hufts, array7[0], this.hufts, array8[0]);
					this.mode = InflateBlockMode.CODES;
					goto IL_0e42;
				}
				}
				r = num8;
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_00ad:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_0f34:
				this.writeAt = num5;
				r = this.Flush(r);
				num5 = this.writeAt;
				num6 = ((num5 >= this.readAt) ? (this.end - num5) : (this.readAt - num5 - 1));
				if (this.readAt != this.writeAt)
				{
					this.bitb = num3;
					this.bitk = num4;
					this._codec.AvailableBytesIn = num2;
					this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
					this._codec.NextIn = num;
					this.writeAt = num5;
					result = this.Flush(r);
					break;
				}
				this.mode = InflateBlockMode.DONE;
				goto IL_0ff0;
				IL_064b:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_0ff0:
				r = 1;
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_0803:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_0c52:
				this.blens = null;
				this.mode = InflateBlockMode.BAD;
				this._codec.Message = "invalid bit length repeat";
				r = -3;
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				IL_0e42:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				r = this.codes.Process(this, r);
				if (r != 1)
				{
					result = this.Flush(r);
					break;
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
				goto IL_0f34;
				IL_08be:
				while (this.index < 4 + (this.table >> 10))
				{
					while (num4 < 3)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (this._codec.InputBuffer[num++] & 255) << num4;
							num4 += 8;
							continue;
						}
						goto IL_0803;
					}
					int[] obj2 = this.blens;
					int[] obj3 = InflateBlocks.border;
					int num20 = this.index;
					int num14 = num20;
					this.index = num20 + 1;
					obj2[obj3[num14]] = (num3 & 7);
					num3 >>= 3;
					num4 -= 3;
				}
				while (this.index < 19)
				{
					int[] obj4 = this.blens;
					int[] obj5 = InflateBlocks.border;
					int num21 = this.index;
					int num14 = num21;
					this.index = num21 + 1;
					obj4[obj5[num14]] = 0;
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
					this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
					this._codec.NextIn = num;
					this.writeAt = num5;
					result = this.Flush(r);
					break;
				}
				this.index = 0;
				this.mode = InflateBlockMode.DTREE;
				goto IL_09cf;
				IL_0a1a:
				this.bitb = num3;
				this.bitk = num4;
				this._codec.AvailableBytesIn = num2;
				this._codec.TotalBytesIn += (long)(num - this._codec.NextIn);
				this._codec.NextIn = num;
				this.writeAt = num5;
				result = this.Flush(r);
				break;
				continue;
				end_IL_0058:
				break;
			}
			return result;
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
			int num = 0;
			int result;
			while (true)
			{
				if (num < 2)
				{
					int num2 = (num != 0) ? (this.writeAt - this.readAt) : (((this.readAt > this.writeAt) ? this.end : this.writeAt) - this.readAt);
					if (num2 == 0)
					{
						if (r == -5)
						{
							r = 0;
						}
						result = r;
						break;
					}
					if (num2 > this._codec.AvailableBytesOut)
					{
						num2 = this._codec.AvailableBytesOut;
					}
					if (num2 != 0 && r == -5)
					{
						r = 0;
					}
					this._codec.AvailableBytesOut -= num2;
					this._codec.TotalBytesOut += (long)num2;
					if (this.checkfn != null)
					{
						this._codec._Adler32 = (this.check = Adler.Adler32(this.check, this.window, this.readAt, num2));
					}
					Array.Copy(this.window, this.readAt, this._codec.OutputBuffer, this._codec.NextOut, num2);
					this._codec.NextOut += num2;
					this.readAt += num2;
					if (this.readAt == this.end && num == 0)
					{
						this.readAt = 0;
						if (this.writeAt == this.end)
						{
							this.writeAt = 0;
						}
					}
					else
					{
						num++;
					}
					num++;
					continue;
				}
				result = r;
				break;
			}
			return result;
		}
	}
}
