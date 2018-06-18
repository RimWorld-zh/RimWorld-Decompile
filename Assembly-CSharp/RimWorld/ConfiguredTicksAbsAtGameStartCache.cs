using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000556 RID: 1366
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x06001965 RID: 6501 RVA: 0x000DC108 File Offset: 0x000DA508
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

		// Token: 0x06001966 RID: 6502 RVA: 0x000DC153 File Offset: 0x000DA553
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x04000ED7 RID: 3799
		private int cachedTicks = -1;

		// Token: 0x04000ED8 RID: 3800
		private int cachedForStartingTile = -1;

		// Token: 0x04000ED9 RID: 3801
		private Season cachedForStartingSeason = Season.Undefined;
	}
}
