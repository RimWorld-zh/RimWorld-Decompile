using System;

namespace Ionic.Zlib
{
	internal sealed class DeflateManager
	{
		internal delegate BlockState CompressFunc(FlushType flush);

		internal class Config
		{
			internal int GoodLength;

			internal int MaxLazy;

			internal int NiceLength;

			internal int MaxChainLength;

			internal DeflateFlavor Flavor;

			private static readonly Config[] Table;

			private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor)
			{
				this.GoodLength = goodLength;
				this.MaxLazy = maxLazy;
				this.NiceLength = niceLength;
				this.MaxChainLength = maxChainLength;
				this.Flavor = flavor;
			}

			static Config()
			{
				Config.Table = new Config[10]
				{
					new Config(0, 0, 0, 0, DeflateFlavor.Store),
					new Config(4, 4, 8, 4, DeflateFlavor.Fast),
					new Config(4, 5, 16, 8, DeflateFlavor.Fast),
					new Config(4, 6, 32, 32, DeflateFlavor.Fast),
					new Config(4, 4, 16, 16, DeflateFlavor.Slow),
					new Config(8, 16, 32, 32, DeflateFlavor.Slow),
					new Config(8, 16, 128, 128, DeflateFlavor.Slow),
					new Config(8, 32, 128, 256, DeflateFlavor.Slow),
					new Config(32, 128, 258, 1024, DeflateFlavor.Slow),
					new Config(32, 258, 258, 4096, DeflateFlavor.Slow)
				};
			}

			public static Config Lookup(CompressionLevel level)
			{
				return Config.Table[(int)level];
			}
		}

		private static readonly int MEM_LEVEL_MAX = 9;

		private static readonly int MEM_LEVEL_DEFAULT = 8;

		private CompressFunc DeflateFunction;

		private static readonly string[] _ErrorMessage = new string[10]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		private static readonly int PRESET_DICT = 32;

		private static readonly int INIT_STATE = 42;

		private static readonly int BUSY_STATE = 113;

		private static readonly int FINISH_STATE = 666;

		private static readonly int Z_DEFLATED = 8;

		private static readonly int STORED_BLOCK = 0;

		private static readonly int STATIC_TREES = 1;

		private static readonly int DYN_TREES = 2;

		private static readonly int Z_BINARY = 0;

		private static readonly int Z_ASCII = 1;

		private static readonly int Z_UNKNOWN = 2;

		private static readonly int Buf_size = 16;

		private static readonly int MIN_MATCH = 3;

		private static readonly int MAX_MATCH = 258;

		private static readonly int MIN_LOOKAHEAD = DeflateManager.MAX_MATCH + DeflateManager.MIN_MATCH + 1;

		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		private static readonly int END_BLOCK = 256;

		internal ZlibCodec _codec;

		internal int status;

		internal byte[] pending;

		internal int nextPending;

		internal int pendingCount;

		internal sbyte data_type;

		internal int last_flush;

		internal int w_size;

		internal int w_bits;

		internal int w_mask;

		internal byte[] window;

		internal int window_size;

		internal short[] prev;

		internal short[] head;

		internal int ins_h;

		internal int hash_size;

		internal int hash_bits;

		internal int hash_mask;

		internal int hash_shift;

		internal int block_start;

		private Config config;

		internal int match_length;

		internal int prev_match;

		internal int match_available;

		internal int strstart;

		internal int match_start;

		internal int lookahead;

		internal int prev_length;

		internal CompressionLevel compressionLevel;

		internal CompressionStrategy compressionStrategy;

		internal short[] dyn_ltree;

		internal short[] dyn_dtree;

		internal short[] bl_tree;

		internal Tree treeLiterals = new Tree();

		internal Tree treeDistances = new Tree();

		internal Tree treeBitLengths = new Tree();

		internal short[] bl_count = new short[InternalConstants.MAX_BITS + 1];

		internal int[] heap = new int[2 * InternalConstants.L_CODES + 1];

		internal int heap_len;

		internal int heap_max;

		internal sbyte[] depth = new sbyte[2 * InternalConstants.L_CODES + 1];

		internal int _lengthOffset;

		internal int lit_bufsize;

		internal int last_lit;

		internal int _distanceOffset;

		internal int opt_len;

		internal int static_len;

		internal int matches;

		internal int last_eob_len;

		internal short bi_buf;

		internal int bi_valid;

		private bool Rfc1950BytesEmitted = false;

		private bool _WantRfc1950HeaderBytes = true;

