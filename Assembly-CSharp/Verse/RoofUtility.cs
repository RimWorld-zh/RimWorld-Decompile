using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CA2 RID: 3234
	public static class RoofUtility
	{
		// Token: 0x06004746 RID: 18246 RVA: 0x00259AF0 File Offset: 0x00257EF0
		public static Thing FirstBlockingThing(IntVec3 pos, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(pos);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.plant != null && list[i].def.plant.interferesWithRoof)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x00259B6C File Offset: 0x00257F6C
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			bool result;
			if (blocker == null)
			{
				result = true;
			}
			else
			{
				if (blocker.def.category == ThingCategory.Plant)
				{
					LocalTargetInfo target = blocker;
					PathEndMode peMode = PathEndMode.ClosestTouch;
					Danger maxDanger = worker.NormalMaxDanger();
					if (worker.CanReserveAndReach(target, peMode, maxDanger, 1, -1, null, forced))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x00259BD4 File Offset: 0x00257FD4
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			Job result;
			if (blocker == null)
			{
				result = null;
			}
			else
			{
				if (blocker.def.category == ThingCategory.Plant)
				{
					LocalTargetInfo target = blocker;
					PathEndMode peMode = PathEndMode.ClosestTouch;
					Danger maxDanger = worker.NormalMaxDanger();
					if (worker.CanReserveAndReach(target, peMode, maxDanger, 1, -1, null, forced))
					{
						return new Job(JobDefOf.CutPlant, blocker);
					}
				}
				result = null;
			}
			return result;
		}
	}
}
