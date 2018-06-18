using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CA3 RID: 3235
	public static class RoofUtility
	{
		// Token: 0x0600473A RID: 18234 RVA: 0x00258624 File Offset: 0x00256A24
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

		// Token: 0x0600473B RID: 18235 RVA: 0x002586A0 File Offset: 0x00256AA0
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

		// Token: 0x0600473C RID: 18236 RVA: 0x00258708 File Offset: 0x00256B08
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
