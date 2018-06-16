using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200028E RID: 654
	public abstract class BiomeWorker
	{
		// Token: 0x06000B16 RID: 2838
		public abstract float GetScore(Tile tile, int tileID);
	}
}
