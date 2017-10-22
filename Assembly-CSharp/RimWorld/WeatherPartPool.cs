using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class WeatherPartPool
	{
		private static List<SkyOverlay> instances = new List<SkyOverlay>();

		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			int num = 0;
			SkyOverlay result;
			while (true)
			{
				if (num < WeatherPartPool.instances.Count)
				{
					T val = (T)(WeatherPartPool.instances[num] as T);
					if (val != null)
					{
						result = (SkyOverlay)(object)val;
						break;
					}
					num++;
					continue;
				}
				SkyOverlay skyOverlay = (SkyOverlay)(object)new T();
				WeatherPartPool.instances.Add(skyOverlay);
				result = skyOverlay;
				break;
			}
			return result;
		}
	}
}