		internal bool WantRfc1950HeaderBytes
		{
			get
			{
				return this._WantRfc1950HeaderBytes;
			}
			set
			{
				this._WantRfc1950HeaderBytes = value;
			}
		}

		internal DeflateManager()
		{
			this.dyn_ltree = new short[DeflateManager.HEAP_SIZE * 2];
			this.dyn_dtree = new short[(2 * InternalConstants.D_CODES + 1) * 2];
			this.bl_tree = new short[(2 * InternalConstants.BL_CODES + 1) * 2];
		}

		private void _InitializeLazyMatch()
		{
			this.window_size = 2 * this.w_size;
			Array.Clear(this.head, 0, this.hash_size);
			this.config = Config.Lookup(this.compressionLevel);
			this.SetDeflater();
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = DeflateManager.MIN_MATCH - 1);
			this.match_available = 0;
			this.ins_h = 0;
		}

		private void _InitializeTreeData()
		{
			this.treeLiterals.dyn_tree = this.dyn_ltree;
			this.treeLiterals.staticTree = StaticTree.Literals;
			this.treeDistances.dyn_tree = this.dyn_dtree;
			this.treeDistances.staticTree = StaticTree.Distances;
			this.treeBitLengths.dyn_tree = this.bl_tree;
			this.treeBitLengths.staticTree = StaticTree.BitLengths;
			this.bi_buf = (short)0;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this._InitializeBlocks();
		}

		internal void _InitializeBlocks()
		{
			for (int i = 0; i < InternalConstants.L_CODES; i++)
			{
				this.dyn_ltree[i * 2] = (short)0;
			}
			for (int j = 0; j < InternalConstants.D_CODES; j++)
			{
				this.dyn_dtree[j * 2] = (short)0;
			}
			for (int k = 0; k < InternalConstants.BL_CODES; k++)
			{
				this.bl_tree[k * 2] = (short)0;
			}
			this.dyn_ltree[DeflateManager.END_BLOCK * 2] = (short)1;
			this.opt_len = (this.static_len = 0);
			this.last_lit = (this.matches = 0);
		}

		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			int num2 = k << 1;
			while (num2 <= this.heap_len)
			{
				if (num2 < this.heap_len && DeflateManager._IsSmaller(tree, this.heap[num2 + 1], this.heap[num2], this.depth))
				{
					num2++;
				}
				if (!DeflateManager._IsSmaller(tree, num, this.heap[num2], this.depth))
				{
					this.heap[k] = this.heap[num2];
					k = num2;
					num2 <<= 1;
					continue;
				}
				break;
			}
			this.heap[k] = num;
		}

		internal static bool _IsSmaller(short[] tree, int n, int m, sbyte[] depth)
		{
			short num = tree[n * 2];
			short num2 = tree[m * 2];
			return num < num2 || (num == num2 && depth[n] <= depth[m]);
		}

		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = (short)32767;
			for (int num6 = 0; num6 <= max_code; num6++)
			{
				int num7 = num2;
				num2 = tree[(num6 + 1) * 2 + 1];
				if (++num3 >= num4 || num7 != num2)
				{
					if (num3 < num5)
					{
						this.bl_tree[num7 * 2] = (short)(this.bl_tree[num7 * 2] + num3);
					}
					else if (num7 != 0)
					{
						if (num7 != num)
						{
							ref short val = ref this.bl_tree[num7 * 2];
							val = (short)(val + 1);
						}
						ref short val2 = ref this.bl_tree[InternalConstants.REP_3_6 * 2];
						val2 = (short)(val2 + 1);
					}
					else if (num3 <= 10)
					{
						ref short val3 = ref this.bl_tree[InternalConstants.REPZ_3_10 * 2];
						val3 = (short)(val3 + 1);
					}
					else
					{
						ref short val4 = ref this.bl_tree[InternalConstants.REPZ_11_138 * 2];
						val4 = (short)(val4 + 1);
					}
					num3 = 0;
					num = num7;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num7 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		internal int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.treeLiterals.max_code);
			this.scan_tree(this.dyn_dtree, this.treeDistances.max_code);
			this.treeBitLengths.build_tree(this);
			int num = InternalConstants.BL_CODES - 1;
			while (num >= 3 && this.bl_tree[Tree.bl_order[num] * 2 + 1] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int num = 0; num < blcodes; num++)
			{
				this.send_bits(this.bl_tree[Tree.bl_order[num] * 2 + 1], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int num6 = 0; num6 <= max_code; num6++)
			{
				int num7 = num2;
				num2 = tree[(num6 + 1) * 2 + 1];
				if (++num3 >= num4 || num7 != num2)
				{
					if (num3 < num5)
					{
						while (true)
						{
							this.send_code(num7, this.bl_tree);
							if (--num3 == 0)
								break;
						}
					}
					else if (num7 != 0)
					{
						if (num7 != num)
						{
							this.send_code(num7, this.bl_tree);
							num3--;
						}
						this.send_code(InternalConstants.REP_3_6, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(InternalConstants.REPZ_3_10, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(InternalConstants.REPZ_11_138, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num7;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num7 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		private void put_bytes(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending, this.pendingCount, len);
			this.pendingCount += len;
		}

		internal void send_code(int c, short[] tree)
		{
			int num = c * 2;
			this.send_bits(tree[num] & 65535, tree[num + 1] & 65535);
		}

		internal void send_bits(int value, int length)
		{
			if (this.bi_valid > DeflateManager.Buf_size - length)
			{
				this.bi_buf = (short)(this.bi_buf | (short)(value << this.bi_valid & 65535));
				byte[] obj = this.pending;
				int num = this.pendingCount;
				int num2 = num;
				this.pendingCount = num + 1;
				obj[num2] = (byte)this.bi_buf;
				byte[] obj2 = this.pending;
				int num3 = this.pendingCount;
				num2 = num3;
				this.pendingCount = num3 + 1;
				obj2[num2] = (byte)(this.bi_buf >> 8);
				this.bi_buf = (short)((uint)value >> DeflateManager.Buf_size - this.bi_valid);
				this.bi_valid += length - DeflateManager.Buf_size;
			}
			else
			{
				this.bi_buf = (short)(this.bi_buf | (short)(value << this.bi_valid & 65535));
				this.bi_valid += length;
			}
		}

		internal void _tr_align()
		{
			this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
			this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
				this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		internal bool _tr_tally(int dist, int lc)
		{
			this.pending[this._distanceOffset + this.last_lit * 2] = (byte)((uint)dist >> 8);
			this.pending[this._distanceOffset + this.last_lit * 2 + 1] = (byte)dist;
			this.pending[this._lengthOffset + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				ref short val = ref this.dyn_ltree[lc * 2];
				val = (short)(val + 1);
			}
			else
			{
				this.matches++;
				dist--;
				ref short val2 = ref this.dyn_ltree[(Tree.LengthCode[lc] + InternalConstants.LITERALS + 1) * 2];
				val2 = (short)(val2 + 1);
				ref short val3 = ref this.dyn_dtree[Tree.DistanceCode(dist) * 2];
				val3 = (short)(val3 + 1);
			}
			bool result;
			if ((this.last_lit & 8191) == 0 && this.compressionLevel > CompressionLevel.Level2)
			{
				int num = this.last_lit << 3;
				int num2 = this.strstart - this.block_start;
				for (int i = 0; i < InternalConstants.D_CODES; i++)
				{
					num = (int)(num + this.dyn_dtree[i * 2] * (5L + (long)Tree.ExtraDistanceBits[i]));
				}
				num >>= 3;
				if (this.matches < this.last_lit / 2 && num < num2 / 2)
				{
					result = true;
					goto IL_0186;
				}
			}
			result = (this.last_lit == this.lit_bufsize - 1 || this.last_lit == this.lit_bufsize);
			goto IL_0186;
			IL_0186:
			return result;
		}

		internal void send_compressed_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				while (true)
				{
					int num2 = this._distanceOffset + num * 2;
					int num3 = (this.pending[num2] << 8 & 65280) | (this.pending[num2 + 1] & 255);
					int num4 = this.pending[this._lengthOffset + num] & 255;
					num++;
					if (num3 == 0)
					{
						this.send_code(num4, ltree);
					}
					else
					{
						int num5 = Tree.LengthCode[num4];
						this.send_code(num5 + InternalConstants.LITERALS + 1, ltree);
						int num6 = Tree.ExtraLengthBits[num5];
						if (num6 != 0)
						{
							num4 -= Tree.LengthBase[num5];
							this.send_bits(num4, num6);
						}
						num3--;
						num5 = Tree.DistanceCode(num3);
						this.send_code(num5, dtree);
						num6 = Tree.ExtraDistanceBits[num5];
						if (num6 != 0)
						{
							num3 -= Tree.DistanceBase[num5];
							this.send_bits(num3, num6);
						}
					}
					if (num >= this.last_lit)
						break;
				}
			}
			this.send_code(DeflateManager.END_BLOCK, ltree);
			this.last_eob_len = ltree[DeflateManager.END_BLOCK * 2 + 1];
		}

		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			for (; i < 7; i++)
			{
				num2 += this.dyn_ltree[i * 2];
			}
			for (; i < 128; i++)
			{
				num += this.dyn_ltree[i * 2];
			}
			for (; i < InternalConstants.LITERALS; i++)
			{
				num2 += this.dyn_ltree[i * 2];
			}
			this.data_type = (sbyte)((num2 <= num >> 2) ? DeflateManager.Z_ASCII : DeflateManager.Z_BINARY);
		}

		internal void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				byte[] obj = this.pending;
				int num = this.pendingCount;
				int num2 = num;
				this.pendingCount = num + 1;
				obj[num2] = (byte)this.bi_buf;
				byte[] obj2 = this.pending;
				int num3 = this.pendingCount;
				num2 = num3;
				this.pendingCount = num3 + 1;
				obj2[num2] = (byte)(this.bi_buf >> 8);
				this.bi_buf = (short)0;
				this.bi_valid = 0;
			}
			else if (this.bi_valid >= 8)
			{
				byte[] obj3 = this.pending;
				int num4 = this.pendingCount;
				int num2 = num4;
				this.pendingCount = num4 + 1;
				obj3[num2] = (byte)this.bi_buf;
				this.bi_buf = (short)(this.bi_buf >> 8);
				this.bi_valid -= 8;
			}
		}

		internal void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				byte[] obj = this.pending;
				int num = this.pendingCount;
				int num2 = num;
				this.pendingCount = num + 1;
				obj[num2] = (byte)this.bi_buf;
				byte[] obj2 = this.pending;
				int num3 = this.pendingCount;
				num2 = num3;
				this.pendingCount = num3 + 1;
				obj2[num2] = (byte)(this.bi_buf >> 8);
			}
			else if (this.bi_valid > 0)
			{
				byte[] obj3 = this.pending;
				int num4 = this.pendingCount;
				int num2 = num4;
				this.pendingCount = num4 + 1;
				obj3[num2] = (byte)this.bi_buf;
			}
			this.bi_buf = (short)0;
			this.bi_valid = 0;
		}

		internal void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				byte[] obj = this.pending;
				int num = this.pendingCount;
				int num2 = num;
				this.pendingCount = num + 1;
				obj[num2] = (byte)len;
				byte[] obj2 = this.pending;
				int num3 = this.pendingCount;
				num2 = num3;
				this.pendingCount = num3 + 1;
				obj2[num2] = (byte)(len >> 8);
				byte[] obj3 = this.pending;
				int num4 = this.pendingCount;
				num2 = num4;
				this.pendingCount = num4 + 1;
				obj3[num2] = (byte)(~len);
				byte[] obj4 = this.pending;
				int num5 = this.pendingCount;
				num2 = num5;
				this.pendingCount = num5 + 1;
				obj4[num2] = (byte)(~len >> 8);
			}
			this.put_bytes(this.window, buf, len);
		}

		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start < 0) ? (-1) : this.block_start, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this._codec.flush_pending();
		}

		internal BlockState DeflateNone(FlushType flush)
		{
			int num = 65535;
			if (num > this.pending.Length - 5)
			{
				num = this.pending.Length - 5;
			}
			BlockState result;
			while (true)
			{
				if (this.lookahead <= 1)
				{
					this._fillWindow();
					if (this.lookahead == 0 && flush == FlushType.None)
					{
						result = BlockState.NeedMore;
					}
					else
					{
						if (this.lookahead != 0)
							goto IL_0062;
						this.flush_block_only(flush == FlushType.Finish);
						result = (BlockState)((this._codec.AvailableBytesOut != 0) ? ((flush != FlushType.Finish) ? 1 : 3) : ((flush == FlushType.Finish) ? 2 : 0));
					}
					break;
				}
				goto IL_0062;
				IL_0062:
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						result = BlockState.NeedMore;
						break;
					}
				}
				if (this.strstart - this.block_start < this.w_size - DeflateManager.MIN_LOOKAHEAD)
					continue;
				this.flush_block_only(false);
				if (this._codec.AvailableBytesOut != 0)
					continue;
				result = BlockState.NeedMore;
				break;
			}
			return result;
		}

		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits((DeflateManager.STORED_BLOCK << 1) + (eof ? 1 : 0), 3);
			this.copy_block(buf, stored_len, true);
		}

		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.compressionLevel > CompressionLevel.None)
			{
				if (this.data_type == DeflateManager.Z_UNKNOWN)
				{
					this.set_data_type();
				}
				this.treeLiterals.build_tree(this);
				this.treeDistances.build_tree(this);
				num = this.build_bl_tree();
				num2 = this.opt_len + 3 + 7 >> 3;
				num3 = this.static_len + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num2 = (num3 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits((DeflateManager.STATIC_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
			}
			else
			{
				this.send_bits((DeflateManager.DYN_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_all_trees(this.treeLiterals.max_code + 1, this.treeDistances.max_code + 1, num + 1);
				this.send_compressed_block(this.dyn_ltree, this.dyn_dtree);
			}
			this._InitializeBlocks();
			if (eof)
			{
				this.bi_windup();
			}
		}

		private void _fillWindow()
		{
			while (true)
			{
				int num = this.window_size - this.lookahead - this.strstart;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					int num2;
					int num3 = num2 = this.hash_size;
					while (true)
					{
						int num4 = this.head[--num2] & 65535;
						this.head[num2] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
						if (--num3 == 0)
							break;
					}
					num3 = (num2 = this.w_size);
					while (true)
					{
						int num4 = this.prev[--num2] & 65535;
						this.prev[num2] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
						if (--num3 == 0)
							break;
					}
					num += this.w_size;
				}
				if (this._codec.AvailableBytesIn != 0)
				{
					int num3 = this._codec.read_buf(this.window, this.strstart + this.lookahead, num);
					this.lookahead += num3;
					if (this.lookahead >= DeflateManager.MIN_MATCH)
					{
						this.ins_h = (this.window[this.strstart] & 255);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[this.strstart + 1] & 255)) & this.hash_mask);
					}
					if (this.lookahead >= DeflateManager.MIN_LOOKAHEAD)
						break;
					if (this._codec.AvailableBytesIn == 0)
						break;
					continue;
				}
				break;
			}
		}

		internal BlockState DeflateFast(FlushType flush)
		{
			int num = 0;
			BlockState result;
			while (true)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						result = BlockState.NeedMore;
					}
					else
					{
						if (this.lookahead != 0)
							goto IL_004b;
						this.flush_block_only(flush == FlushType.Finish);
						result = (BlockState)((this._codec.AvailableBytesOut != 0) ? ((flush != FlushType.Finish) ? 1 : 3) : ((flush == FlushType.Finish) ? 2 : 0));
					}
					break;
				}
				goto IL_004b;
				IL_004b:
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
					num = (this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if (num != 0 && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD && this.compressionStrategy != CompressionStrategy.HuffmanOnly)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= DeflateManager.MIN_MATCH)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.config.MaxLazy && this.lookahead >= DeflateManager.MIN_MATCH)
					{
						this.match_length--;
						while (true)
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
							num = (this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
							if (--this.match_length == 0)
								break;
						}
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (this.window[this.strstart] & 255);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[this.strstart + 1] & 255)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, this.window[this.strstart] & 255);
					this.lookahead--;
					this.strstart++;
				}
				if (!flag)
					continue;
				this.flush_block_only(false);
				if (this._codec.AvailableBytesOut != 0)
					continue;
				result = BlockState.NeedMore;
				break;
			}
			return result;
		}

		internal BlockState DeflateSlow(FlushType flush)
		{
			int num = 0;
			BlockState result;
			while (true)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						result = BlockState.NeedMore;
					}
					else
					{
						if (this.lookahead != 0)
							goto IL_004a;
						if (this.match_available != 0)
						{
							bool flag = this._tr_tally(0, this.window[this.strstart - 1] & 255);
							this.match_available = 0;
						}
						this.flush_block_only(flush == FlushType.Finish);
						result = (BlockState)((this._codec.AvailableBytesOut != 0) ? ((flush != FlushType.Finish) ? 1 : 3) : ((flush == FlushType.Finish) ? 2 : 0));
					}
					break;
				}
				goto IL_004a;
				IL_004a:
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
					num = (this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = DeflateManager.MIN_MATCH - 1;
				if (num != 0 && this.prev_length < this.config.MaxLazy && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					if (this.compressionStrategy != CompressionStrategy.HuffmanOnly)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.compressionStrategy == CompressionStrategy.Filtered || (this.match_length == DeflateManager.MIN_MATCH && this.strstart - this.match_start > 4096)))
					{
						this.match_length = DeflateManager.MIN_MATCH - 1;
					}
				}
				if (this.prev_length >= DeflateManager.MIN_MATCH && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - DeflateManager.MIN_MATCH;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					while (true)
					{
						if (++this.strstart <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
							num = (this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						if (--this.prev_length == 0)
							break;
					}
					this.match_available = 0;
					this.match_length = DeflateManager.MIN_MATCH - 1;
					this.strstart++;
					if (!flag)
						continue;
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut != 0)
						continue;
					result = BlockState.NeedMore;
					break;
				}
				if (this.match_available != 0)
				{
					if (this._tr_tally(0, this.window[this.strstart - 1] & 255))
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this._codec.AvailableBytesOut != 0)
						continue;
					result = BlockState.NeedMore;
					break;
				}
				this.match_available = 1;
				this.strstart++;
				this.lookahead--;
			}
			return result;
		}

		internal int longest_match(int cur_match)
		{
			int num = this.config.MaxChainLength;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - DeflateManager.MIN_LOOKAHEAD) ? (this.strstart - (this.w_size - DeflateManager.MIN_LOOKAHEAD)) : 0;
			int niceLength = this.config.NiceLength;
			int num5 = this.w_mask;
			int num6 = this.strstart + DeflateManager.MAX_MATCH;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.config.GoodLength)
			{
				num >>= 2;
			}
			if (niceLength > this.lookahead)
			{
				niceLength = this.lookahead;
			}
			do
			{
				int num7 = cur_match;
				if (this.window[num7 + num3] == b2 && this.window[num7 + num3 - 1] == b && this.window[num7] == this.window[num2] && this.window[++num7] == this.window[num2 + 1])
				{
					num2 += 2;
					num7++;
					do
					{
					}
					while (!(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(this.window[++num2] != this.window[++num7]) && !(num2 >= num6));
					int num8 = DeflateManager.MAX_MATCH - (num6 - num2);
					num2 = num6 - DeflateManager.MAX_MATCH;
					if (num8 > num3)
					{
						this.match_start = cur_match;
						num3 = num8;
						if (num8 < niceLength)
						{
							b = this.window[num2 + num3 - 1];
							b2 = this.window[num2 + num3];
							continue;
						}
						break;
					}
				}
			}
			while (!((cur_match = (this.prev[cur_match & num5] & 65535)) <= num4) && !(--num == 0));
			return (num3 > this.lookahead) ? this.lookahead : num3;
		}

		internal int Initialize(ZlibCodec codec, CompressionLevel level)
		{
			return this.Initialize(codec, level, 15);
		}

		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, CompressionStrategy.Default);
		}

		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, compressionStrategy);
		}

		internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
		{
			this._codec = codec;
			this._codec.Message = (string)null;
			if (windowBits >= 9 && windowBits <= 15)
			{
				if (memLevel >= 1 && memLevel <= DeflateManager.MEM_LEVEL_MAX)
				{
					this._codec.dstate = this;
					this.w_bits = windowBits;
					this.w_size = 1 << this.w_bits;
					this.w_mask = this.w_size - 1;
					this.hash_bits = memLevel + 7;
					this.hash_size = 1 << this.hash_bits;
					this.hash_mask = this.hash_size - 1;
					this.hash_shift = (this.hash_bits + DeflateManager.MIN_MATCH - 1) / DeflateManager.MIN_MATCH;
					this.window = new byte[this.w_size * 2];
					this.prev = new short[this.w_size];
					this.head = new short[this.hash_size];
					this.lit_bufsize = 1 << memLevel + 6;
					this.pending = new byte[this.lit_bufsize * 4];
					this._distanceOffset = this.lit_bufsize;
					this._lengthOffset = 3 * this.lit_bufsize;
					this.compressionLevel = level;
					this.compressionStrategy = strategy;
					this.Reset();
					return 0;
				}
				throw new ZlibException(string.Format("memLevel must be in the range 1.. {0}", DeflateManager.MEM_LEVEL_MAX));
			}
			throw new ZlibException("windowBits must be in the range 9..15.");
		}

		internal void Reset()
		{
			this._codec.TotalBytesIn = (this._codec.TotalBytesOut = 0L);
			this._codec.Message = (string)null;
			this.pendingCount = 0;
			this.nextPending = 0;
			this.Rfc1950BytesEmitted = false;
			this.status = ((!this.WantRfc1950HeaderBytes) ? DeflateManager.BUSY_STATE : DeflateManager.INIT_STATE);
			this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
			this.last_flush = 0;
			this._InitializeTreeData();
			this._InitializeLazyMatch();
		}

		internal int End()
		{
			int result;
			if (this.status != DeflateManager.INIT_STATE && this.status != DeflateManager.BUSY_STATE && this.status != DeflateManager.FINISH_STATE)
			{
				result = -2;
			}
			else
			{
				this.pending = null;
				this.head = null;
				this.prev = null;
				this.window = null;
				result = ((this.status == DeflateManager.BUSY_STATE) ? (-3) : 0);
			}
			return result;
		}

		private void SetDeflater()
		{
			switch (this.config.Flavor)
			{
			case DeflateFlavor.Store:
			{
				this.DeflateFunction = new CompressFunc(this.DeflateNone);
				break;
			}
			case DeflateFlavor.Fast:
			{
				this.DeflateFunction = new CompressFunc(this.DeflateFast);
				break;
			}
			case DeflateFlavor.Slow:
			{
				this.DeflateFunction = new CompressFunc(this.DeflateSlow);
				break;
			}
			}
		}

		internal int SetParams(CompressionLevel level, CompressionStrategy strategy)
		{
			int result = 0;
			if (this.compressionLevel != level)
			{
				Config config = Config.Lookup(level);
				if (config.Flavor != this.config.Flavor && this._codec.TotalBytesIn != 0)
				{
					result = this._codec.Deflate(FlushType.Partial);
				}
				this.compressionLevel = level;
				this.config = config;
				this.SetDeflater();
			}
			this.compressionStrategy = strategy;
			return result;
		}

		internal int SetDictionary(byte[] dictionary)
		{
			int num = dictionary.Length;
			int sourceIndex = 0;
			if (dictionary != null && this.status == DeflateManager.INIT_STATE)
			{
				this._codec._Adler32 = Adler.Adler32(this._codec._Adler32, dictionary, 0, dictionary.Length);
				int result;
				if (num < DeflateManager.MIN_MATCH)
				{
					result = 0;
				}
				else
				{
					if (num > this.w_size - DeflateManager.MIN_LOOKAHEAD)
					{
						num = this.w_size - DeflateManager.MIN_LOOKAHEAD;
						sourceIndex = dictionary.Length - num;
					}
					Array.Copy(dictionary, sourceIndex, this.window, 0, num);
					this.strstart = num;
					this.block_start = num;
					this.ins_h = (this.window[0] & 255);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[1] & 255)) & this.hash_mask);
					for (int i = 0; i <= num - DeflateManager.MIN_MATCH; i++)
					{
						this.ins_h = ((this.ins_h << this.hash_shift ^ (this.window[i + (DeflateManager.MIN_MATCH - 1)] & 255)) & this.hash_mask);
						this.prev[i & this.w_mask] = this.head[this.ins_h];
						this.head[this.ins_h] = (short)i;
					}
					result = 0;
				}
				return result;
			}
			throw new ZlibException("Stream error.");
		}

		internal int Deflate(FlushType flush)
		{
			int result;
			if (this._codec.OutputBuffer != null && (this._codec.InputBuffer != null || this._codec.AvailableBytesIn == 0) && (this.status != DeflateManager.FINISH_STATE || flush == FlushType.Finish))
			{
				if (this._codec.AvailableBytesOut == 0)
				{
					this._codec.Message = DeflateManager._ErrorMessage[7];
					throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
				}
				int num = this.last_flush;
				this.last_flush = (int)flush;
				if (this.status == DeflateManager.INIT_STATE)
				{
					int num2 = DeflateManager.Z_DEFLATED + (this.w_bits - 8 << 4) << 8;
					int num3 = ((int)(this.compressionLevel - 1) & 255) >> 1;
					if (num3 > 3)
					{
						num3 = 3;
					}
					num2 |= num3 << 6;
					if (this.strstart != 0)
					{
						num2 |= DeflateManager.PRESET_DICT;
					}
					num2 += 31 - num2 % 31;
					this.status = DeflateManager.BUSY_STATE;
					byte[] obj = this.pending;
					int num4 = this.pendingCount;
					int num5 = num4;
					this.pendingCount = num4 + 1;
					obj[num5] = (byte)(num2 >> 8);
					byte[] obj2 = this.pending;
					int num6 = this.pendingCount;
					num5 = num6;
					this.pendingCount = num6 + 1;
					obj2[num5] = (byte)num2;
					if (this.strstart != 0)
					{
						byte[] obj3 = this.pending;
						int num7 = this.pendingCount;
						num5 = num7;
						this.pendingCount = num7 + 1;
						obj3[num5] = (byte)((uint)((int)this._codec._Adler32 & -16777216) >> 24);
						byte[] obj4 = this.pending;
						int num8 = this.pendingCount;
						num5 = num8;
						this.pendingCount = num8 + 1;
						obj4[num5] = (byte)((this._codec._Adler32 & 16711680) >> 16);
						byte[] obj5 = this.pending;
						int num9 = this.pendingCount;
						num5 = num9;
						this.pendingCount = num9 + 1;
						obj5[num5] = (byte)((this._codec._Adler32 & 65280) >> 8);
						byte[] obj6 = this.pending;
						int num10 = this.pendingCount;
						num5 = num10;
						this.pendingCount = num10 + 1;
						obj6[num5] = (byte)(this._codec._Adler32 & 255);
					}
					this._codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
				}
				if (this.pendingCount != 0)
				{
					this._codec.flush_pending();
					if (this._codec.AvailableBytesOut == 0)
					{
						this.last_flush = -1;
						result = 0;
						goto IL_04e3;
					}
				}
				else if (this._codec.AvailableBytesIn == 0 && (int)flush <= num && flush != FlushType.Finish)
				{
					result = 0;
					goto IL_04e3;
				}
				if (((this.status == DeflateManager.FINISH_STATE) ? this._codec.AvailableBytesIn : 0) != 0)
				{
					this._codec.Message = DeflateManager._ErrorMessage[7];
					throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
				}
				if (this._codec.AvailableBytesIn != 0 || this.lookahead != 0 || (flush != 0 && this.status != DeflateManager.FINISH_STATE))
				{
					BlockState blockState = this.DeflateFunction(flush);
					if (blockState == BlockState.FinishStarted || blockState == BlockState.FinishDone)
					{
						this.status = DeflateManager.FINISH_STATE;
					}
					switch (blockState)
					{
					case BlockState.NeedMore:
					case BlockState.FinishStarted:
					{
						if (this._codec.AvailableBytesOut == 0)
						{
							this.last_flush = -1;
						}
						result = 0;
						goto IL_04e3;
					}
					case BlockState.BlockDone:
					{
						if (flush == FlushType.Partial)
						{
							this._tr_align();
						}
						else
						{
							this._tr_stored_block(0, 0, false);
							if (flush == FlushType.Full)
							{
								for (int i = 0; i < this.hash_size; i++)
								{
									this.head[i] = (short)0;
								}
							}
						}
						this._codec.flush_pending();
						if (this._codec.AvailableBytesOut == 0)
						{
							this.last_flush = -1;
							result = 0;
							goto IL_04e3;
						}
						break;
					}
					}
				}
				if (flush != FlushType.Finish)
				{
					result = 0;
				}
				else if (!this.WantRfc1950HeaderBytes || this.Rfc1950BytesEmitted)
				{
					result = 1;
				}
				else
				{
					byte[] obj7 = this.pending;
					int num11 = this.pendingCount;
					int num5 = num11;
					this.pendingCount = num11 + 1;
					obj7[num5] = (byte)((uint)((int)this._codec._Adler32 & -16777216) >> 24);
					byte[] obj8 = this.pending;
					int num12 = this.pendingCount;
					num5 = num12;
					this.pendingCount = num12 + 1;
					obj8[num5] = (byte)((this._codec._Adler32 & 16711680) >> 16);
					byte[] obj9 = this.pending;
					int num13 = this.pendingCount;
					num5 = num13;
					this.pendingCount = num13 + 1;
					obj9[num5] = (byte)((this._codec._Adler32 & 65280) >> 8);
					byte[] obj10 = this.pending;
					int num14 = this.pendingCount;
					num5 = num14;
					this.pendingCount = num14 + 1;
					obj10[num5] = (byte)(this._codec._Adler32 & 255);
					this._codec.flush_pending();
					this.Rfc1950BytesEmitted = true;
					result = ((this.pendingCount == 0) ? 1 : 0);
				}
				goto IL_04e3;
			}
			this._codec.Message = DeflateManager._ErrorMessage[4];
			throw new ZlibException(string.Format("Something is fishy. [{0}]", this._codec.Message));
			IL_04e3:
			return result;
		}
	}
}
