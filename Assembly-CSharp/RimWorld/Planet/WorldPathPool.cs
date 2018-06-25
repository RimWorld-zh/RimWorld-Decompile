using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000544 RID: 1348
	public class WorldPathPool
	{
		// Token: 0x04000ED0 RID: 3792
		private List<WorldPath> paths = new List<WorldPath>(64);

		// Token: 0x04000ED1 RID: 3793
		private static readonly WorldPath notFoundPathInt = WorldPath.NewNotFound();

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x000DB9DC File Offset: 0x000D9DDC
		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000DB9F8 File Offset: 0x000D9DF8
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
	}
}
