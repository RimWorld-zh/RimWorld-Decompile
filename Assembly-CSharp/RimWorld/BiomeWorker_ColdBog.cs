using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000549 RID: 1353
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x06001948 RID: 6472 RVA: 0x000DBC0C File Offset: 0x000DA00C
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
			else if (tile.swampiness < 0.5f)
			{
				result = 0f;
			}
			else
			{
				result = -tile.temperature + 13f + tile.swampiness * 8f;
			}
			return result;
		}
	}
}
