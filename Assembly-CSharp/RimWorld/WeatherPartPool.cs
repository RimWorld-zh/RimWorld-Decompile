using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000450 RID: 1104
	public static class WeatherPartPool
	{
		// Token: 0x06001339 RID: 4921 RVA: 0x000A570C File Offset: 0x000A3B0C
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

		// Token: 0x04000BB0 RID: 2992
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
