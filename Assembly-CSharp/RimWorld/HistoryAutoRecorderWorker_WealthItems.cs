using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000301 RID: 769
	public class HistoryAutoRecorderWorker_WealthItems : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x00070564 File Offset: 0x0006E964
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthItems;
				}
			}
			return num;
		}
	}
}
