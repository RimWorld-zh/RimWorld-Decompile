using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000553 RID: 1363
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		// Token: 0x0600195F RID: 6495 RVA: 0x000DBF68 File Offset: 0x000DA368
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.temperature < 15f)
			{
				result = 0f;
			}
			else if (tile.rainfall < 2000f)
			{
				result = 0f;
			}
			else
			{
				result = 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f;
			}
			return result;
		}
	}
}
