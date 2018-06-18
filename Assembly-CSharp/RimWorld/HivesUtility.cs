using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CA RID: 1738
	public static class HivesUtility
	{
		// Token: 0x060025A3 RID: 9635 RVA: 0x00142428 File Offset: 0x00140828
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}
	}
}
