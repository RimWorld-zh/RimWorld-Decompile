using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000552 RID: 1362
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x0600195C RID: 6492 RVA: 0x000DC118 File Offset: 0x000DA518
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

		// Token: 0x0600195D RID: 6493 RVA: 0x000DC163 File Offset: 0x000DA563
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x04000ED4 RID: 3796
		private int cachedTicks = -1;

		// Token: 0x04000ED5 RID: 3797
		private int cachedForStartingTile = -1;

		// Token: 0x04000ED6 RID: 3798
		private Season cachedForStartingSeason = Season.Undefined;
	}
}
