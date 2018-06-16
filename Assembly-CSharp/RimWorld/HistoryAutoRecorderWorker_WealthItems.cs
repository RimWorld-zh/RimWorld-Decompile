using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FF RID: 767
	public class HistoryAutoRecorderWorker_WealthItems : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CC5 RID: 3269 RVA: 0x00070358 File Offset: 0x0006E758
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
