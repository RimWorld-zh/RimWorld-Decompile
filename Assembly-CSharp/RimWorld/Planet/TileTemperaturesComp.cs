using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x020005AA RID: 1450
	public class TileTemperaturesComp : WorldComponent
	{
		// Token: 0x04001075 RID: 4213
		private TileTemperaturesComp.CachedTileTemperatureData[] cache;

		// Token: 0x04001076 RID: 4214
		private List<int> usedSlots;

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000EF126 File Offset: 0x000ED526
		public TileTemperaturesComp(World world) : base(world)
		{
			this.ClearCaches();
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000EF138 File Offset: 0x000ED538
		public override void WorldComponentTick()
		{
			for (int i = 0; i < this.usedSlots.Count; i++)
			{
				this.cache[this.usedSlots[i]].CheckCache();
			}
			if (Find.TickManager.TicksGame % 300 == 84 && this.usedSlots.Any<int>())
			{
				this.cache[this.usedSlots[0]] = null;
				this.usedSlots.RemoveAt(0);
			}
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000EF1C8 File Offset: 0x000ED5C8
		public float GetOutdoorTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetOutdoorTemp();
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x000EF1EC File Offset: 0x000ED5EC
		public float GetSeasonalTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetSeasonalTemp();
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000EF210 File Offset: 0x000ED610
		public float OutdoorTemperatureAt(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OutdoorTemperatureAt(absTick);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000EF234 File Offset: 0x000ED634
		public float OffsetFromDailyRandomVariation(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OffsetFromDailyRandomVariation(absTick);
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000EF258 File Offset: 0x000ED658
		public float AverageTemperatureForTwelfth(int tile, Twelfth twelfth)
		{
			return this.RetrieveCachedData(tile).AverageTemperatureForTwelfth(twelfth);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000EF27C File Offset: 0x000ED67C
		public bool SeasonAcceptableFor(int tile, ThingDef animalRace)
		{
			float seasonalTemp = this.GetSeasonalTemp(tile);
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000EF2C0 File Offset: 0x000ED6C0
		public bool OutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			float outdoorTemp = this.GetOutdoorTemp(tile);
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000EF304 File Offset: 0x000ED704
		public bool SeasonAndOutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			return this.SeasonAcceptableFor(tile, animalRace) && this.OutdoorTemperatureAcceptableFor(tile, animalRace);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000EF331 File Offset: 0x000ED731
		public void ClearCaches()
		{
			this.cache = new TileTemperaturesComp.CachedTileTemperatureData[Find.WorldGrid.TilesCount];
			this.usedSlots = new List<int>();
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x000EF354 File Offset: 0x000ED754
		private TileTemperaturesComp.CachedTileTemperatureData RetrieveCachedData(int tile)
		{
			TileTemperaturesComp.CachedTileTemperatureData result;
			if (this.cache[tile] != null)
			{
				result = this.cache[tile];
			}
			else
			{
				this.cache[tile] = new TileTemperaturesComp.CachedTileTemperatureData(tile);
				this.usedSlots.Add(tile);
				result = this.cache[tile];
			}
			return result;
		}

		// Token: 0x020005AB RID: 1451
		private class CachedTileTemperatureData
		{
			// Token: 0x04001077 RID: 4215
			private int tile;

			// Token: 0x04001078 RID: 4216
			private int tickCachesNeedReset = int.MinValue;

			// Token: 0x04001079 RID: 4217
			private float cachedOutdoorTemp = float.MinValue;

			// Token: 0x0400107A RID: 4218
			private float cachedSeasonalTemp = float.MinValue;

			// Token: 0x0400107B RID: 4219
			private float[] twelfthlyTempAverages;

			// Token: 0x0400107C RID: 4220
			private Perlin dailyVariationPerlinCached;

			// Token: 0x0400107D RID: 4221
			private const int CachedTempUpdateInterval = 60;

			// Token: 0x06001BAD RID: 7085 RVA: 0x000EF3A8 File Offset: 0x000ED7A8
			public CachedTileTemperatureData(int tile)
			{
				this.tile = tile;
				int seed = Gen.HashCombineInt(tile, 199372327);
				this.dailyVariationPerlinCached = new Perlin(5.0000000745058062E-06, 2.0, 0.5, 3, seed, QualityMode.Medium);
				this.twelfthlyTempAverages = new float[12];
				for (int i = 0; i < 12; i++)
				{
					this.twelfthlyTempAverages[i] = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				}
				this.CheckCache();
			}

			// Token: 0x06001BAE RID: 7086 RVA: 0x000EF454 File Offset: 0x000ED854
			public float GetOutdoorTemp()
			{
				return this.cachedOutdoorTemp;
			}

			// Token: 0x06001BAF RID: 7087 RVA: 0x000EF470 File Offset: 0x000ED870
			public float GetSeasonalTemp()
			{
				return this.cachedSeasonalTemp;
			}

			// Token: 0x06001BB0 RID: 7088 RVA: 0x000EF48C File Offset: 0x000ED88C
			public float OutdoorTemperatureAt(int absTick)
			{
				return this.CalculateOutdoorTemperatureAtTile(absTick, true);
			}

			// Token: 0x06001BB1 RID: 7089 RVA: 0x000EF4AC File Offset: 0x000ED8AC
			public float OffsetFromDailyRandomVariation(int absTick)
			{
				return (float)this.dailyVariationPerlinCached.GetValue((double)absTick, 0.0, 0.0) * 7f;
			}

			// Token: 0x06001BB2 RID: 7090 RVA: 0x000EF4E8 File Offset: 0x000ED8E8
			public float AverageTemperatureForTwelfth(Twelfth twelfth)
			{
				return this.twelfthlyTempAverages[(int)twelfth];
			}

			// Token: 0x06001BB3 RID: 7091 RVA: 0x000EF508 File Offset: 0x000ED908
			public void CheckCache()
			{
				if (this.tickCachesNeedReset <= Find.TickManager.TicksGame)
				{
					this.tickCachesNeedReset = Find.TickManager.TicksGame + 60;
					Map map = Current.Game.FindMap(this.tile);
					this.cachedOutdoorTemp = this.OutdoorTemperatureAt(Find.TickManager.TicksAbs);
					if (map != null)
					{
						this.cachedOutdoorTemp += map.gameConditionManager.AggregateTemperatureOffset();
					}
					this.cachedSeasonalTemp = this.CalculateOutdoorTemperatureAtTile(Find.TickManager.TicksAbs, false);
				}
			}

			// Token: 0x06001BB4 RID: 7092 RVA: 0x000EF59C File Offset: 0x000ED99C
			private float CalculateOutdoorTemperatureAtTile(int absTick, bool includeDailyVariations)
			{
				if (absTick == 0)
				{
					absTick = 1;
				}
				Tile tile = Find.WorldGrid[this.tile];
				float num = tile.temperature + GenTemperature.OffsetFromSeasonCycle(absTick, this.tile);
				if (includeDailyVariations)
				{
					num += this.OffsetFromDailyRandomVariation(absTick) + GenTemperature.OffsetFromSunCycle(absTick, this.tile);
				}
				return num;
			}
		}
	}
}
