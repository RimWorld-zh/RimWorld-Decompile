using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

// Token: 0x020006EC RID: 1772
public static class SkyfallerUtility
{
	// Token: 0x06002696 RID: 9878 RVA: 0x0014A274 File Offset: 0x00148674
	public static bool CanPossiblyFallOnColonist(ThingDef skyfaller, IntVec3 c, Map map)
	{
		CellRect cellRect = GenAdj.OccupiedRect(c, Rot4.North, skyfaller.size);
		int dist = Mathf.Max(Mathf.CeilToInt(skyfaller.skyfaller.explosionRadius) + 7, 14);
		CellRect.CellRectIterator iterator = cellRect.ExpandedBy(dist).GetIterator();
		while (!iterator.Done())
		{
			IntVec3 c2 = iterator.Current;
			if (c2.InBounds(map))
			{
				List<Thing> thingList = c2.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						return true;
					}
				}
			}
			iterator.MoveNext();
		}
		return false;
	}
}
