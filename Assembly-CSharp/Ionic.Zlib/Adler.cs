namespace Ionic.Zlib
{
	public sealed class Adler
	{
		private static readonly uint BASE = 65521u;

		private static readonly int NMAX = 5552;

		public static uint Adler32(uint adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1u;
			}
			uint num = adler & 0xFFFF;
			uint num2 = adler >> 16 & 0xFFFF;
			while (len > 0)
			{
				int num3 = (len >= Adler.NMAX) ? Adler.NMAX : len;
				len -= num3;
				while (num3 >= 16)
				{
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num += buf[index++];
					num2 += num;
					num3 -= 16;
				}
				if (num3 != 0)
				{
					while (true)
					{
						num += buf[index++];
						num2 += num;
						if (--num3 == 0)
							break;
					}
				}
				num %= Adler.BASE;
				num2 %= Adler.BASE;
			}
			return num2 << 16 | num;
		}
	}
}
