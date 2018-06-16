using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000300 RID: 768
	public class HistoryAutoRecorderWorker_WealthTameAnimals : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC7 RID: 3271 RVA: 0x000703C0 File Offset: 0x0006E7C0
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
