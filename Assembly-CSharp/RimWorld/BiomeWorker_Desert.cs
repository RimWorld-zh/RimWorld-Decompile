using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054A RID: 1354
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x0600194A RID: 6474 RVA: 0x000DBC90 File Offset: 0x000DA090
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.rainfall >= 600f)
			{
				result = 0f;
			}
			else
			{
				result = tile.temperature + 0.0001f;
			}
			return result;
		}
	}
}
