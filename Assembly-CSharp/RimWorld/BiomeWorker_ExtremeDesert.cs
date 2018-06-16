using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054D RID: 1357
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x06001950 RID: 6480 RVA: 0x000DBB38 File Offset: 0x000D9F38
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
