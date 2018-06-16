using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CA4 RID: 3236
	public static class RoofUtility
	{
		// Token: 0x0600473C RID: 18236 RVA: 0x0025864C File Offset: 0x00256A4C
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

		// Token: 0x0600473D RID: 18237 RVA: 0x002586C8 File Offset: 0x00256AC8
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

		// Token: 0x0600473E RID: 18238 RVA: 0x00258730 File Offset: 0x00256B30
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
