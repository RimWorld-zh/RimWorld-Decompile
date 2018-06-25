using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F03 RID: 3843
	public static class ThingCountUtility
	{
		// Token: 0x06005C2A RID: 23594 RVA: 0x002EE498 File Offset: 0x002EC898
		public static int CountOf(List<ThingCount> list, Thing thing)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					num += list[i].Count;
				}
			}
			return num;
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x002EE4F4 File Offset: 0x002EC8F4
		public static void AddToList(List<ThingCount> list, Thing thing, int countToAdd)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					list[i] = list[i].WithCount(list[i].Count + countToAdd);
					return;
				}
			}
			list.Add(new ThingCount(thing, countToAdd));
		}
	}
}
