using System;

namespace Verse
{
	public class WeatherEventMaker
	{
		public float averageInterval = 100f;

		public Type eventClass = null;

		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1.0 / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, map);
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}
	}
}
