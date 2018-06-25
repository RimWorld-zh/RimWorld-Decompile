using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054E RID: 1358
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x06001954 RID: 6484 RVA: 0x000DBF48 File Offset: 0x000DA348
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
