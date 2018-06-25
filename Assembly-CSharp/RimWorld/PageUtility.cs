using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class PageUtility
	{
		[CompilerGenerated]
		private static Action <>f__am$cache0;

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

		[CompilerGenerated]
		private static void <InitGameStart>m__0()
		{
			Find.GameInitData.PrepForMapGen();
			Find.GameInitData.startedFromEntry = true;
			Find.Scenario.PreMapGenerate();
		}
	}
}
