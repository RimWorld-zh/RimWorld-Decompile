using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200054A RID: 1354
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x06001949 RID: 6473 RVA: 0x000DBEF8 File Offset: 0x000DA2F8
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
