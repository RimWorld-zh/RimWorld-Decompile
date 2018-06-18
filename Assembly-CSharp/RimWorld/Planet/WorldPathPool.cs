using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000546 RID: 1350
	public class WorldPathPool
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x000DB614 File Offset: 0x000D9A14
		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x000DB630 File Offset: 0x000D9A30
		public WorldPath GetEmptyWorldPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > Find.WorldObjects.CaravansCount + 2 + (Find.WorldObjects.RoutePlannerWaypointsCount - 1))
			{
				Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788, false);
				this.paths.Clear();
			}
			WorldPath worldPath = new WorldPath();
			this.paths.Add(worldPath);
			worldPath.inUse = true;
			return worldPath;
		}

		// Token: 0x04000ECF RID: 3791
		private List<WorldPath> paths = new List<WorldPath>(64);

		// Token: 0x04000ED0 RID: 3792
		private static readonly WorldPath notFoundPathInt = WorldPath.NewNotFound();
	}
}
