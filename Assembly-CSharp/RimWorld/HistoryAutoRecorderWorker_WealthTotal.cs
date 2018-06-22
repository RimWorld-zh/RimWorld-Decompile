using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000301 RID: 769
	public class HistoryAutoRecorderWorker_WealthTotal : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x000704DC File Offset: 0x0006E8DC
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthTotal;
				}
			}
			return num;
		}
	}
}
