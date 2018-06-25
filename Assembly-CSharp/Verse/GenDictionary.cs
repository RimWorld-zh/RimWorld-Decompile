using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000F47 RID: 3911
	public static class GenDictionary
	{
		// Token: 0x06005E6C RID: 24172 RVA: 0x003004B8 File Offset: 0x002FE8B8
		public static string ToStringFullContents<K, V>(this Dictionary<K, V> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<K, V> keyValuePair in dict)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				K key = keyValuePair.Key;
				string str = key.ToString();
				string str2 = ": ";
				V value = keyValuePair.Value;
				stringBuilder2.AppendLine(str + str2 + value.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
