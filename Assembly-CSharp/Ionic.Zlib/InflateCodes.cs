using System;

namespace Ionic.Zlib
{
	internal sealed class InflateCodes
	{
		private const int START = 0;

		private const int LEN = 1;

		private const int LENEXT = 2;

		private const int DIST = 3;

		private const int DISTEXT = 4;

		private const int COPY = 5;

		private const int LIT = 6;

		private const int WASH = 7;

		private const int END = 8;

		private const int BADCODE = 9;

		internal int mode;

		internal int len;

		internal int[] tree;

		internal int tree_index;

		internal int need;

		internal int lit;

		internal int bitsToGet;

		internal int dist;

		internal byte lbits;

		internal byte dbits;

		internal int[] ltree;

		internal int ltree_index;

		internal int[] dtree;

		internal int dtree_index;

		internal InflateCodes()
		{
		}

		internal void Init(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index)
		{
			this.mode = 0;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = tl_index;
			this.dtree = td;
			this.dtree_index = td_index;
			this.tree = null;
		}

		internal int Process(InflateBlocks blocks, int r)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			ZlibCodec codec = blocks._codec;
			num3 = codec.NextIn;
			int num4 = codec.AvailableBytesIn;
			num = blocks.bitb;
			num2 = blocks.bitk;
			int num5 = blocks.writeAt;
			int num6 = (num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1);
			while (true)
			{
				switch (this.mode)
				{
				case 0:
					break;
				case 1:
					goto IL_01c4;
				case 2:
					goto IL_0387;
				case 3:
					goto IL_0471;
				case 4:
					goto IL_05fd;
				case 5:
					goto IL_06c3;
				case 6:
					goto IL_0868;
				case 7:
					if (num2 > 7)
					{
						num2 -= 8;
						num4++;
						num3--;
					}
					blocks.writeAt = num5;
					r = blocks.Flush(r);
					num5 = blocks.writeAt;
					num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
					if (blocks.readAt != blocks.writeAt)
					{
						blocks.bitb = num;
						blocks.bitk = num2;
						codec.AvailableBytesIn = num4;
						codec.TotalBytesIn += num3 - codec.NextIn;
						codec.NextIn = num3;
						blocks.writeAt = num5;
						return blocks.Flush(r);
					}
					this.mode = 8;
					goto case 8;
				case 8:
					r = 1;
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				case 9:
					r = -3;
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				default:
					r = -2;
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				}
				if (num6 >= 258 && num4 >= 10)
				{
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					r = this.InflateFast(this.lbits, this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, blocks, codec);
					num3 = codec.NextIn;
					num4 = codec.AvailableBytesIn;
					num = blocks.bitb;
					num2 = blocks.bitk;
					num5 = blocks.writeAt;
					num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
					if (r != 0)
					{
						this.mode = ((r != 1) ? 9 : 7);
						continue;
					}
				}
				this.need = this.lbits;
				this.tree = this.ltree;
				this.tree_index = this.ltree_index;
				this.mode = 1;
				goto IL_01c4;
				IL_0868:
				if (num6 == 0)
				{
					if (num5 == blocks.end && blocks.readAt != 0)
					{
						num5 = 0;
						num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
					}
					if (num6 == 0)
					{
						blocks.writeAt = num5;
						r = blocks.Flush(r);
						num5 = blocks.writeAt;
						num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
						if (num5 == blocks.end && blocks.readAt != 0)
						{
							num5 = 0;
							num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
						}
						if (num6 == 0)
							break;
					}
				}
				r = 0;
				blocks.window[num5++] = (byte)this.lit;
				num6--;
				this.mode = 0;
				continue;
				IL_0471:
				int num8 = this.need;
				while (num2 < num8)
				{
					if (num4 != 0)
					{
						r = 0;
						num4--;
						num |= (codec.InputBuffer[num3++] & 0xFF) << num2;
						num2 += 8;
						continue;
					}
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				}
				int num10 = (this.tree_index + (num & InternalInflateConstants.InflateMask[num8])) * 3;
				num >>= this.tree[num10 + 1];
				num2 -= this.tree[num10 + 1];
				int num11 = this.tree[num10];
				if ((num11 & 0x10) != 0)
				{
					this.bitsToGet = (num11 & 0xF);
					this.dist = this.tree[num10 + 2];
					this.mode = 4;
					continue;
				}
				if ((num11 & 0x40) == 0)
				{
					this.need = num11;
					this.tree_index = num10 / 3 + this.tree[num10 + 2];
					continue;
				}
				this.mode = 9;
				codec.Message = "invalid distance code";
				r = -3;
				blocks.bitb = num;
				blocks.bitk = num2;
				codec.AvailableBytesIn = num4;
				codec.TotalBytesIn += num3 - codec.NextIn;
				codec.NextIn = num3;
				blocks.writeAt = num5;
				return blocks.Flush(r);
				IL_05fd:
				num8 = this.bitsToGet;
				while (num2 < num8)
				{
					if (num4 != 0)
					{
						r = 0;
						num4--;
						num |= (codec.InputBuffer[num3++] & 0xFF) << num2;
						num2 += 8;
						continue;
					}
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				}
				this.dist += (num & InternalInflateConstants.InflateMask[num8]);
				num >>= num8;
				num2 -= num8;
				this.mode = 5;
				goto IL_06c3;
				IL_06c3:
				int i;
				for (i = num5 - this.dist; i < 0; i += blocks.end)
				{
				}
				while (this.len != 0)
				{
					if (num6 == 0)
					{
						if (num5 == blocks.end && blocks.readAt != 0)
						{
							num5 = 0;
							num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
						}
						if (num6 == 0)
						{
							blocks.writeAt = num5;
							r = blocks.Flush(r);
							num5 = blocks.writeAt;
							num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
							if (num5 == blocks.end && blocks.readAt != 0)
							{
								num5 = 0;
								num6 = ((num5 >= blocks.readAt) ? (blocks.end - num5) : (blocks.readAt - num5 - 1));
							}
							if (num6 == 0)
							{
								blocks.bitb = num;
								blocks.bitk = num2;
								codec.AvailableBytesIn = num4;
								codec.TotalBytesIn += num3 - codec.NextIn;
								codec.NextIn = num3;
								blocks.writeAt = num5;
								return blocks.Flush(r);
							}
						}
					}
					blocks.window[num5++] = blocks.window[i++];
					num6--;
					if (i == blocks.end)
					{
						i = 0;
					}
					this.len--;
				}
				this.mode = 0;
				continue;
				IL_0387:
				num8 = this.bitsToGet;
				while (num2 < num8)
				{
					if (num4 != 0)
					{
						r = 0;
						num4--;
						num |= (codec.InputBuffer[num3++] & 0xFF) << num2;
						num2 += 8;
						continue;
					}
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				}
				this.len += (num & InternalInflateConstants.InflateMask[num8]);
				num >>= num8;
				num2 -= num8;
				this.need = this.dbits;
				this.tree = this.dtree;
				this.tree_index = this.dtree_index;
				this.mode = 3;
				goto IL_0471;
				IL_01c4:
				num8 = this.need;
				while (num2 < num8)
				{
					if (num4 != 0)
					{
						r = 0;
						num4--;
						num |= (codec.InputBuffer[num3++] & 0xFF) << num2;
						num2 += 8;
						continue;
					}
					blocks.bitb = num;
					blocks.bitk = num2;
					codec.AvailableBytesIn = num4;
					codec.TotalBytesIn += num3 - codec.NextIn;
					codec.NextIn = num3;
					blocks.writeAt = num5;
					return blocks.Flush(r);
				}
				num10 = (this.tree_index + (num & InternalInflateConstants.InflateMask[num8])) * 3;
				num >>= this.tree[num10 + 1];
				num2 -= this.tree[num10 + 1];
				num11 = this.tree[num10];
				if (num11 == 0)
				{
					this.lit = this.tree[num10 + 2];
					this.mode = 6;
					continue;
				}
				if ((num11 & 0x10) != 0)
				{
					this.bitsToGet = (num11 & 0xF);
					this.len = this.tree[num10 + 2];
					this.mode = 2;
					continue;
				}
				if ((num11 & 0x40) == 0)
				{
					this.need = num11;
					this.tree_index = num10 / 3 + this.tree[num10 + 2];
					continue;
				}
				if ((num11 & 0x20) != 0)
				{
					this.mode = 7;
					continue;
				}
				this.mode = 9;
				codec.Message = "invalid literal/length code";
				r = -3;
				blocks.bitb = num;
				blocks.bitk = num2;
				codec.AvailableBytesIn = num4;
				codec.TotalBytesIn += num3 - codec.NextIn;
				codec.NextIn = num3;
				blocks.writeAt = num5;
				return blocks.Flush(r);
			}
			blocks.bitb = num;
			blocks.bitk = num2;
			codec.AvailableBytesIn = num4;
			codec.TotalBytesIn += num3 - codec.NextIn;
			codec.NextIn = num3;
			blocks.writeAt = num5;
			return blocks.Flush(r);
		}

