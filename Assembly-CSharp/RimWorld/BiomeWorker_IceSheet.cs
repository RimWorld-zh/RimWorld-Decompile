using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054C RID: 1356
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x000DBFC4 File Offset: 0x000DA3C4
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

		// Token: 0x0600194E RID: 6478 RVA: 0x000DBFF8 File Offset: 0x000DA3F8
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
