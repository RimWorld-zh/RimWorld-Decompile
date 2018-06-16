using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F00 RID: 3840
	public static class ThingCountUtility
	{
		// Token: 0x06005BFA RID: 23546 RVA: 0x002EBD08 File Offset: 0x002EA108
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

		// Token: 0x06005BFB RID: 23547 RVA: 0x002EBD64 File Offset: 0x002EA164
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
