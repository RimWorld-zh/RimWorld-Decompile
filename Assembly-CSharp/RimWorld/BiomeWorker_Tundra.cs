using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000555 RID: 1365
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x06001962 RID: 6498 RVA: 0x000DC064 File Offset: 0x000DA464
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
