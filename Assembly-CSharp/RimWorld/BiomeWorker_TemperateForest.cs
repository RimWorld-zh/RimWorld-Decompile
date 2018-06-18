using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000551 RID: 1361
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x0600195B RID: 6491 RVA: 0x000DBE20 File Offset: 0x000DA220
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
