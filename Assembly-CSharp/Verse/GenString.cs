using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F49 RID: 3913
	public static class GenString
	{
		// Token: 0x06005E86 RID: 24198 RVA: 0x003012B4 File Offset: 0x002FF6B4
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x06005E87 RID: 24199 RVA: 0x00301308 File Offset: 0x002FF708
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

		// Token: 0x06005E88 RID: 24200 RVA: 0x00301368 File Offset: 0x002FF768
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

		// Token: 0x04003E1B RID: 15899
		private static string[] numberStrings = new string[10000];
	}
}
