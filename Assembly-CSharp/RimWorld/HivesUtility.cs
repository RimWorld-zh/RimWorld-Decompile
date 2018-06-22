using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C6 RID: 1734
	public static class HivesUtility
	{
		// Token: 0x0600259B RID: 9627 RVA: 0x00142574 File Offset: 0x00140974
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}
	}
}
