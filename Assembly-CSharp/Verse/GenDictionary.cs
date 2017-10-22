using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	public static class GenDictionary
	{
		public static string ToStringFullContents<K, V>(this Dictionary<K, V> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<K, V>.Enumerator enumerator = dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<K, V> current = enumerator.Current;
					stringBuilder.AppendLine(current.Key.ToString() + ": " + current.Value.ToString());
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return stringBuilder.ToString();
		}
	}
}
