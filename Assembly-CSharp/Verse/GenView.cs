using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE6 RID: 3558
	public static class GenView
	{
		// Token: 0x040034D4 RID: 13524
		private static CellRect viewRect;

		// Token: 0x040034D5 RID: 13525
		private const int ViewRectMargin = 5;

		// Token: 0x06004FB9 RID: 20409 RVA: 0x00297298 File Offset: 0x00295698
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x002972BC File Offset: 0x002956BC
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

		// Token: 0x06004FBB RID: 20411 RVA: 0x00297320 File Offset: 0x00295720
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}
	}
}
