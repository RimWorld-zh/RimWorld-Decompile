using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C8 RID: 1736
	public static class HivesUtility
	{
		// Token: 0x0600259E RID: 9630 RVA: 0x00142924 File Offset: 0x00140D24
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}
	}
}
