using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C8 RID: 1736
	public static class HivesUtility
	{
		// Token: 0x0600259F RID: 9631 RVA: 0x001426C4 File Offset: 0x00140AC4
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}
	}
}
