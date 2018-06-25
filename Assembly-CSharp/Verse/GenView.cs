using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE7 RID: 3559
	public static class GenView
	{
		// Token: 0x040034DB RID: 13531
		private static CellRect viewRect;

		// Token: 0x040034DC RID: 13532
		private const int ViewRectMargin = 5;

		// Token: 0x06004FB9 RID: 20409 RVA: 0x00297578 File Offset: 0x00295978
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x0029759C File Offset: 0x0029599C
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

		// Token: 0x06004FBB RID: 20411 RVA: 0x00297600 File Offset: 0x00295A00
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}
	}
}
