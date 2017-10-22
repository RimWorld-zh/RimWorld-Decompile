namespace Ionic.Zlib
{
	public sealed class Adler
	{
		private static readonly uint BASE = 65521u;

		private static readonly int NMAX = 5552;

		public static uint Adler32(uint adler, byte[] buf, int index, int len)
		{
			uint result;
			if (buf == null)
			{
				result = 1u;
			}
			else
			{
				uint num = adler & 65535;
				uint num2 = adler >> 16 & 65535;
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
				result = (num2 << 16 | num);
			}
			return result;
		}
	}
}
