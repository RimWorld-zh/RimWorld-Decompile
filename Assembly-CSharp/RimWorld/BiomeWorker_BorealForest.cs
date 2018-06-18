using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054A RID: 1354
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x0600194B RID: 6475 RVA: 0x000DBA3C File Offset: 0x000D9E3C
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.temperature < -10f)
			{
				result = 0f;
			}
			else if (tile.rainfall < 600f)
			{
				result = 0f;
			}
			else
			{
				result = 15f;
			}
			return result;
		}
	}
}
