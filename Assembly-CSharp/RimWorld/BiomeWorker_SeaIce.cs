using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200054F RID: 1359
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		// Token: 0x06001956 RID: 6486 RVA: 0x000DBC60 File Offset: 0x000DA060
		public override float GetScore(Tile tile, int tileID)
		{
			float result;
			if (!tile.WaterCovered)
			{
				result = -100f;
			}
			else if (!this.AllowedAt(tileID))
			{
				result = -100f;
			}
			else
			{
				result = BiomeWorker_IceSheet.PermaIceScore(tile) - 23f;
			}
			return result;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000DBCB0 File Offset: 0x000DA0B0
		private bool AllowedAt(int tile)
		{
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			Vector3 viewCenter = Find.WorldGrid.viewCenter;
			float value = Vector3.Angle(viewCenter, tileCenter);
			float viewAngle = Find.WorldGrid.viewAngle;
			float num = Mathf.Min(7.5f, viewAngle * 0.12f);
			float num2 = Mathf.InverseLerp(viewAngle - num, viewAngle, value);
			bool result;
			if (num2 <= 0f)
			{
				result = true;
			}
			else
			{
				if (this.cachedSeaIceAllowedNoise == null || this.cachedSeaIceAllowedNoiseForSeed != Find.World.info.Seed)
				{
					this.cachedSeaIceAllowedNoise = new Perlin(0.017000000923871994, 2.0, 0.5, 6, Find.World.info.Seed, QualityMode.Medium);
					this.cachedSeaIceAllowedNoiseForSeed = Find.World.info.Seed;
				}
				float headingFromTo = Find.WorldGrid.GetHeadingFromTo(viewCenter, tileCenter);
				float num3 = (float)this.cachedSeaIceAllowedNoise.GetValue((double)headingFromTo, 0.0, 0.0) * 0.5f + 0.5f;
				result = (num2 <= num3);
			}
			return result;
		}

		// Token: 0x04000ED5 RID: 3797
		private ModuleBase cachedSeaIceAllowedNoise;

		// Token: 0x04000ED6 RID: 3798
		private int cachedSeaIceAllowedNoiseForSeed;
	}
}
