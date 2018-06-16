using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000831 RID: 2097
	public static class PageUtility
	{
		// Token: 0x06002F3C RID: 12092 RVA: 0x00193B34 File Offset: 0x00191F34
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

		// Token: 0x06002F3D RID: 12093 RVA: 0x00193BC4 File Offset: 0x00191FC4
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
