using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000553 RID: 1363
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x0600195E RID: 6494 RVA: 0x000DC218 File Offset: 0x000DA618
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
