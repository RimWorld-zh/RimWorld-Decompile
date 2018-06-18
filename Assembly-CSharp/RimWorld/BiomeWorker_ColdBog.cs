using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054B RID: 1355
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x000DBAAC File Offset: 0x000D9EAC
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
