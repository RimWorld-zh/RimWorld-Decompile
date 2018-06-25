using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000300 RID: 768
	public class HistoryAutoRecorderWorker_WealthBuildings : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x000704FC File Offset: 0x0006E8FC
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthBuildings;
				}
			}
			return num;
		}
	}
}
