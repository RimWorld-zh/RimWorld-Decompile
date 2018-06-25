using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082F RID: 2095
	public static class PageUtility
	{
		// Token: 0x06002F3B RID: 12091 RVA: 0x00193EF8 File Offset: 0x001922F8
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

		// Token: 0x06002F3C RID: 12092 RVA: 0x00193F88 File Offset: 0x00192388
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
