using System;

namespace Ionic.Zlib
{
	// Token: 0x0200001E RID: 30
	public sealed class Adler
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000B40C File Offset: 0x0000980C
		public static uint Adler32(uint adler, byte[] buf, int index, int len)
		{
			uint result;
			if (buf == null)
			{
				result = 1u;
			}
			else
			{
				uint num = adler & 65535u;
				uint num2 = adler >> 16 & 65535u;
				while (len > 0)
				{
					int i = (len >= Adler.NMAX) ? Adler.NMAX : len;
					len -= i;
					while (i >= 16)
					{
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						num += (uint)buf[index++];
						num2 += num;
						i -= 16;
					}
					if (i != 0)
					{
						do
						{
							num += (uint)buf[index++];
							num2 += num;
						}
						while (--i != 0);
					}
					num %= Adler.BASE;
					num2 %= Adler.BASE;
				}
				result = (num2 << 16 | num);
			}
			return result;
		}

		// Token: 0x04000154 RID: 340
		private static readonly uint BASE = 65521u;

		// Token: 0x04000155 RID: 341
		private static readonly int NMAX = 5552;
	}
}
