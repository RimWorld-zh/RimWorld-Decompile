using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	// Token: 0x02000F0D RID: 3853
	public class DebugLogsUtility
	{
		// Token: 0x06005C8C RID: 23692 RVA: 0x002EFD70 File Offset: 0x002EE170
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
