using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000555 RID: 1365
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x06001963 RID: 6499 RVA: 0x000DC0B8 File Offset: 0x000DA4B8
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
