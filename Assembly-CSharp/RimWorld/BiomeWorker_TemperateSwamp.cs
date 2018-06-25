using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000550 RID: 1360
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		// Token: 0x06001958 RID: 6488 RVA: 0x000DC010 File Offset: 0x000DA410
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
			else if (tile.swampiness < 0.5f)
			{
				result = 0f;
			}
			else
			{
				result = 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f + tile.swampiness * 3f;
			}
			return result;
		}
	}
}
