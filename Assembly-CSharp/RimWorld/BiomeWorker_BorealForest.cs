using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000546 RID: 1350
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x06001942 RID: 6466 RVA: 0x000DBA4C File Offset: 0x000D9E4C
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
