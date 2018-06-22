using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054D RID: 1357
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x06001952 RID: 6482 RVA: 0x000DBE30 File Offset: 0x000DA230
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
				result = 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f;
			}
			return result;
		}
	}
}
