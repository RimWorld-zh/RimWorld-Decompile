using System;

namespace Verse
{
	// Token: 0x02000CB6 RID: 3254
	public class WeatherEventMaker
	{
		// Token: 0x040030AE RID: 12462
		public float averageInterval = 100f;

		// Token: 0x040030AF RID: 12463
		public Type eventClass = null;

		// Token: 0x060047BB RID: 18363 RVA: 0x0025CFD4 File Offset: 0x0025B3D4
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
	}
}
