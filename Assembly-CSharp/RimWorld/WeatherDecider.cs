using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000449 RID: 1097
	public class WeatherDecider : IExposable
	{
		// Token: 0x04000B92 RID: 2962
		private Map map;

		// Token: 0x04000B93 RID: 2963
		private int curWeatherDuration = 10000;

		// Token: 0x04000B94 RID: 2964
		private int ticksWhenRainAllowedAgain = 0;

		// Token: 0x04000B95 RID: 2965
		private const int FirstWeatherDuration = 10000;

		// Token: 0x04000B96 RID: 2966
		private const float ChanceFactorRainOnFire = 15f;

		// Token: 0x06001307 RID: 4871 RVA: 0x000A4327 File Offset: 0x000A2727
		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x000A4349 File Offset: 0x000A2749
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x000A4370 File Offset: 0x000A2770
		public void WeatherDeciderTick()
		{
			int num = this.curWeatherDuration;
			bool flag = this.map.fireWatcher.LargeFireDangerPresent || !this.map.weatherManager.curWeather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp);
			if (flag)
			{
				num = (int)((float)num * 0.25f);
			}
			if (this.map.weatherManager.curWeatherAge > num)
			{
				this.StartNextWeather();
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000A43F8 File Offset: 0x000A27F8
		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000A4430 File Offset: 0x000A2830
		public void StartInitialWeather()
		{
			if (Find.GameInitData != null)
			{
				this.map.weatherManager.curWeather = WeatherDefOf.Clear;
				this.curWeatherDuration = 10000;
				this.map.weatherManager.curWeatherAge = 0;
			}
			else
			{
				this.map.weatherManager.curWeather = null;
				WeatherDef weatherDef = this.ChooseNextWeather();
				WeatherDef lastWeather = this.ChooseNextWeather();
				this.map.weatherManager.curWeather = weatherDef;
				this.map.weatherManager.lastWeather = lastWeather;
				this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
				this.map.weatherManager.curWeatherAge = Rand.Range(0, this.curWeatherDuration);
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000A44F0 File Offset: 0x000A28F0
		private WeatherDef ChooseNextWeather()
		{
			WeatherDef result;
			WeatherDef weatherDef;
			if (TutorSystem.TutorialMode)
			{
				result = WeatherDefOf.Clear;
			}
			else if (!DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight((WeatherDef w) => this.CurrentWeatherCommonality(w), out weatherDef))
			{
				Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".", false);
				result = WeatherDefOf.Clear;
			}
			else
			{
				result = weatherDef;
			}
			return result;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000A4563 File Offset: 0x000A2963
		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000A4578 File Offset: 0x000A2978
		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			float result;
			if (this.map.weatherManager.curWeather != null && !this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				result = 0f;
			}
			else if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				result = 0f;
			}
			else if (weather.favorability < Favorability.Neutral && GenDate.DaysPassed < 8)
			{
				result = 0f;
			}
			else if (weather.rainRate > 0.1f && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				result = 0f;
			}
			else
			{
				if (weather.rainRate > 0.1f)
				{
					if (this.map.gameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.preventRain))
					{
						return 0f;
					}
				}
				BiomeDef biome = this.map.Biome;
				for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
				{
					WeatherCommonalityRecord weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
					if (weatherCommonalityRecord.weather == weather)
					{
						float num = weatherCommonalityRecord.commonality;
						if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.1f)
						{
							num *= 15f;
						}
						if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
						{
							num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
						}
						return num;
					}
				}
				result = 0f;
			}
			return result;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x000A476C File Offset: 0x000A2B6C
		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef weatherDef in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(weatherDef.label + " - " + this.CurrentWeatherCommonality(weatherDef).ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
