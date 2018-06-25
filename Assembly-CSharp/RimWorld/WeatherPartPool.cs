using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000452 RID: 1106
	public static class WeatherPartPool
	{
		// Token: 0x04000BB3 RID: 2995
		private static List<SkyOverlay> instances = new List<SkyOverlay>();

		// Token: 0x0600133C RID: 4924 RVA: 0x000A5A5C File Offset: 0x000A3E5C
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
