using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000549 RID: 1353
	public class BiomeWorker_AridShrubland : BiomeWorker
	{
		// Token: 0x06001949 RID: 6473 RVA: 0x000DB998 File Offset: 0x000D9D98
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
			else if (tile.rainfall < 600f || tile.rainfall >= 2000f)
			{
				result = 0f;
			}
			else
			{
				result = 22.5f + (tile.temperature - 20f) * 2.2f + (tile.rainfall - 600f) / 100f;
			}
			return result;
		}
	}
}
