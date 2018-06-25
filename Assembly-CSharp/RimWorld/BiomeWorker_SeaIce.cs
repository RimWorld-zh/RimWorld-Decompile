using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		private ModuleBase cachedSeaIceAllowedNoise;

		private int cachedSeaIceAllowedNoiseForSeed;

		public BiomeWorker_SeaIce()
		{
		}

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
	}
}
