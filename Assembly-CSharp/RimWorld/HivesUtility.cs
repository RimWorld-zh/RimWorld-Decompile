using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CA RID: 1738
	public static class HivesUtility
	{
		// Token: 0x060025A1 RID: 9633 RVA: 0x001423B0 File Offset: 0x001407B0
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}
	}
}
