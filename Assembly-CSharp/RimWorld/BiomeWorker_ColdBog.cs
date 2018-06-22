using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000547 RID: 1351
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x06001944 RID: 6468 RVA: 0x000DBABC File Offset: 0x000D9EBC
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
