using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000831 RID: 2097
	public static class PageUtility
	{
		// Token: 0x06002F3E RID: 12094 RVA: 0x00193BC8 File Offset: 0x00191FC8
		public static Page StitchedPages(IEnumerable<Page> pages)
		{
			List<Page> list = pages.ToList<Page>();
			Page result;
			if (list.Count == 0)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (i > 0)
					{
						list[i].prev = list[i - 1];
					}
					if (i < list.Count - 1)
					{
						list[i].next = list[i + 1];
					}
				}
				result = list[0];
			}
			return result;
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x00193C58 File Offset: 0x00192058
		public static void InitGameStart()
		{
			Action preLoadLevelAction = delegate()
			{
				Find.GameInitData.PrepForMapGen();
				Find.GameInitData.startedFromEntry = true;
				Find.Scenario.PreMapGenerate();
			};
			LongEventHandler.QueueLongEvent(preLoadLevelAction, "Play", "GeneratingMap", true, null);
		}
	}
}
