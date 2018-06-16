using System;

namespace Verse
{
	// Token: 0x02000CB7 RID: 3255
	public class WeatherEventMaker
	{
		// Token: 0x060047B1 RID: 18353 RVA: 0x0025B850 File Offset: 0x00259C50
		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1f / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, new object[]
				{
					map
				});
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}

		// Token: 0x0400309E RID: 12446
		public float averageInterval = 100f;

		// Token: 0x0400309F RID: 12447
		public Type eventClass = null;
	}
}
