using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE8 RID: 3560
	public static class GenView
	{
		// Token: 0x06004FA2 RID: 20386 RVA: 0x00295BB0 File Offset: 0x00293FB0
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x00295BD4 File Offset: 0x00293FD4
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

		// Token: 0x06004FA4 RID: 20388 RVA: 0x00295C38 File Offset: 0x00294038
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x040034CB RID: 13515
		private static CellRect viewRect;

		// Token: 0x040034CC RID: 13516
		private const int ViewRectMargin = 5;
	}
}
