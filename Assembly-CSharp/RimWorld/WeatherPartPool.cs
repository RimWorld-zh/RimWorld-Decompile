using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class WeatherPartPool
	{
		private static List<SkyOverlay> instances = new List<SkyOverlay>();

		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			for (int i = 0; i < WeatherPartPool.instances.Count; i++)
			{
				T val = (T)(WeatherPartPool.instances[i] as T);
				if (val != null)
				{
					return (SkyOverlay)(object)val;
				}
			}
			SkyOverlay skyOverlay = (SkyOverlay)(object)new T();
			WeatherPartPool.instances.Add(skyOverlay);
			return skyOverlay;
		}
	}
}
