using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082D RID: 2093
	public static class PageUtility
	{
		// Token: 0x06002F37 RID: 12087 RVA: 0x00193DA8 File Offset: 0x001921A8
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

		// Token: 0x06002F38 RID: 12088 RVA: 0x00193E38 File Offset: 0x00192238
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
