using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054A RID: 1354
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x0600194A RID: 6474 RVA: 0x000DBC0C File Offset: 0x000DA00C
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else
			{
				result = BiomeWorker_IceSheet.PermaIceScore(tile);
			}
			return result;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x000DBC40 File Offset: 0x000DA040
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
