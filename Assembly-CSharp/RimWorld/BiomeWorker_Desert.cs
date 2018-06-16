using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054C RID: 1356
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x0600194E RID: 6478 RVA: 0x000DBADC File Offset: 0x000D9EDC
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (tile.WaterCovered)
			{
				result = -100f;
			}
			else if (tile.rainfall >= 600f)
			{
				result = 0f;
			}
			else
			{
				result = tile.temperature + 0.0001f;
			}
			return result;
		}
	}
}
