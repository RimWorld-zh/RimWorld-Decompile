using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x0200001B RID: 27
	internal class SharedUtils
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x0000B218 File Offset: 0x00009618
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000B234 File Offset: 0x00009634
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			int result;
			if (target.Length == 0)
			{
				result = 0;
			}
			else
			{
				char[] array = new char[target.Length];
				int num = sourceTextReader.Read(array, start, count);
				if (num == 0)
				{
					result = -1;
				}
				else
				{
					for (int i = start; i < start + num; i++)
					{
						target[i] = (byte)array[i];
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000B294 File Offset: 0x00009694
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000B2B4 File Offset: 0x000096B4
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
