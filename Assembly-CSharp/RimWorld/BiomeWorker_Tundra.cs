using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000551 RID: 1361
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x0600195A RID: 6490 RVA: 0x000DC0C8 File Offset: 0x000DA4C8
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
