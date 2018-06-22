using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000549 RID: 1353
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x06001948 RID: 6472 RVA: 0x000DBB9C File Offset: 0x000D9F9C
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
