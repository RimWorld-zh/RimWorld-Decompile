using System;

namespace Ionic.Zlib
{
	internal sealed class Tree
	{
		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		internal static readonly int[] ExtraLengthBits = new int[29]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			4,
			4,
			4,
			4,
			5,
			5,
			5,
			5,
			0
		};

		internal static readonly int[] ExtraDistanceBits = new int[30]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			13,
			13
		};

		internal static readonly int[] extra_blbits = new int[19]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			2,
			3,
			7
		};

		internal static readonly sbyte[] bl_order = new sbyte[19]
		{
			(sbyte)16,
			(sbyte)17,
			(sbyte)18,
			(sbyte)0,
			(sbyte)8,
			(sbyte)7,
			(sbyte)9,
			(sbyte)6,
			(sbyte)10,
			(sbyte)5,
			(sbyte)11,
			(sbyte)4,
			(sbyte)12,
			(sbyte)3,
			(sbyte)13,
			(sbyte)2,
			(sbyte)14,
			(sbyte)1,
			(sbyte)15
		};

		internal const int Buf_size = 16;

		private static readonly sbyte[] _dist_code = new sbyte[512]
		{
			(sbyte)0,
			(sbyte)1,
			(sbyte)2,
			(sbyte)3,
			(sbyte)4,
			(sbyte)4,
			(sbyte)5,
			(sbyte)5,
			(sbyte)6,
			(sbyte)6,
			(sbyte)6,
			(sbyte)6,
			(sbyte)7,
			(sbyte)7,
			(sbyte)7,
			(sbyte)7,
			(sbyte)8,
			(sbyte)8,
			(sbyte)8,
			(sbyte)8,
			(sbyte)8,
			(sbyte)8,
			(sbyte)8,
			(sbyte)8,
			(sbyte)9,
			(sbyte)9,
			(sbyte)9,
			(sbyte)9,
			(sbyte)9,
			(sbyte)9,
			(sbyte)9,
			(sbyte)9,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)10,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)11,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)0,
			(sbyte)0,
			(sbyte)16,
			(sbyte)17,
			(sbyte)18,
			(sbyte)18,
			(sbyte)19,
			(sbyte)19,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)28,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29,
			(sbyte)29
		};

		internal static readonly sbyte[] LengthCode = new sbyte[256]
		{
			(sbyte)0,
			(sbyte)1,
			(sbyte)2,
			(sbyte)3,
			(sbyte)4,
			(sbyte)5,
			(sbyte)6,
			(sbyte)7,
			(sbyte)8,
			(sbyte)8,
			(sbyte)9,
			(sbyte)9,
			(sbyte)10,
			(sbyte)10,
			(sbyte)11,
			(sbyte)11,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)12,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)13,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)14,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)15,
			(sbyte)16,
			(sbyte)16,
			(sbyte)16,
			(sbyte)16,
			(sbyte)16,
			(sbyte)16,
			(sbyte)16,
			(sbyte)16,
			(sbyte)17,
			(sbyte)17,
			(sbyte)17,
			(sbyte)17,
			(sbyte)17,
			(sbyte)17,
			(sbyte)17,
			(sbyte)17,
			(sbyte)18,
			(sbyte)18,
			(sbyte)18,
			(sbyte)18,
			(sbyte)18,
			(sbyte)18,
			(sbyte)18,
			(sbyte)18,
			(sbyte)19,
			(sbyte)19,
			(sbyte)19,
			(sbyte)19,
			(sbyte)19,
			(sbyte)19,
			(sbyte)19,
			(sbyte)19,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)20,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)21,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)22,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)23,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)24,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)25,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)26,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)27,
			(sbyte)28
		};

		internal static readonly int[] LengthBase = new int[29]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			10,
			12,
			14,
			16,
			20,
			24,
			28,
			32,
			40,
			48,
			56,
			64,
			80,
			96,
			112,
			128,
			160,
			192,
			224,
			0
		};

		internal static readonly int[] DistanceBase = new int[30]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			12,
			16,
			24,
			32,
			48,
			64,
			96,
			128,
			192,
			256,
			384,
			512,
			768,
			1024,
			1536,
			2048,
			3072,
			4096,
			6144,
			8192,
			12288,
			16384,
			24576
		};

		internal short[] dyn_tree;

		internal int max_code;

		internal StaticTree staticTree;

		internal static int DistanceCode(int dist)
		{
			return (dist >= 256) ? Tree._dist_code[256 + SharedUtils.URShift(dist, 7)] : Tree._dist_code[dist];
		}

		internal void gen_bitlen(DeflateManager s)
		{
			short[] array = this.dyn_tree;
			short[] treeCodes = this.staticTree.treeCodes;
			int[] extraBits = this.staticTree.extraBits;
			int extraBase = this.staticTree.extraBase;
			int maxLength = this.staticTree.maxLength;
			int num = 0;
			for (int i = 0; i <= InternalConstants.MAX_BITS; i++)
			{
				s.bl_count[i] = (short)0;
			}
			array[s.heap[s.heap_max] * 2 + 1] = (short)0;
			int j;
			for (j = s.heap_max + 1; j < Tree.HEAP_SIZE; j++)
			{
				int num2 = s.heap[j];
				int i = array[array[num2 * 2 + 1] * 2 + 1] + 1;
				if (i > maxLength)
				{
					i = maxLength;
					num++;
				}
				array[num2 * 2 + 1] = (short)i;
				if (num2 <= this.max_code)
				{
					ref short val = ref s.bl_count[i];
					val = (short)(val + 1);
					int num3 = 0;
					if (num2 >= extraBase)
					{
						num3 = extraBits[num2 - extraBase];
					}
					short num4 = array[num2 * 2];
					s.opt_len += num4 * (i + num3);
					if (treeCodes != null)
					{
						s.static_len += num4 * (treeCodes[num2 * 2 + 1] + num3);
					}
				}
			}
			if (num != 0)
			{
				while (true)
				{
					int i = maxLength - 1;
					while (s.bl_count[i] == 0)
					{
						i--;
					}
					ref short val2 = ref s.bl_count[i];
					val2 = (short)(val2 - 1);
					s.bl_count[i + 1] = (short)(s.bl_count[i + 1] + 2);
					ref short val3 = ref s.bl_count[maxLength];
					val3 = (short)(val3 - 1);
					num -= 2;
					if (num <= 0)
						break;
				}
				for (int i = maxLength; i != 0; i--)
				{
					int num2 = s.bl_count[i];
					while (num2 != 0)
					{
						int num5 = s.heap[--j];
						if (num5 <= this.max_code)
						{
							if (array[num5 * 2 + 1] != i)
							{
								s.opt_len = (int)(s.opt_len + ((long)i - (long)array[num5 * 2 + 1]) * array[num5 * 2]);
								array[num5 * 2 + 1] = (short)i;
							}
							num2--;
						}
					}
				}
			}
		}

		internal void build_tree(DeflateManager s)
		{
			short[] array = this.dyn_tree;
			short[] treeCodes = this.staticTree.treeCodes;
			int elems = this.staticTree.elems;
			int num = -1;
			s.heap_len = 0;
			s.heap_max = Tree.HEAP_SIZE;
			for (int num2 = 0; num2 < elems; num2++)
			{
				if (array[num2 * 2] != 0)
				{
					num = (s.heap[++s.heap_len] = num2);
					s.depth[num2] = (sbyte)0;
				}
				else
				{
					array[num2 * 2 + 1] = (short)0;
				}
			}
			int num6;
			while (s.heap_len < 2)
			{
				int[] heap = s.heap;
				int num3 = ++s.heap_len;
				int num4 = (num < 2) ? (++num) : 0;
				int num5 = num4;
				heap[num3] = num4;
				num6 = num5;
				array[num6 * 2] = (short)1;
				s.depth[num6] = (sbyte)0;
				s.opt_len--;
				if (treeCodes != null)
				{
					s.static_len -= (int)treeCodes[num6 * 2 + 1];
				}
			}
			this.max_code = num;
			for (int num2 = s.heap_len / 2; num2 >= 1; num2--)
			{
				s.pqdownheap(array, num2);
			}
			num6 = elems;
			while (true)
			{
				int num2 = s.heap[1];
				int[] heap2 = s.heap;
				int[] heap3 = s.heap;
				int heap_len = s.heap_len;
				int num5 = heap_len;
				s.heap_len = heap_len - 1;
				heap2[1] = heap3[num5];
				s.pqdownheap(array, 1);
				int num7 = s.heap[1];
				s.heap[--s.heap_max] = num2;
				s.heap[--s.heap_max] = num7;
				array[num6 * 2] = (short)(array[num2 * 2] + array[num7 * 2]);
				s.depth[num6] = (sbyte)(Math.Max((byte)s.depth[num2], (byte)s.depth[num7]) + 1);
				array[num2 * 2 + 1] = (array[num7 * 2 + 1] = (short)num6);
				s.heap[1] = num6++;
				s.pqdownheap(array, 1);
				if (s.heap_len < 2)
					break;
			}
			s.heap[--s.heap_max] = s.heap[1];
			this.gen_bitlen(s);
			Tree.gen_codes(array, num, s.bl_count);
		}

		internal static void gen_codes(short[] tree, int max_code, short[] bl_count)
		{
			short[] array = new short[InternalConstants.MAX_BITS + 1];
			short num = (short)0;
			for (int i = 1; i <= InternalConstants.MAX_BITS; i++)
			{
				num = (array[i] = (short)(num + bl_count[i - 1] << 1));
			}
			for (int num2 = 0; num2 <= max_code; num2++)
			{
				int num3 = tree[num2 * 2 + 1];
				if (num3 != 0)
				{
					int num4 = num2 * 2;
					ref short val = ref array[num3];
					short num5 = val;
					short code = num5;
					val = (short)(num5 + 1);
					tree[num4] = (short)Tree.bi_reverse(code, num3);
				}
			}
		}

		internal static int bi_reverse(int code, int len)
		{
			int num = 0;
			while (true)
			{
				num |= (code & 1);
				code >>= 1;
				num <<= 1;
				if (--len <= 0)
					break;
			}
			return num >> 1;
		}
	}
}
