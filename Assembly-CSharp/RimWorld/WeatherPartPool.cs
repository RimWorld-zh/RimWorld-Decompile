using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000452 RID: 1106
	public static class WeatherPartPool
	{
		// Token: 0x04000BB0 RID: 2992
		private static List<SkyOverlay> instances = new List<SkyOverlay>();

		// Token: 0x0600133D RID: 4925 RVA: 0x000A585C File Offset: 0x000A3C5C
		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			for (int i = 0; i < WeatherPartPool.instances.Count; i++)
			{
				T t = WeatherPartPool.instances[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			SkyOverlay skyOverlay = Activator.CreateInstance<T>();
			WeatherPartPool.instances.Add(skyOverlay);
			return skyOverlay;
		}
	}
}
