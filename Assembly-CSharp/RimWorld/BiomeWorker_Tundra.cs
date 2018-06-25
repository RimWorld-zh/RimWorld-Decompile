using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000553 RID: 1363
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x0600195D RID: 6493 RVA: 0x000DC480 File Offset: 0x000DA880
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else
			{
				result = -tile.temperature;
			}
			return result;
		}
	}
}
