using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE7 RID: 3559
	public static class GenView
	{
		// Token: 0x06004FA0 RID: 20384 RVA: 0x00295B90 File Offset: 0x00293F90
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00295BB4 File Offset: 0x00293FB4
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

		// Token: 0x06004FA2 RID: 20386 RVA: 0x00295C18 File Offset: 0x00294018
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x040034C9 RID: 13513
		private static CellRect viewRect;

		// Token: 0x040034CA RID: 13514
		private const int ViewRectMargin = 5;
	}
}
