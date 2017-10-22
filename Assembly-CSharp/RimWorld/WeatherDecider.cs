using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class WeatherDecider : IExposable
	{
		private Map map;

		private int curWeatherDuration = 10000;

		private int ticksWhenRainAllowedAgain = 0;

		private const int FirstWeatherDuration = 10000;

		private const float ChanceFactorRainOnFire = 15f;

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

		private WeatherDef ChooseNextWeather()
		{
			WeatherDef result;
			WeatherDef weatherDef = default(WeatherDef);
			if (TutorSystem.TutorialMode)
			{
				result = WeatherDefOf.Clear;
			}
			else if (!DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight<WeatherDef>((Func<WeatherDef, float>)((WeatherDef w) => this.CurrentWeatherCommonality(w)), out weatherDef))
			{
				Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".");
				result = WeatherDefOf.Clear;
			}
			else
			{
				result = weatherDef;
			}
			return result;
		}

		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			float result;
			WeatherCommonalityRecord weatherCommonalityRecord;
			if (this.map.weatherManager.curWeather != null && !this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				result = 0f;
			}
			else if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				result = 0f;
			}
			else if ((int)weather.favorability < 2 && GenDate.DaysPassed < 8)
			{
				result = 0f;
			}
			else if (weather.rainRate > 0.10000000149011612 && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				result = 0f;
			}
			else if (weather.rainRate > 0.10000000149011612 && this.map.gameConditionManager.ActiveConditions.Any((Predicate<GameCondition>)((GameCondition x) => x.def.preventRain)))
			{
				result = 0f;
			}
			else
			{
				BiomeDef biome = this.map.Biome;
				for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
				{
					weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
					if (weatherCommonalityRecord.weather == weather)
						goto IL_014d;
				}
				result = 0f;
			}
			goto IL_01e3;
			IL_014d:
			float num = weatherCommonalityRecord.commonality;
			if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.10000000149011612)
			{
				num = (float)(num * 15.0);
			}
			if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
			{
				num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
			}
			result = num;
			goto IL_01e3;
			IL_01e3:
			return result;
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
