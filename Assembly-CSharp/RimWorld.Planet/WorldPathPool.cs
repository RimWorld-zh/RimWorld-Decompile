using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class WorldPathPool
	{
		private List<WorldPath> paths = new List<WorldPath>(64);

		private static readonly WorldPath notFoundPathInt;

		public static WorldPath NotFoundPath
		{
			get
			{
				return WorldPathPool.notFoundPathInt;
			}
		}

		static WorldPathPool()
		{
			WorldPathPool.notFoundPathInt = WorldPath.NewNotFound();
		}

		public WorldPath GetEmptyWorldPath()
		{
			int num = 0;
			WorldPath result;
			while (true)
			{
				if (num < this.paths.Count)
				{
					if (!this.paths[num].inUse)
					{
						this.paths[num].inUse = true;
						result = this.paths[num];
						break;
					}
					num++;
					continue;
				}
				if (this.paths.Count > Find.WorldObjects.CaravansCount + 2 + (Find.WorldObjects.RoutePlannerWaypointsCount - 1))
				{
					Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788);
					this.paths.Clear();
				}
				WorldPath worldPath = new WorldPath();
				this.paths.Add(worldPath);
				worldPath.inUse = true;
				result = worldPath;
				break;
			}
			return result;
		}
	}
}
