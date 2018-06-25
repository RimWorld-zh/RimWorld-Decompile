using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	// Token: 0x02000F11 RID: 3857
	public class DebugLogsUtility
	{
		// Token: 0x06005C96 RID: 23702 RVA: 0x002F03F0 File Offset: 0x002EE7F0
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
	}
}
