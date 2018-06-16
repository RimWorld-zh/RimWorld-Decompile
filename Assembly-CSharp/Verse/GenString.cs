using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F4A RID: 3914
	public static class GenString
	{
		// Token: 0x06005E88 RID: 24200 RVA: 0x003011D8 File Offset: 0x002FF5D8
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x06005E89 RID: 24201 RVA: 0x0030122C File Offset: 0x002FF62C
		public static string ToStringCached(this int num)
		{
			string result;
			if (num < -4999)
			{
				result = num.ToString();
			}
			else if (num > 4999)
			{
				result = num.ToString();
			}
			else
			{
				result = GenString.numberStrings[num + 5000];
			}
			return result;
		}

		// Token: 0x06005E8A RID: 24202 RVA: 0x0030128C File Offset: 0x002FF68C
		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (str.NullOrEmpty())
			{
				yield break;
			}
			if (chunkLength < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength > str.Length - i)
				{
					chunkLength = str.Length - i;
				}
				yield return str.Substring(i, chunkLength);
			}
			yield break;
		}

		// Token: 0x04003E1C RID: 15900
		private static string[] numberStrings = new string[10000];
	}
}
