using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000302 RID: 770
	public class HistoryAutoRecorderWorker_WealthTameAnimals : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CCA RID: 3274 RVA: 0x000705CC File Offset: 0x0006E9CC
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthTameAnimals;
				}
			}
			return num;
		}
	}
}
