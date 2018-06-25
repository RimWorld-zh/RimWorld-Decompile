using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000303 RID: 771
	public class HistoryAutoRecorderWorker_WealthTotal : HistoryAutoRecorderWorker
	{
		// Token: 0x06000CCD RID: 3277 RVA: 0x0007062C File Offset: 0x0006EA2C
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
