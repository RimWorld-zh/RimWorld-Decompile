using System;

namespace Verse
{
	// Token: 0x02000CB3 RID: 3251
	public class WeatherEventMaker
	{
		// Token: 0x060047B8 RID: 18360 RVA: 0x0025CC18 File Offset: 0x0025B018
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

		// Token: 0x040030A7 RID: 12455
		public float averageInterval = 100f;

		// Token: 0x040030A8 RID: 12456
		public Type eventClass = null;
	}
}
