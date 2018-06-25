using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Verse
{
	public class DebugLogsUtility
	{
		[CompilerGenerated]
		private static Func<KeyValuePair<ThingDef, int>, int> <>f__am$cache0;

		public DebugLogsUtility()
		{
		}

		public static string ThingListToUniqueCountString(IEnumerable<Thing> things)
		{
			string result;
			if (things == null)
			{
				result = "null";
			}
			else
			{
				Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
				foreach (Thing thing in things)
				{
					if (!dictionary.ContainsKey(thing.def))
					{
						dictionary.Add(thing.def, 0);
					}
					Dictionary<ThingDef, int> dictionary2;
					ThingDef def;
					(dictionary2 = dictionary)[def = thing.def] = dictionary2[def] + 1;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Registered things in dynamic draw list:");
				foreach (KeyValuePair<ThingDef, int> keyValuePair in from k in dictionary
				orderby k.Value descending
				select k)
				{
					stringBuilder.AppendLine(keyValuePair.Key + " - " + keyValuePair.Value);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		[CompilerGenerated]
		private static int <ThingListToUniqueCountString>m__0(KeyValuePair<ThingDef, int> k)
		{
			return k.Value;
		}
	}
}
