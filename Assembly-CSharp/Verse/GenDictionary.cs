using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000F43 RID: 3907
	public static class GenDictionary
	{
		// Token: 0x06005E3C RID: 24124 RVA: 0x002FDB00 File Offset: 0x002FBF00
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
