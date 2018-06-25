using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F4D RID: 3917
	public static class GenString
	{
		// Token: 0x04003E30 RID: 15920
		private static string[] numberStrings = new string[10000];

		// Token: 0x06005EB8 RID: 24248 RVA: 0x00303970 File Offset: 0x00301D70
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x003039C4 File Offset: 0x00301DC4
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

		// Token: 0x06005EBA RID: 24250 RVA: 0x00303A24 File Offset: 0x00301E24
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
	}
}
