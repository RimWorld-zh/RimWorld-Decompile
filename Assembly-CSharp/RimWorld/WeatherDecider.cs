using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class WeatherDecider : IExposable
	{
		private const int FirstWeatherDuration = 10000;

		private Map map;

		private int curWeatherDuration = 10000;

		private int ticksWhenRainAllowedAgain;

		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		public void WeatherDeciderTick()
		{
			int num = this.curWeatherDuration;
			if (this.map.fireWatcher.LargeFireDangerPresent || !this.map.weatherManager.curWeather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				num = (int)((float)num * 0.25);
			}
			if (this.map.weatherManager.curWeatherAge > num)
			{
				this.StartNextWeather();
			}
		}

		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		private WeatherDef ChooseNextWeather()
		{
			if (TutorSystem.TutorialMode)
			{
				return WeatherDefOf.Clear;
			}
			WeatherDef result = default(WeatherDef);
			if (!DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight<WeatherDef>((Func<WeatherDef, float>)((WeatherDef w) => this.CurrentWeatherCommonality(w)), out result))
			{
				Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".");
				return WeatherDefOf.Clear;
			}
			return result;
		}

		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			if (!this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				return 0f;
			}
			if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				return 0f;
			}
			if ((int)weather.favorability < 2 && GenDate.DaysPassed < 8)
			{
				return 0f;
			}
			if (weather.rainRate > 0.10000000149011612 && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				return 0f;
			}
			if (weather.rainRate > 0.10000000149011612 && this.map.gameConditionManager.ActiveConditions.Any((Predicate<GameCondition>)((GameCondition x) => x.def.preventRain)))
			{
				return 0f;
			}
			BiomeDef biome = this.map.Biome;
			for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
			{
				WeatherCommonalityRecord weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
				if (weatherCommonalityRecord.weather == weather)
				{
					float num = weatherCommonalityRecord.commonality;
					if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.10000000149011612)
					{
						num = (float)(num * 20.0);
					}
					if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
					{
						num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
					}
					return num;
				}
			}
			return 0f;
		}

		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef item in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(item.label + " - " + this.CurrentWeatherCommonality(item).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
