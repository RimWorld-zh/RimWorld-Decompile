using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054E RID: 1358
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x06001953 RID: 6483 RVA: 0x000DBBFC File Offset: 0x000D9FFC
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

		// Token: 0x06001954 RID: 6484 RVA: 0x000DBC30 File Offset: 0x000DA030
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
