using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CA0 RID: 3232
	public static class RoofUtility
	{
		// Token: 0x06004743 RID: 18243 RVA: 0x00259A14 File Offset: 0x00257E14
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

		// Token: 0x06004744 RID: 18244 RVA: 0x00259A90 File Offset: 0x00257E90
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

		// Token: 0x06004745 RID: 18245 RVA: 0x00259AF8 File Offset: 0x00257EF8
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
