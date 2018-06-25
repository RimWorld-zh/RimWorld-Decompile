using System;
using Verse;

namespace RimWorld
{
	public class ConfiguredTicksAbsAtGameStartCache
	{
		private int cachedTicks = -1;

		private int cachedForStartingTile = -1;

		private Season cachedForStartingSeason = Season.Undefined;

		public ConfiguredTicksAbsAtGameStartCache()
		{
		}

		public bool TryGetCachedValue(GameInitData initData, out int ticksAbs)
		{
			bool result;
			if (initData.startingTile == this.cachedForStartingTile && initData.startingSeason == this.cachedForStartingSeason)
			{
				ticksAbs = this.cachedTicks;
				result = true;
			}
			else
			{
				ticksAbs = -1;
				result = false;
			}
			return result;
		}

		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}
	}
}
