using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054E RID: 1358
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x06001953 RID: 6483 RVA: 0x000DC1B0 File Offset: 0x000DA5B0
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (!tile.WaterCovered)
			{
				result = -100f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
