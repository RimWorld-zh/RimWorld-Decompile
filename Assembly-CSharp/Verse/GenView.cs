using System;
using UnityEngine;

namespace Verse
{
	public static class GenView
	{
		private static CellRect viewRect;

		private const int ViewRectMargin = 5;

		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		public static bool ShouldSpawnMotesAt(this IntVec3 loc, Map map)
		{
			bool result;
			if (map != Find.CurrentMap)
			{
				result = false;
			}
			else if (!loc.InBounds(map))
			{
				result = false;
			}
			else
			{
				GenView.viewRect = Find.CameraDriver.CurrentViewRect;
				GenView.viewRect = GenView.viewRect.ExpandedBy(5);
				result = GenView.viewRect.Contains(loc);
			}
			return result;
		}

		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}
	}
}
