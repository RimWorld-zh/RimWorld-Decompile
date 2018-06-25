using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054C RID: 1356
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x0600194E RID: 6478 RVA: 0x000DBD5C File Offset: 0x000DA15C
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

		// Token: 0x0600194F RID: 6479 RVA: 0x000DBD90 File Offset: 0x000DA190
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
