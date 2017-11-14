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
			foreach (KeyValuePair<ThingDef, int> item in from k in dictionary
			orderby k.Value descending
			select k)
			{
				stringBuilder.AppendLine(item.Key + " - " + item.Value);
			}
			return stringBuilder.ToString();
		}
	}
}
