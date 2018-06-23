using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x02000002 RID: 2
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class CRC32
	{
		// Token: 0x04000001 RID: 1
		private uint dwPolynomial;

		// Token: 0x04000002 RID: 2
		private long _TotalBytesRead;

		// Token: 0x04000003 RID: 3
		private bool reverseBits;

		// Token: 0x04000004 RID: 4
		private uint[] crc32Table;

		// Token: 0x04000005 RID: 5
		private const int BUFFER_SIZE = 8192;

		// Token: 0x04000006 RID: 6
		private uint _register = uint.MaxValue;

		// Token: 0x06000001 RID: 1 RVA: 0x00002243 File Offset: 0x00000643
		public CRC32() : this(false)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000224D File Offset: 0x0000064D
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000225C File Offset: 0x0000065C
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002280 File Offset: 0x00000680
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000229C File Offset: 0x0000069C
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022B8 File Offset: 0x000006B8
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022D8 File Offset: 0x000006D8
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, count);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002388 File Offset: 0x00000788
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023A8 File Offset: 0x000007A8
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((UIntPtr)((W ^ (uint)B) & 255u))] ^ W >> 8);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023D4 File Offset: 0x000007D4
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
				}
				else
				{
					uint num3 = (this._register & 255u) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num3)]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002480 File Offset: 0x00000880
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num)]);
			}
			else
			{
				uint num2 = (this._register & 255u) ^ (uint)b;
				this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024F0 File Offset: 0x000008F0
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)((num < 0u) ? (num + 256u) : num))]);
				}
				else
				{
					uint num2 = (this._register & 255u) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)((num2 < 0u) ? (num2 + 256u) : num2))]);
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002598 File Offset: 0x00000998
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765u) << 1 | (data >> 1 & 1431655765u);
			num = ((num & 858993459u) << 2 | (num >> 2 & 858993459u));
			num = ((num & 252645135u) << 4 | (num >> 4 & 252645135u));
			return num << 24 | (num & 65280u) << 8 | (num >> 8 & 65280u) | num >> 24;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000260C File Offset: 0x00000A0C
		private static byte ReverseBits(byte data)
		{
			uint num = (uint)data * 131586u;
			uint num2 = 17055760u;
			uint num3 = num & num2;
			uint num4 = num << 2 & num2 << 1;
			return (byte)(16781313u * (num3 + num4) >> 24);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000264C File Offset: 0x00000A4C
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1u) == 1u)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000026E4 File Offset: 0x00000AE4
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0u;
			int num2 = 0;
			while (vec != 0u)
			{
				if ((vec & 1u) == 1u)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002724 File Offset: 0x00000B24
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002754 File Offset: 0x00000B54
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length != 0)
			{
				uint num = ~this._register;
				array2[0] = this.dwPolynomial;
				uint num2 = 1u;
				for (int i = 1; i < 32; i++)
				{
					array2[i] = num2;
					num2 <<= 1;
				}
				this.gf2_matrix_square(array, array2);
				this.gf2_matrix_square(array2, array);
				uint num3 = (uint)length;
				do
				{
					this.gf2_matrix_square(array, array2);
					if ((num3 & 1u) == 1u)
					{
						num = this.gf2_matrix_times(array, num);
					}
					num3 >>= 1;
					if (num3 == 0u)
					{
						break;
					}
					this.gf2_matrix_square(array2, array);
					if ((num3 & 1u) == 1u)
					{
						num = this.gf2_matrix_times(array2, num);
					}
					num3 >>= 1;
				}
				while (num3 != 0u);
				num ^= (uint)crc;
				this._register = ~num;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002833 File Offset: 0x00000C33
		public void Reset()
		{
			this._register = uint.MaxValue;
		}
	}
}
