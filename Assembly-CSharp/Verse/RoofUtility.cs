using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	public static class RoofUtility
	{
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
