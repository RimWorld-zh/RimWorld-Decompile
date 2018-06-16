using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054E RID: 1358
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x06001952 RID: 6482 RVA: 0x000DBBA8 File Offset: 0x000D9FA8
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

		// Token: 0x06001953 RID: 6483 RVA: 0x000DBBDC File Offset: 0x000D9FDC
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
