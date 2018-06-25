using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054B RID: 1355
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x0600194C RID: 6476 RVA: 0x000DBCEC File Offset: 0x000DA0EC
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.rainfall >= 340f)
			{
				result = 0f;
			}
			else
			{
				result = tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
			}
			return result;
		}
	}
}
