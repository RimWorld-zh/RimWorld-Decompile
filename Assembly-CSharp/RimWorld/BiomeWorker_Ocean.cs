using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054C RID: 1356
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x06001950 RID: 6480 RVA: 0x000DBDF8 File Offset: 0x000DA1F8
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (!tile.WaterCovered)
			{
				result = -100f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
