using System;

namespace Verse
{
	// Token: 0x02000CB6 RID: 3254
	public class WeatherEventMaker
	{
		// Token: 0x060047AF RID: 18351 RVA: 0x0025B828 File Offset: 0x00259C28
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

		// Token: 0x0400309C RID: 12444
		public float averageInterval = 100f;

		// Token: 0x0400309D RID: 12445
		public Type eventClass = null;
	}
}
