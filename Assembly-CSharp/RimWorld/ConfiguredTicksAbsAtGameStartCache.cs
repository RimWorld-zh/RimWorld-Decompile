using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000554 RID: 1364
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x04000ED4 RID: 3796
		private int cachedTicks = -1;

		// Token: 0x04000ED5 RID: 3797
		private int cachedForStartingTile = -1;

		// Token: 0x04000ED6 RID: 3798
		private Season cachedForStartingSeason = Season.Undefined;

		// Token: 0x06001960 RID: 6496 RVA: 0x000DC268 File Offset: 0x000DA668
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

		// Token: 0x06001961 RID: 6497 RVA: 0x000DC2B3 File Offset: 0x000DA6B3
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}
	}
}
