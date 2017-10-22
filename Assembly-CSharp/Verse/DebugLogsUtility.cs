using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	public class DebugLogsUtility
	{
		public static string ThingListToUniqueCountString(IEnumerable<Thing> things)
		{
			if (things == null)
			{
				return "null";
			}
			Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
			foreach (Thing item in things)
			{
				if (!dictionary.ContainsKey(item.def))
				{
					dictionary.Add(item.def, 0);
				}
				Dictionary<ThingDef, int> dictionary2;
				Dictionary<ThingDef, int> obj = dictionary2 = dictionary;
				ThingDef def;
				ThingDef key = def = item.def;
				int num = dictionary2[def];
				obj[key] = num + 1;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Registered things in dynamic draw list:");
			foreach (KeyValuePair<ThingDef, int> item2 in from k in dictionary
			orderby k.Value descending
			select k)
			{
				stringBuilder.AppendLine(item2.Key + " - " + item2.Value);
			}
			return stringBuilder.ToString();
		}
	}
}
