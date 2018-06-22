using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE4 RID: 3556
	public static class GenView
	{
		// Token: 0x06004FB5 RID: 20405 RVA: 0x0029716C File Offset: 0x0029556C
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x00297190 File Offset: 0x00295590
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

		// Token: 0x06004FB7 RID: 20407 RVA: 0x002971F4 File Offset: 0x002955F4
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x040034D4 RID: 13524
		private static CellRect viewRect;

		// Token: 0x040034D5 RID: 13525
		private const int ViewRectMargin = 5;
	}
}