		internal int InflateFast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InflateBlocks s, ZlibCodec z)
		{
			int num = z.NextIn;
			int num2 = z.AvailableBytesIn;
			int num3 = s.bitb;
			int num4 = s.bitk;
			int num5 = s.writeAt;
			int num6 = (num5 >= s.readAt) ? (s.end - num5) : (s.readAt - num5 - 1);
			int num7 = InternalInflateConstants.InflateMask[bl];
			int num8 = InternalInflateConstants.InflateMask[bd];
			int num14;
			while (true)
			{
				if (num4 < 20)
				{
					num2--;
					num3 |= (z.InputBuffer[num++] & 0xFF) << num4;
					num4 += 8;
					continue;
				}
				int num10 = num3 & num7;
				int num11 = (tl_index + num10) * 3;
				int num12;
				if ((num12 = tl[num11]) == 0)
				{
					num3 >>= tl[num11 + 1];
					num4 -= tl[num11 + 1];
					s.window[num5++] = (byte)tl[num11 + 2];
					num6--;
				}
				else
				{
					while (true)
					{
						num3 >>= tl[num11 + 1];
						num4 -= tl[num11 + 1];
						if ((num12 & 0x10) == 0)
						{
							if ((num12 & 0x40) == 0)
							{
								num10 += tl[num11 + 2];
								num10 += (num3 & InternalInflateConstants.InflateMask[num12]);
								num11 = (tl_index + num10) * 3;
								if ((num12 = tl[num11]) != 0)
									continue;
								goto IL_04cb;
							}
							goto IL_050a;
						}
						break;
					}
					num12 &= 0xF;
					num14 = tl[num11 + 2] + (num3 & InternalInflateConstants.InflateMask[num12]);
					num3 >>= num12;
					for (num4 -= num12; num4 < 15; num4 += 8)
					{
						num2--;
						num3 |= (z.InputBuffer[num++] & 0xFF) << num4;
					}
					num10 = (num3 & num8);
					num11 = (td_index + num10) * 3;
					num12 = td[num11];
					while (true)
					{
						num3 >>= td[num11 + 1];
						num4 -= td[num11 + 1];
						if ((num12 & 0x10) == 0)
						{
							if ((num12 & 0x40) == 0)
							{
								num10 += td[num11 + 2];
								num10 += (num3 & InternalInflateConstants.InflateMask[num12]);
								num11 = (td_index + num10) * 3;
								num12 = td[num11];
								continue;
							}
							z.Message = "invalid distance code";
							num14 = z.AvailableBytesIn - num2;
							num14 = ((num4 >> 3 >= num14) ? num14 : (num4 >> 3));
							num2 += num14;
							num -= num14;
							num4 -= num14 << 3;
							s.bitb = num3;
							s.bitk = num4;
							z.AvailableBytesIn = num2;
							z.TotalBytesIn += num - z.NextIn;
							z.NextIn = num;
							s.writeAt = num5;
							return -3;
						}
						break;
					}
					num12 &= 0xF;
					for (; num4 < num12; num4 += 8)
					{
						num2--;
						num3 |= (z.InputBuffer[num++] & 0xFF) << num4;
					}
					int num17 = td[num11 + 2] + (num3 & InternalInflateConstants.InflateMask[num12]);
					num3 >>= num12;
					num4 -= num12;
					num6 -= num14;
					int num18;
					if (num5 >= num17)
					{
						num18 = num5 - num17;
						if (num5 - num18 > 0 && 2 > num5 - num18)
						{
							s.window[num5++] = s.window[num18++];
							s.window[num5++] = s.window[num18++];
							num14 -= 2;
						}
						else
						{
							Array.Copy(s.window, num18, s.window, num5, 2);
							num5 += 2;
							num18 += 2;
							num14 -= 2;
						}
					}
					else
					{
						num18 = num5 - num17;
						while (true)
						{
							num18 += s.end;
							if (num18 >= 0)
								break;
						}
						num12 = s.end - num18;
						if (num14 > num12)
						{
							num14 -= num12;
							if (num5 - num18 > 0 && num12 > num5 - num18)
							{
								while (true)
								{
									s.window[num5++] = s.window[num18++];
									if (--num12 == 0)
										break;
								}
							}
							else
							{
								Array.Copy(s.window, num18, s.window, num5, num12);
								num5 += num12;
								num18 += num12;
								num12 = 0;
							}
							num18 = 0;
						}
					}
					if (num5 - num18 > 0 && num14 > num5 - num18)
					{
						while (true)
						{
							s.window[num5++] = s.window[num18++];
							if (--num14 == 0)
								break;
						}
					}
					else
					{
						Array.Copy(s.window, num18, s.window, num5, num14);
						num5 += num14;
						num18 += num14;
						num14 = 0;
					}
				}
				goto IL_062b;
				IL_062b:
				if (num6 < 258)
					break;
				if (num2 < 10)
					break;
				continue;
				IL_04cb:
				num3 >>= tl[num11 + 1];
				num4 -= tl[num11 + 1];
				s.window[num5++] = (byte)tl[num11 + 2];
				num6--;
				goto IL_062b;
				IL_050a:
				if ((num12 & 0x20) != 0)
				{
					num14 = z.AvailableBytesIn - num2;
					num14 = ((num4 >> 3 >= num14) ? num14 : (num4 >> 3));
					num2 += num14;
					num -= num14;
					num4 -= num14 << 3;
					s.bitb = num3;
					s.bitk = num4;
					z.AvailableBytesIn = num2;
					z.TotalBytesIn += num - z.NextIn;
					z.NextIn = num;
					s.writeAt = num5;
					return 1;
				}
				z.Message = "invalid literal/length code";
				num14 = z.AvailableBytesIn - num2;
				num14 = ((num4 >> 3 >= num14) ? num14 : (num4 >> 3));
				num2 += num14;
				num -= num14;
				num4 -= num14 << 3;
				s.bitb = num3;
				s.bitk = num4;
				z.AvailableBytesIn = num2;
				z.TotalBytesIn += num - z.NextIn;
				z.NextIn = num;
				s.writeAt = num5;
				return -3;
			}
			num14 = z.AvailableBytesIn - num2;
			num14 = ((num4 >> 3 >= num14) ? num14 : (num4 >> 3));
			num2 += num14;
			num -= num14;
			num4 -= num14 << 3;
			s.bitb = num3;
			s.bitk = num4;
			z.AvailableBytesIn = num2;
			z.TotalBytesIn += num - z.NextIn;
			z.NextIn = num;
			s.writeAt = num5;
			return 0;
		}
	}
}
