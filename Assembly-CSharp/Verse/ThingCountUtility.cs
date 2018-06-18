using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EFF RID: 3839
	public static class ThingCountUtility
	{
		// Token: 0x06005BF8 RID: 23544 RVA: 0x002EBDE4 File Offset: 0x002EA1E4
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

		// Token: 0x06005BF9 RID: 23545 RVA: 0x002EBE40 File Offset: 0x002EA240
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
