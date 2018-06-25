using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000554 RID: 1364
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x04000ED8 RID: 3800
		private int cachedTicks = -1;

		// Token: 0x04000ED9 RID: 3801
		private int cachedForStartingTile = -1;

		// Token: 0x04000EDA RID: 3802
		private Season cachedForStartingSeason = Season.Undefined;

		// Token: 0x0600195F RID: 6495 RVA: 0x000DC4D0 File Offset: 0x000DA8D0
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

		// Token: 0x06001960 RID: 6496 RVA: 0x000DC51B File Offset: 0x000DA91B
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}
	}
}
