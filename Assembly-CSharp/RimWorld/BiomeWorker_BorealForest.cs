using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000548 RID: 1352
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x06001946 RID: 6470 RVA: 0x000DBB9C File Offset: 0x000D9F9C
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
			else if (tile.rainfall < 600f)
			{
				result = 0f;
			}
			else
			{
				result = 15f;
			}
			return result;
		}
	}
}
