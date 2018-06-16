using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000454 RID: 1108
	public static class WeatherPartPool
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x000A56F0 File Offset: 0x000A3AF0
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

		// Token: 0x04000BB3 RID: 2995
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
